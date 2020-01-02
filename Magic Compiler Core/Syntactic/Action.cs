using MagicCompiler.Automaton;
using MagicCompiler.Grammars;
using MagicCompiler.Structures.Grammar;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.Syntactic
{
    internal class Action
    {
        public ActionType Type;
        public State Shift;
        public Production Reduce;
    }
}
