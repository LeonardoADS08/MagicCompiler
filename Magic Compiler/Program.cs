using MagicCompiler.LexicalAnalyzer;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Magic_Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            Lexer lexer = new Lexer();
            lexer.Tokenize();
            
            for (int i = 0; i < lexer.Tokens.Count; i++)
            {
                Console.WriteLine(lexer.Tokens[i].Lexeme + " - " + lexer.Tokens[i].Symbol.Type);
            }
            Console.ReadKey();
        }

    }
}
