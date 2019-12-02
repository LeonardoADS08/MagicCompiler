using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MagicCompiler.Grammar;

namespace MagicCompiler.Automaton
{
    public class AutomatonBuilder
    {
        private Dictionary<Rule, List<Item>> ProductionItems;
        private CFG KGrammar;

        public AutomatonBuilder()
        {
            ProductionItems = new Dictionary<Rule, List<Item>>();
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

        private bool isNonTerminal(String symbol)
        {
            
            foreach(Rule regla in KGrammar.Productions)
            {
                if (regla.Left == symbol)
                {
                    return true;
                }
            }
            return false;
        }

        
        private void ComputeItems(Rule production)
        {
            List<Item> items = new List<Item>();
            for (int i = 0; i < production.Right.Count; i++)
            {
                items.Add(new Item()
                {
                    Production = production,
                    DotPosition = i
                });
            }
            ProductionItems.Add(production, items);
        }

      /* private void Closure(State estau)
        {
           foreach(Item iten in estau.ItemSet)
            {
                if (isNonTerminal(iten.Production.Right[iten.DotPosition]))
                {

                }
            }
        }*/

        #region Tests
        public void PrintAllItems()
        {
            foreach (var val in ProductionItems)
            {
                val.Key.PrintRule();
                foreach (var prod in val.Value)
                {
                    Console.Write(prod.Production.Left + " :== ");
                    for (int i = 0; i < prod.Production.Right.Count; i++)
                    {
                        if (i == prod.DotPosition) Console.Write("·");
                        Console.Write(prod.Production.Right[i] + " ");
                    }
                    Console.WriteLine("");
                }
                Console.WriteLine("");
            }
        }
        #endregion
    }
}