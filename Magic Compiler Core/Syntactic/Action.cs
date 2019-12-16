using MagicCompiler.Automaton;
using MagicCompiler.Grammar;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.Syntactic
{
    internal class Action
    {
        public ActionType Type;
        public State Shift;
        public Rule Reduce;
    }
}
