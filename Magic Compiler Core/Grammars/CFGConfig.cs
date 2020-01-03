using MagicCompiler.Structures.Grammar;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.Grammars
{
    public class CFGConfig
    {
        public Production AugmentedProduction { get; set; }
        public string Epsilon { get; set; }
    }
}
