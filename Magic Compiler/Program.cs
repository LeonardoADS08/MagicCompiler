using MagicCompiler.Grammar;
using MagicCompiler.Lexical;
using MagicCompiler.Automaton;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using MagicCompiler.Syntactic;

namespace Magic_Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            Lexer lexer = new Lexer(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data/input.txt"));
            //lexer.Analyze(x => x.PrintToken());

            Parser parser = new Parser(lexer);
            lexer.Analyze();
            parser.Check();
        }
    }
}
