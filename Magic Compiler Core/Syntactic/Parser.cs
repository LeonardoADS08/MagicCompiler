using MagicCompiler.Grammars;
using MagicCompiler.Lexical;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.Syntactic
{
    public abstract class Parser : IParser
    {
        protected ILexer _lexer;
        protected IGrammar _grammar;

        public Parser(ILexer lexer, IGrammar grammar)
        {
            _lexer = lexer;
            _grammar = grammar;
        }

        public abstract bool Check();
    }
}
