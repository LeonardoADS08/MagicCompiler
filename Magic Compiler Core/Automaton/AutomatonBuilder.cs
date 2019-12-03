using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MagicCompiler.Grammar;

namespace MagicCompiler.Automaton
{
    public class AutomatonBuilder
    {
        public State InitialState;

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
        }

        public void Build()
        {
            ExtendGrammar();
            KGrammar.Productions.ForEach(x => ComputeItems(x));
            var spItem = ProductionItems.FindAll(x => x.Production == StartingProduction && x.DotPosition == 0).ToList();
            InitialState = BuildStates(spItem, new List<List<Item>>());
            int order = 0;
            BFS(x =>
            {
                x.Order = order;
                x.PrintState();
                order++;
            });
        }

        private void ExtendGrammar()
        {
            StartingProduction = KGrammar.Configuration.AugmentedGrammar;
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

        private State BuildStates(List<Item> currentItems, List<List<Item>> analyzedItems)
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

            List<Item> closure = new List<Item>();
            currentItems.ForEach(x => closure.AddRange(Closure(x)));

            List<Item> closureAndItems = new List<Item>(closure);
            closureAndItems.AddRange(currentItems);

            List<State> GotoStates = new List<State>();
            var groupsGoto = GroupGoto(closureAndItems);
            foreach (var val in groupsGoto)
            {
                var resultState = BuildStates(val.Value, analyzedItems);
                if (resultState != null) GotoStates.Add(resultState);
            }

            State actualState = new State()
            {
                CurrentItems = currentItems,
                Closure = closure,
                Goto = GotoStates
            };

            return actualState;
        }

        public void BFS(Action<State> action)
        {
            Queue<State> queue = new Queue<State>();
            HashSet<State> visitedStates = new HashSet<State>();
            queue.Enqueue(InitialState);
            while (queue.Count != 0)
            {
                var actualState = queue.Dequeue();

                if (visitedStates.Contains(actualState)) continue;
                else visitedStates.Add(actualState);

                // Execute action
                action?.Invoke(actualState);

                for (int i = 0; i < actualState.Goto.Count; i++)
                {
                    queue.Enqueue(actualState.Goto[i]);
                }
            }
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