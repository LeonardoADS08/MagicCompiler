using MagicCompiler;
using MagicCompiler.Grammars;
using MagicCompiler.Lexical;
using MagicCompiler.Syntactic;
using System.Collections.Generic;

namespace Magic_Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            var grammar = new LRGrammar();
            var tokenizer = new Tokenizer();
            var lexer = new LexerMC(grammar, tokenizer);
            var parser = new LRParser(lexer, grammar);
            var Compiler = new Compiler(lexer, parser);
            Compiler.Compile();
        }
    }
}
