using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.Extensions.DependencyModel;
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
        public static readonly string FILE_DIRECTION_SCRIPTS = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Scripts");

        private Assembly _compiledAssembly = null;
        private ScriptParams _scriptParams;

        public ScriptEngine(ScriptParams parameters)
        {
            _scriptParams = parameters;
        }

        public Assembly Compile(bool recompile = false)
        {
            if (_compiledAssembly != null && !recompile) return _compiledAssembly;
            
            List<SyntaxTree> syntaxTrees = new List<SyntaxTree>();
            _scriptParams.Scripts.ForEach(script =>
            {
                using (StreamReader reader = new StreamReader(Path.Combine(FILE_DIRECTION_SCRIPTS, script)))
                {
                    syntaxTrees.Add(CSharpSyntaxTree.ParseText(reader.ReadToEnd()));
                }
            });
            
            string assemblyName = Path.GetRandomFileName();
            var refPaths = new List<string>(new[] {
                typeof(System.Object).GetTypeInfo().Assembly.Location,
                typeof(IQueryable).GetTypeInfo().Assembly.Location,
                typeof(Console).GetTypeInfo().Assembly.Location,
                Path.Combine(Path.GetDirectoryName(typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly.Location), "System.Runtime.dll"),
                Path.Combine(Path.GetDirectoryName(typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly.Location), "mscorlib.dll"),
                Path.Combine(Path.GetDirectoryName(typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly.Location), "netstandard.dll"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Magic Compiler Semantic Interface.dll"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Magic Compiler Structures.dll")
            });
            refPaths.AddRange(_scriptParams.References);

            List<MetadataReference> references = new List<MetadataReference>(refPaths.Select(r => MetadataReference.CreateFromFile(r))) ;
            references.AddRange(DependencyContext.Default.CompileLibraries.SelectMany(cl => cl.ResolveReferencePaths()).Select(asm => MetadataReference.CreateFromFile(asm)));
           
            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: syntaxTrees,
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
