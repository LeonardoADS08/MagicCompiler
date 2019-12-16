using MCSI;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace MagicCompiler.Tools
{
    // TODO Customs scripts list
    public class ScriptEngine
    {
        private readonly string FILE_DIRECTION_SCRIPTS = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Scripts");
        private string _codeText;

        private Assembly _compiledAssembly = null;

        public ScriptEngine(string fileName)
        {
            using (var reader = new StreamReader(Path.Combine(FILE_DIRECTION_SCRIPTS, fileName)))
            {
                _codeText = reader.ReadToEnd();
            }
        }

        public Assembly Compile(bool recompile = false)
        {
            if (_compiledAssembly != null && !recompile) return _compiledAssembly;

            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(_codeText);
            
            // TODO custom references!
            string assemblyName = Path.GetRandomFileName();
            var refPaths = new[] {
                typeof(System.Object).GetTypeInfo().Assembly.Location,
                typeof(Console).GetTypeInfo().Assembly.Location,
                Path.Combine(Path.GetDirectoryName(typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly.Location), "System.Runtime.dll"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"MCSI.dll")
            };
            MetadataReference[] references = refPaths.Select(r => MetadataReference.CreateFromFile(r)).ToArray();

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var memoryStream = new MemoryStream())
            {
                EmitResult result = compilation.Emit(memoryStream);

                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    Console.ForegroundColor = ConsoleColor.Red;
                    foreach (Diagnostic diagnostic in failures)
                    {
                        Console.WriteLine("\t{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                        Console.WriteLine();
                    }
                    Console.ResetColor();
                }
                else
                {
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    Assembly assembly = AssemblyLoadContext.Default.LoadFromStream(memoryStream);
                    _compiledAssembly = assembly;
                    return assembly;
                }
            }

            return null;
        }

    }
}
