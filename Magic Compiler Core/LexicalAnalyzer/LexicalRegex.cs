using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MagicCompiler.LexicalAnalyzer
{
    public class LexicalRegex : Regex
    {
        public string RegularExpression { get => this.pattern; }

        private string _alias;
        public string Alias { get => _alias; }

        public LexicalRegex(string alias, string pattern) : base(pattern)
        {
            _alias = alias;
        }
    }
}
