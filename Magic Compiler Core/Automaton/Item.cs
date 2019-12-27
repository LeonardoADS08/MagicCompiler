using MagicCompiler.Grammar;
using MagicCompiler.Structures.Grammar;
using System;
using System.Collections.Generic;

namespace MagicCompiler.Automaton
{
    internal class Item
    {
        public Production Production;
        public int DotPosition;

        public override string ToString()
        {
            string res = Production.Left + " :== ";
            for (int i = 0; i < Production.Right.Count + 1; i++)
            {
                if (i == DotPosition)
                {
                    res += "· ";
                }
                if (i < Production.Right.Count) res += Production.Right[i] + " ";
            }
            return res;
        }

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
