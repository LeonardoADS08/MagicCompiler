using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.Structures.Lexical
{
    public struct Symbol
    {
        public string Type { get; set; }
        public string Pattern { get; set; }
        public string TSymbol { get; set; }

        public Symbol(string type, string pattern, string tsymbol)
        {
            Type = type;
            Pattern = pattern;
            TSymbol = tsymbol;
        }

        #region Tests
        public void PrintSymbol(bool newLine = false)
        {
            string text = string.Format("{0} - {1} - {2}" , Type, Pattern, TSymbol);
            if (newLine) Console.WriteLine(text);
            else Console.Write(text);
        }
        #endregion
    }
}
