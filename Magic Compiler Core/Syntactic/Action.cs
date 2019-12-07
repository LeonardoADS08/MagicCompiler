﻿using MagicCompiler.Automaton;
using MagicCompiler.Grammar;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.Syntactic
{
    public class Action
    {
        public ActionType Type;
        public State Shift;
        public Rule Reduce;
    }
}