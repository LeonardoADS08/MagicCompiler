using MagicCompiler.Lexical;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Magic_Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            Lexer lexer = new Lexer(@"Data\input.txt");
            lexer.Analyze();
            Console.ReadKey();
        }
    }
}
