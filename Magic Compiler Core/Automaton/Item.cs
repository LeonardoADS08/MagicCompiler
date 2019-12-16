using MagicCompiler.Grammar;
using System;
using System.Collections.Generic;

namespace MagicCompiler.Automaton
{
    internal class Item
    {
        public Rule Production;
        public int DotPosition;

        #region Tests
        public void PrintItem()
        {
            Console.Write(Production.Left + " :== ");
            for (int i = 0; i < Production.Right.Count + 1; i++)
            {
                if (i == DotPosition)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("· ");
                    Console.ResetColor();
                }
                if (i < Production.Right.Count) Console.Write(Production.Right[i] + " ");
            }
            Console.WriteLine();
        }
        #endregion
    }
}
