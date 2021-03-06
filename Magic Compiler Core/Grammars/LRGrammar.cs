﻿using MagicCompiler.Structures.Grammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCompiler.Grammars
{
    /*  la 4-tupla de G=(Vt, Vn, P, S) donde:
        Vt = Conjunto finito de terminales
        Vn = Conjunto fnito de no terminales
        P = Conjunto finito de producciones
        S ∈ Vn = Simbolo inicial
    */
    public class LRGrammar : Grammar
    {
        public CFGConfig Configuration;
        
        public List<Production> _productions;

        public override List<Production> Productions => _productions;
        public override Production AugmentedProduction => Configuration.AugmentedProduction;


        public LRGrammar()
        {
            _productions = new List<Production>();
            LRReader reader = new LRReader();
            reader.Build(this);
            ExtendGrammar();
        }

        public void ExtendGrammar()
        {
            if (!Productions.Contains(Configuration.AugmentedProduction))
                Productions.Add(Configuration.AugmentedProduction);
        }

        protected override void CategorizeSymbols()
        {
            _nonTerminals = new List<string>();

            HashSet<string> nonTerminalsHashSet = new HashSet<string>();
            HashSet<string> symbols = new HashSet<string>();

            Productions.ForEach(production =>
            {
                symbols.Add(production.Left);
                nonTerminalsHashSet.Add(production.Left);

                production.Right.ForEach(rightSymbol => symbols.Add(rightSymbol));
            });

            _nonTerminals = nonTerminalsHashSet.ToList();
            _terminals = symbols.ToList().Except(_nonTerminals).ToList();
        }

        public override bool IsTerminal(string symbol) => Terminals.Contains(symbol);

        public override bool IsNonTerminal(string symbol) => NonTerminals.Contains(symbol);

        protected override void CalculateFirst()
        {
            _first = new Dictionary<string, List<string>>();

            HashSet<Production> analyzedProductions = new HashSet<Production>();
            var terminalStartingProductions = Productions.Where(x => IsTerminal(x.Right[0])).ToList();
            terminalStartingProductions.ForEach(production =>
            {
                FirstOf(production, analyzedProductions);
            });
            
            Productions.Except(terminalStartingProductions).ToList().ForEach(production =>
            {
                FirstOf(production, analyzedProductions);
            });

        }

        // ref: https://www.youtube.com/watch?v=BSTBaPFxs3Q
        private List<string> FirstOf(Production production, HashSet<Production> analyzedProductions)
        {
            if (analyzedProductions.Contains(production))
            {
                if (_first.ContainsKey(production.Left)) return _first[production.Left];
                return new List<string>();
            }
            analyzedProductions.Add(production);

            List<string> result = new List<string>();
            for (int i = 0; i < production.Right.Count; i++)
            {
                if (i == 0 && IsTerminal(production.Right[i]))
                {
                    result.Add(production.Right[i]);
                    break;
                }
                else if (IsNonTerminal(production.Right[i]))
                {
                    var productionsStartinWith = Productions.Where(x => x.Left == production.Right[i]).ToList();
                    bool haveEpsilon = false;
                    productionsStartinWith.ForEach(x =>
                    {
                        var firstOfResult = FirstOf(x, analyzedProductions);
                        result.AddRange(firstOfResult);
                        if (firstOfResult.Contains(Configuration.Epsilon)) haveEpsilon = true;
                    });

                    if (!haveEpsilon) break;
                }
            }

            // Accepted grammar doesn't have epsilon
            if (production == Configuration.AugmentedProduction)
            {
                result.RemoveAll(x => x == Configuration.Epsilon);
            }

            if (_first.ContainsKey(production.Left)) _first[production.Left].AddRange(result);
            else _first.Add(production.Left, result);

            // TODO Improve this with hashsets
            _first[production.Left] = _first[production.Left].Distinct().ToList();

            return _first[production.Left];
        }

        protected override void CalculateFollow()
        {
            _follow = new Dictionary<string, List<string>>();
            
            HashSet<Production> analyzedProductions = new HashSet<Production>();
            Productions.ForEach(x => CalculateFollowOf(x, analyzedProductions));
        }

        // ref:  https://www.youtube.com/watch?v=BSTBaPFxs3Q
        // ref2: https://www.youtube.com/watch?v=_uSlP91jmTM
        private List<string> CalculateFollowOf(Production actualProduction, HashSet<Production> analyzedProductions)
        {
            if (actualProduction.Right[0] == Configuration.Epsilon) return new List<string>();
            if (analyzedProductions.Contains(actualProduction))
            {
                if (_follow.ContainsKey(actualProduction.Left)) return _follow[actualProduction.Left];
                return new List<string>();
            }
            analyzedProductions.Add(actualProduction);
            List<string> result = new List<string>();

            // case 1
            if (actualProduction == Configuration.AugmentedProduction) 
                result.Add(Configuration.AugmentedProduction.Left);

            var productionsWith = Productions.Where(x => x.Right.Contains(actualProduction.Left)).ToList();
            productionsWith.RemoveAll(p => p.Right[0] == Configuration.Epsilon);

            productionsWith.ForEach(production =>
            {
                int occurence = production.Right.FindIndex(x => x == actualProduction.Left);

                // case 3
                if (occurence + 1 >= production.Right.Count)
                {
                    var validProductions = Productions.Where(x => x.Left == production.Left).ToList();
                    validProductions.ForEach(p => result.AddRange(CalculateFollowOf(p, analyzedProductions)));
                }
                else // case 3, 2
                {
                    // case 2
                    if (IsTerminal(production.Right[occurence + 1])) result.Add(production.Right[occurence + 1]);
                    else // case 3
                    {
                        result.AddRange(_first[production.Right[occurence + 1]]);
                        int count = 2;
                        while (result.Contains(Configuration.Epsilon))
                        {
                            result.RemoveAll(p => p == Configuration.Epsilon);
                            if (occurence + count >= production.Right.Count)
                            {
                                var validProductions = Productions.Where(x => x.Left == production.Left).ToList();
                                validProductions.ForEach(p => result.AddRange(CalculateFollowOf(p, analyzedProductions)));
                            }
                            else
                            {
                                if (IsTerminal(production.Right[occurence + count])) result.Add(production.Right[occurence + count]);
                                else result.AddRange(_first[production.Right[occurence + count]]);
                            }
                            count++;
                        }
                    }
                }
            });

            result.RemoveAll(x => x == Configuration.Epsilon);

            if (_follow.ContainsKey(actualProduction.Left)) _follow[actualProduction.Left].AddRange(result);
            else _follow.Add(actualProduction.Left, result);

            // TODO Improve this with hashsets
            _follow[actualProduction.Left] = _follow[actualProduction.Left].Distinct().ToList();

            return result;
        }

        #region Tests
        public void PrintFirst()
        {
            foreach (var val in First)
            {
                Console.WriteLine("First of: " + val.Key);
                val.Value.ForEach(x => Console.Write(x + " "));
                Console.WriteLine();
            }
        }

        public void PrintFollow()
        {
            foreach (var val in Follow)
            {
                Console.WriteLine("Follow of: " + val.Key);
                val.Value.ForEach(x => Console.Write(x + " "));
                Console.WriteLine();
            }
        }
        #endregion
    }
}
