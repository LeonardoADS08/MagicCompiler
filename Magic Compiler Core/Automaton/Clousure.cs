using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.Automaton
{
    public class Closure
    {
        public List<Item> Items = new List<Item>();
        public Dictionary<string, Closure> Goto = new Dictionary<string, Closure>();


    }
}