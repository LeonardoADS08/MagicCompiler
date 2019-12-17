using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.Structures.Grammar
{
    public class Production
    {
        public string Left;
        public List<string> Right = new List<string>();

        public override string ToString()
        {
            var result = string.Format("{0} ::=", Left);
            Right.ForEach(x => result += " " + x);
            return result;
        }

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
        #endregion
    }
}
