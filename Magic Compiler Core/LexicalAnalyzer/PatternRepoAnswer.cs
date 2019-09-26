using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.LexicalAnalyzer
{
    public struct PatternRepoAnswer
    {
        private readonly static PatternRepoAnswer _instanceNullAnswer = new PatternRepoAnswer(false, Token.NULL_TOKEN);
        public static PatternRepoAnswer NULL_ANSWER => _instanceNullAnswer;

        public bool Valid { get; set; }
        public Token Token { get; set; }

        public PatternRepoAnswer(bool valid, Token token)
        {
            Valid = valid;
            Token = token;
        }
    }
}
