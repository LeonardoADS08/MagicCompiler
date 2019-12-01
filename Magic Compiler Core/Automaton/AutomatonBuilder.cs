using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using MagicCompiler.Grammar;

namespace MagicCompiler.Automaton
{
    public class AutomatonBuilder
    {
        public Dictionary<Rule, List<Item>> Automaton;
        private CFG KGrammar;

        public AutomatonBuilder()
        {
            Automaton = new Dictionary<Rule, List<Item>>();
            Reader reader = new Reader();
            KGrammar = reader.Build();
            ExtendGrammar();
            Build();
        }

        public void Build()
        {
            KGrammar.Productions.ForEach(x => ComputeItems(x));
        }

        private void ExtendGrammar()
        {
            KGrammar.Productions.Add(new Rule
            {
                Left = "$accepted",
                Right = { KGrammar.Configuration.StartSymbol }
            });
        }

        private void ComputeItems(Rule production)
        {
           
        }
    }
}
