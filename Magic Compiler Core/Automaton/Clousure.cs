using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.Automaton
{
    public class Clousure
    {
        public List<Item> Items = new List<Item>();
        public Dictionary<string, Clousure> Goto = new Dictionary<string, Clousure>();
    }
}
