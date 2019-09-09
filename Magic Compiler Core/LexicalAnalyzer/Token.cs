using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.LexicalAnalyzer
{
    public struct Token
    {
        public static Token INVALID_TOKEN => new Token("[NULL]", "[NULL]");

        public string Lexeme;
        public string Value;

        public Token(string lexeme, string value)
        {
            Lexeme = lexeme;
            Value = value;
        }
    }
}
