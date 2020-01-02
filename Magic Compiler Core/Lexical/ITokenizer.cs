using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.Lexical
{
    public interface ITokenizer
    {
        TokenizerAnswer MatchToken(string word);
    }
}
