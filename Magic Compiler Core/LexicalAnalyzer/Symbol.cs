using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.LexicalAnalyzer
{
    public struct Symbol
    {
        public string Type { get; set; }
        public string Pattern { get; set; }


        public Symbol(string type, string pattern)
        {
            Type = type;
            Pattern = pattern;
        }
    }
}
