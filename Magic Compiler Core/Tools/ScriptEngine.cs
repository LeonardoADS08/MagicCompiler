using MagicCompiler.Structures.Lexical;
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
            string gcLocation = Path.GetDirectoryName(typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly.Location);
            var refPaths = new List<string>(new[] {
                typeof(Object).GetTypeInfo().Assembly.Location,
                typeof(IQueryable).GetTypeInfo().Assembly.Location,
                typeof(Console).GetTypeInfo().Assembly.Location,
                typeof(Enumerable).GetTypeInfo().Assembly.Location,
                Path.Combine(gcLocation, "System.dll"),
                Path.Combine(gcLocation, "System.Core.dll"),
                Path.Combine(gcLocation, "System.IO.dll"),
                Path.Combine(gcLocation, "System.Runtime.dll"),
                Path.Combine(gcLocation, "System.Runtime.Extensions.dll"),
                Path.Combine(gcLocation, "System.Collections.dll"),
                Path.Combine(gcLocation, "mscorlib.dll"),
                Path.Combine(gcLocation, "netstandard.dll"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Magic Compiler Interface.dll"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Magic Compiler Structures.dll")
            });
            refPaths.AddRange(_scriptParams.References);
            refPaths = refPaths.Distinct().ToList();

            List<PortableExecutableReference> references = new List<PortableExecutableReference>();
            references.AddRange(refPaths.Select(r => MetadataReference.CreateFromFile(r)));

            CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName,
            syntaxTrees: syntaxTrees,
            references: references,
            options: new CSharpCompilationOptions(
                        OutputKind.DynamicallyLinkedLibrary,
                        platform: Platform.AnyCpu,
                        assemblyIdentityComparer: DesktopAssemblyIdentityComparer.Default,
                        optimizationLevel: OptimizationLevel.Release));

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
                        Console.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
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
