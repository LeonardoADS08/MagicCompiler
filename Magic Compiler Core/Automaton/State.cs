using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.Automaton
{
    internal class State
    {
        public int Order;
        public List<Item> CurrentItems;
        public List<Item> Closure = new List<Item>();
        public Dictionary<string, State> Goto = new Dictionary<string, State>();

        #region Tests
        public void PrintState()
        {
            Console.WriteLine("_____ State " + Order +" _____");
            CurrentItems.ForEach(x => x.PrintItem());
            Console.WriteLine("Closure:");
            Closure.ForEach(x => x.PrintItem());
            Console.WriteLine("Goto:");
            foreach (var val in Goto)
            {
                Console.Write(val.Value.Order + " ");
            }
            Console.WriteLine();
        }
        #endregion
    }
}
