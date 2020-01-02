using MagicCompiler.Grammars;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MagicCompiler.Lexical
{
    public abstract class Lexer : ILexer
    {
        public abstract List<Token> Tokens { get; }

        protected ITokenizer _tokenizer;
        private int _nextIterator = 0;

        public Lexer(ITokenizer tokenizer) 
        {
            _tokenizer = tokenizer;
        }

        public abstract void Analyze();

        public Token Next()
        {
            if (_nextIterator + 1 < Tokens.Count)
            {
                var result = Tokens[_nextIterator];
                _nextIterator++;
                return result;
            }
            else return Tokens[_nextIterator];
        }

        public void Reset() => _nextIterator = 0;
    }
}
