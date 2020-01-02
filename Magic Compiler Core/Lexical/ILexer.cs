using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.Lexical
{
    public interface ILexer
    {
        public List<Token> Tokens { get; }
        void Analyze();
        Token Next();
        void Reset();
    }
}
