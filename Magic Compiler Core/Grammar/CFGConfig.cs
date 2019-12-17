using MagicCompiler.Structures.Grammar;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.Grammar
{
    internal class CFGConfig
    {
        public Production AugmentedGrammar { get; set; }
        public string Epsilon { get; set; }
    }
}
