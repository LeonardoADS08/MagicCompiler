using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.Automaton
{
    public class State
    {
        public int Order;
        public List<Item> CurrentItems;
        public List<Item> Closure = new List<Item>();
        public List<State> Goto = new List<State>();

        #region Tests
        public void PrintState()
        {
            Console.WriteLine("_____ State " + Order +" _____");
            CurrentItems.ForEach(x => x.PrintItem());
            Console.WriteLine("Closure:");
            Closure.ForEach(x => x.PrintItem());
            //Console.WriteLine("Goto:");
            //Goto.ForEach(x => x.PrintState());
        }
        #endregion
    }
}
