using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.Grammar
{
    internal class Rule
    {
        public string Left;
        public List<string> Right = new List<string>();

        #region Tests
        public void PrintRule(bool newLine = true)
        {
            Console.Write(Left + " ::= ");
            for (int i = 0; i < Right.Count; i++)
            {
                Console.Write(Right[i] + " ");
            }
            if (newLine) Console.WriteLine("");
        }

        public string RuleToString()
        {
            var result = string.Format("{0} ::=", Left);
            Right.ForEach(x => result += " " + x);
            return result;
        }
        #endregion
    }
}
