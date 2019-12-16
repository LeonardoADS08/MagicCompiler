using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.Grammar
{
    internal class CFGConfig
    {
        public Rule AugmentedGrammar { get; set; }
        public string Epsilon { get; set; }
    }
}
