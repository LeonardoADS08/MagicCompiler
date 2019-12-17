using MagicCompiler.Syntactic;
using System.Collections.Generic;

namespace Magic_Compiler
{
    class Program
    {
        static void Main(string[] args)
        {

            Parser parser = new Parser();
            parser.Check();
            List<int> a = new List<int>();
            //SemanticScriptLoader sc = new SemanticScriptLoader();
            //var instancia = sc.GetSemanticAnalyzer();
            //if (instancia.Analyze()) Console.WriteLine("WORKS!");
        }
    }
}
