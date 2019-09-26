using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.Lexical
{
    internal struct TokenizerAnswer
    {
        private readonly static TokenizerAnswer _instanceNullAnswer = new TokenizerAnswer(false, Token.NULL_TOKEN);
        public static TokenizerAnswer NULL_ANSWER => _instanceNullAnswer;

        public bool Valid { get; set; }
        public Token Token { get; set; }

        public TokenizerAnswer(bool valid, Token token)
        {
            Valid = valid;
            Token = token;
        }
    }
}
