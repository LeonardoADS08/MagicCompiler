using MagicCompiler.Grammar;
using MagicCompiler.Lexical;
using MagicCompiler.Automaton;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using MagicCompiler.Syntactic;
using MagicCompiler.Tools;
using MCSI;

namespace Magic_Compiler
{
    class Program
    {
        static void Main(string[] args)
        {

            Parser parser = new Parser();
            parser.Check();

            ScriptEngine sc = new ScriptEngine("semanticScripts.txt");
            var assembly = sc.Compile();
            var instancia = assembly.CreateInstance<ISemanticAnalyzer>("MagicCompilerScripts.SemanticScript");
            if (instancia.Analyze()) Console.WriteLine("WORKS!");
        }
    }
}
