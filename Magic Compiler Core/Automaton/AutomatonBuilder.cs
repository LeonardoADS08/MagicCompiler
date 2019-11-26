using System;
using System.Collections.Generic;
using System.Text;

using MagicCompiler.Grammar;

namespace MagicCompiler.Automaton
{
    public class AutomatonBuilder
    {
        public CFG KGrammar;
        public HashSet<string> NonTerminals;
        public HashSet<string> Terminals;

        public AutomatonBuilder()
        {
            Reader reader = new Reader();
            KGrammar = reader.Build();
            NonTerminals = new HashSet<string>();
            Terminals = new HashSet<string>();

            // used to fill NonTerminals and Terminals hashSets
            CategorizeSymbols();

            /*Console.WriteLine("Non Terminals Found: ");
            foreach(string nonTerminal in NonTerminals)
            {
                Console.WriteLine(nonTerminal);
            }
            Console.WriteLine("Terminals Found: ");
            foreach (string Terminal in Terminals)
            {
                Console.WriteLine(Terminal);
            }*/

            AugmentGrammar();

            HashSet<Item> allItems = ComputeAllItems();
            int kernel = 0, nonKernel = 0;

            HashSet<Item> I = new HashSet<Item>();

            // prints all items with their dots
            foreach(Item item in allItems)
            {
                if (KGrammar.Productions[item.ProductionId].Left == "$accepted" || item.DotPosition != 0) ++kernel;
                else ++nonKernel;

                if (KGrammar.Productions[item.ProductionId].Left == "$accepted" && item.DotPosition == 0)
                    I.Add(item);

                Console.Write(this.KGrammar.Productions[item.ProductionId].Left + " ::= ");
                for(int j = 0; j < this.KGrammar.Productions[item.ProductionId].Right.Count; ++j)
                {
                    if (item.DotPosition == j) Console.Write('.');
                    Console.Write(this.KGrammar.Productions[item.ProductionId].Right[j]);
                }
                if (item.DotPosition == this.KGrammar.Productions[item.ProductionId].Right.Count) Console.Write('.');
                Console.Write('\n');
            }
            Console.WriteLine("Kernel items: " + kernel);
            Console.WriteLine("Non-Kernel items: " + nonKernel);

            HashSet<Item> ans = Closure(I);

            foreach (Item item in ans)
            {
                Console.Write(KGrammar.Productions[item.ProductionId].Left + " ::= ");
                for (int j = 0; j < KGrammar.Productions[item.ProductionId].Right.Count; ++j)
                {
                    if (item.DotPosition == j) Console.Write('.');
                    Console.Write(KGrammar.Productions[item.ProductionId].Right[j]);
                }
                if (item.DotPosition == KGrammar.Productions[item.ProductionId].Right.Count) Console.Write('.');
                Console.Write('\n');
            }
            Console.WriteLine("Done writing Closure");
        }

        private void CategorizeSymbols()
        {
            Console.WriteLine("Entered function");
            foreach(Rule rule in KGrammar.Productions)
            {
                Console.WriteLine(rule.Left);
                NonTerminals.Add(rule.Left);
            }

            foreach(Rule rule in KGrammar.Productions)
            {
                for(int i = 0; i < rule.Right.Count; ++i)
                {
                    if(!NonTerminals.Contains(rule.Right[i]))
                    {
                        Terminals.Add(rule.Right[i]);
                    }
                }
            }
        }

        private void AugmentGrammar()
        {
            Rule accepted = new Rule() { Left = "$accepted" };
            accepted.Right.Add(KGrammar.Configuration.StartSymbol);
            this.KGrammar.Productions.Add(accepted);
        }

        private HashSet<Item> ComputeAllItems()
        {
            HashSet<Item> items = new HashSet<Item>();
            int id = 1;
            for (int i = 0; i < this.KGrammar.Productions.Count; ++i)
            {
                for (int k = 0; k <= this.KGrammar.Productions[i].Right.Count; ++k, ++id)
                {
                    Item item = new Item();
                    item.ProductionId = i;
                    item.DotPosition = k;
                    items.Add(item);
                }
            }

            return items;
        }

        private bool 

        private HashSet<Item> Closure(HashSet<Item> I)
        {
            HashSet<Item> closure = new HashSet<Item>(I);
            bool added;

            do
            {
                added = false;
                foreach (Item item in closure)
                {
                    if (item.DotPosition < KGrammar.Productions[item.ProductionId].Right.Count &&
                        NonTerminals.Contains(KGrammar.Productions[item.ProductionId].Right[item.DotPosition]))
                    {
                        foreach (Rule gamma in KGrammar.Productions)
                        {
                            if (gamma.Left == KGrammar.Productions[item.ProductionId].Right[item.DotPosition])
                            {
                                added = true;
                                Item aux = new Item();
                                closure.Add()
                            }
                        }
                    }
                }
            } while (added == true);


            return closure;
        }
    }
}
