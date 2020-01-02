using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.Structures.Lexical
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

        public override string ToString() => Type;

        #region Tests
        public void PrintSymbol(bool newLine = false)
        {
            string text = string.Format("{0} - {1}" , Type, Pattern);
            if (newLine) Console.WriteLine(text);
            else Console.Write(text);
        }
        #endregion
    }
}
