using MagicCompiler.Structures.Grammar;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.Grammars
{
    public interface IGrammar
    {
        Production AugmentedGrammar { get; }
        List<Production> Productions { get; }

        List<string> Terminals { get; }
        List<string> NonTerminals { get; }

        Dictionary<string, List<string>> First { get; }
        Dictionary<string, List<string>> Follow { get; }

        bool IsTerminal(string symbol);
        bool IsNonTerminal(string symbol);
    }
}
