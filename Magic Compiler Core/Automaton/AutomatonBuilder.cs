using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MagicCompiler.Grammar;

namespace MagicCompiler.Automaton
{
    public class AutomatonBuilder
    {
        private List<Item> ProductionItems;
        private CFG KGrammar;
        private Rule StartingProduction;

        private List<Item> NonKernelItems => ProductionItems.Where(x => x.DotPosition == 0 && x.Production != StartingProduction).ToList();
        private List<Item> KernelItems => ProductionItems.Where(x => x.DotPosition != 0 || x.Production == StartingProduction).ToList();
        private List<Item> AugmentedItems => ProductionItems.Where(x => x.Production == StartingProduction).ToList();

        public AutomatonBuilder()
        {
            ProductionItems = new List<Item>();
            Reader reader = new Reader();
            KGrammar = reader.Build();
            Build();

            var spItem = ProductionItems.FindAll(x => x.Production == StartingProduction && x.DotPosition == 0).ToList();
            BuildStates(spItem, new List<State>());
           // PrintAllItems();
           //             var result = Closure(spItem);
           //             Console.WriteLine();
           //             result.ForEach(x => x.PrintItem());
           //             Console.WriteLine();
           //             result = NextDotItems(result);
           //             result.ForEach(x => x.PrintItem());
           //             Console.WriteLine();
           //             result = NextStateValidItems("e", result);
           //             result.ForEach(x => x.PrintItem());
           //var result = BuildStates(spItem, new List<Item>());
           //result.PrintState();

        }

        public void Build()
        {
            ExtendGrammar();
            KGrammar.Productions.ForEach(x => ComputeItems(x));
        }

        private void ExtendGrammar()
        {
            StartingProduction = new Rule
            {
                Left = "$accepted",
                Right = { KGrammar.Configuration.StartSymbol }
            };
            KGrammar.Productions.Add(StartingProduction);
        }

        private bool IsNonTerminal(string symbol) => KGrammar.Productions.Exists(x => x.Left == symbol);

        private void ComputeItems(Rule production)
        {
            List<Item> items = new List<Item>();
            for (int i = 0; i <= production.Right.Count; i++)
            {
                items.Add(new Item()
                {
                    Production = production,
                    DotPosition = i
                });
            }
            ProductionItems.AddRange(items);
        }

        private List<Item> Closure(Item item)
        {
            List<Item> result = new List<Item>(),
                       analyzableItems = new List<Item>() { item },
                       nonKernelItems = NonKernelItems;
            HashSet<string> analyzedSymbols = new HashSet<string>();

            while (analyzableItems.Count != 0)
            {
                var currentItem = analyzableItems[0];
                analyzableItems.RemoveAt(0);

                // Si el punto no se encuentra al final y es no terminal y no ha sido analizado todavía
                if (currentItem.DotPosition < currentItem.Production.Right.Count && 
                    //IsNonTerminal(currentItem.Production.Right[currentItem.DotPosition]) && 
                    !analyzedSymbols.Contains(currentItem.Production.Right[currentItem.DotPosition]))
                {
                    string actualSymbol = currentItem.Production.Right[currentItem.DotPosition];

                    // Se lo considera un simbolo analizado
                    analyzedSymbols.Add(actualSymbol);

                    // Se agregan al resultado todos los items que serán parte del closure
                    var partialClosure = nonKernelItems.Where(x => x.Production.Left == actualSymbol).ToList();
                    result.AddRange(partialClosure);
                    // Se eliminan todos los items agregados de los posibles elegibles al closure
                    nonKernelItems.RemoveAll(x => partialClosure.Contains(x));

                    // Se agregan como posibles items analizables los nuevos elementos
                    analyzableItems.AddRange(partialClosure);
                }
            }
            return result;
        }

        // Devuelve los items con un punto adelante, siempre y cuando se permita un punto adelante
        private List<Item> NextDotItems(List<Item> items)
        {
            List<Item> result = new List<Item>();
            items.ForEach(x => result.AddRange(ProductionItems.Where(y => y.Production == x.Production && x.DotPosition + 1 == y.DotPosition)));
            return result;
        }
        
        // Devuelve los items validos para un simbolo dado
        private List<Item> NextStateValidItems(string symbol, List<Item> productions)
        {
            productions.RemoveAll(x => x.DotPosition >= x.Production.Right.Count || x.DotPosition == 0);
            return productions.Where(x => x.Production.Right[x.DotPosition - 1] == symbol).ToList();
        }

        private Dictionary<string, List<Item>> GroupGoto(List<Item> items)
        {
            Dictionary<string, List<Item>> result = new Dictionary<string, List<Item>>();

            items.RemoveAll(x => x.DotPosition >= x.Production.Right.Count);

            items.ForEach(x =>
            {
                string actualSymbol = x.Production.Right[x.DotPosition];
                if (!result.ContainsKey(actualSymbol))
                    result.Add(actualSymbol, new List<Item>());

                result[actualSymbol].AddRange(NextDotItems(new List<Item>() { x }));
            });

            List<string> keysToDelete = new List<string>();
            foreach (var val in result)
            {
                if (val.Value.Count == 0) keysToDelete.Add(val.Key);
            }

            keysToDelete.ForEach(x => result.Remove(x));
            return result;
        }

        private State ExistState(List<Item> items, List<State> states)
        {
            foreach (var val in states)
            {
                val.CurrentItems.TrueForAll(x => items.Contains(x));
                return val;
            }
            return null;
        }

        private List<List<Item>> analyzedItems = new List<List<Item>>();
        private int StateCount = 0;
        private State BuildStates(List<Item> currentItems, List<State> states)
        {
            for (int i = 0; i < analyzedItems.Count; i++)
            {
                bool exists = true;
                for (int j = 0; exists && j < analyzedItems[i].Count; j++)
                {
                    if (!currentItems.Contains(analyzedItems[i][j])) exists = false;
                }
                if (exists) return null;
            }
            analyzedItems.Add(currentItems);

            int stateOrderHere = StateCount;
            StateCount++;

            List<Item> closure = new List<Item>();
            currentItems.ForEach(x => closure.AddRange(Closure(x)));

            List<Item> closureAndItems = new List<Item>(closure);
            closureAndItems.AddRange(currentItems);

            List<State> GotoStates = new List<State>();
            var groupsGoto = GroupGoto(closureAndItems);
            foreach (var val in groupsGoto)
            {
                var resultState = BuildStates(val.Value, states);
                if (resultState != null) GotoStates.Add(resultState);
            }

            State actualState = new State()
            {
                Order = stateOrderHere,
                CurrentItems = currentItems,
                Closure = closure,
                Goto = GotoStates
            };
            actualState.PrintState();
            states.Add(actualState);

            return actualState;
        }

        #region Tests
        public void PrintAllItems()
        {
            foreach (var val in ProductionItems)
            {
                val.PrintItem();
            }
        }

        public void ClosureTest()
        {
            KernelItems.ForEach(y =>
            {
                Console.WriteLine("Closure of: ");
                y.PrintItem();
                Console.WriteLine("result: ");

                var closure = Closure(y);
                closure.ForEach(x => x.PrintItem());
                Console.WriteLine();
            });
        }
        #endregion
    }
}