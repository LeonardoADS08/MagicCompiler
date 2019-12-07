using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.Grammar
{
    public class Rule
    {
        public string Left;
        public List<string> Right = new List<string>();

        #region Tests
        public void PrintRule(bool newLine = true)
        {
            Console.Write(Left + " :== ");
            for (int i = 0; i < Right.Count; i++)
            {
                Console.Write(Right[i] + " ");
            }
            if (newLine) Console.WriteLine("");
        }
        #endregion
    }
}
