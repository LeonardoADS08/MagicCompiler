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
        public CFG KGrammar;

        private List<Item> _productionItems;
        private Rule _startingProduction;

        private List<Item> NonKernelItems => _productionItems.Where(x => x.DotPosition == 0 && x.Production != _startingProduction).ToList();
        private List<Item> KernelItems => _productionItems.Where(x => x.DotPosition != 0 || x.Production == _startingProduction).ToList();

        public AutomatonBuilder()
        {
            _productionItems = new List<Item>();
            Reader reader = new Reader();
            KGrammar = CFG.Instance;
        }

        public void Build()
        {
            ExtendGrammar();
            KGrammar.Productions.ForEach(x => ComputeItems(x));
            var spItem = _productionItems.FindAll(x => x.Production == _startingProduction && x.DotPosition == 0).ToList();
            InitialState = BuildStates(spItem, new List<State>());
            int order = 0;
            BFS(x =>
            {
                x.Order = order;
                order++;
            });
        }

        private void ExtendGrammar()
        {
            _startingProduction = KGrammar.Configuration.AugmentedGrammar;
            KGrammar.ExtendGrammar();
        }

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
            _productionItems.AddRange(items);
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
            items.ForEach(x => result.AddRange(_productionItems.Where(y => y.Production == x.Production && x.DotPosition + 1 == y.DotPosition)));
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

        private State BuildStates(List<Item> currentItems, List<State> states)
        {
            for (int i = 0; i < states.Count; i++)
            {
                bool exists = true;
                for (int j = 0; exists && j < states[i].CurrentItems.Count; j++)
                {
                    if (!currentItems.Contains(states[i].CurrentItems[j]))
                    {
                        exists = false;
                    }
                }
                if (exists) return states[i];
            }

            State actualState = new State()
            {
                CurrentItems = currentItems
            };
            states.Add(actualState);

            List<Item> closure = new List<Item>();
            currentItems.ForEach(x => closure.AddRange(Closure(x)));

            List<Item> closureAndItems = new List<Item>(closure);
            closureAndItems.AddRange(currentItems);

            Dictionary<string, State> GotoStates = new Dictionary<string, State>();
            var groupsGoto = GroupGoto(closureAndItems);
            foreach (var val in groupsGoto)
            {
                var resultState = BuildStates(val.Value, states);
                if (resultState != null) GotoStates.Add(val.Key, resultState);
            }

            actualState.Closure = closure;
            actualState.Goto = GotoStates;

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

                foreach (var gotoState in actualState.Goto)
                {
                    queue.Enqueue(gotoState.Value);
                }
            }
        }

        #region Tests
        public void PrintAllItems()
        {
            foreach (var val in _productionItems)
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

        public void PrintStates()
        {
            BFS(x =>
            {
                x.PrintState();
            });
        }
        #endregion
    }
}