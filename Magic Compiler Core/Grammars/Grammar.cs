using MagicCompiler.Structures.Grammar;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.Grammars
{
    public abstract class Grammar : IGrammar
    {
        public abstract List<Production> Productions { get; }
        public abstract Production AugmentedGrammar { get; }

        protected List<string> _terminals;
        protected List<string> _nonTerminals;

        protected Dictionary<string, List<string>> _first;
        protected Dictionary<string, List<string>> _follow;

        public Grammar() { }

        public List<string> Terminals
        {
            get
            {
                if (_terminals == null) CategorizeSymbols();
                return _terminals;
            }
        }
        public List<string> NonTerminals
        {
            get
            {
                if (_nonTerminals == null) CategorizeSymbols();
                return _nonTerminals;
            }
        }
        public Dictionary<string, List<string>> First
        {
            get
            {
                if (_first == null) CalculateFirst();
                return _first;
            }
        }
        public Dictionary<string, List<string>> Follow
        {
            get
            {
                if (_first == null) CalculateFirst();
                if (_follow == null) CalculateFollow();
                return _follow;
            }
        }


        protected abstract void CategorizeSymbols();
        protected abstract void CalculateFirst();
        protected abstract void CalculateFollow();
        public abstract bool IsTerminal(string symbol);
        public abstract bool IsNonTerminal(string symbol);
    }
}
