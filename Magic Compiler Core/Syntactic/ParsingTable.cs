using MagicCompiler.Automaton;
using MagicCompiler.Grammar;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.Syntactic
{
    public class ParsingTable
    {
        public List<StateParser> Table = new List<StateParser>();

        public ParsingTable()
        {
            AutomatonBuilder automaton = new AutomatonBuilder();
            automaton.Build();
            automaton.BFS(x =>
            {
                Table.Add(new StateParser(x, automaton.KGrammar));
            });
            automaton.KGrammar.PrintFirst();
            automaton.KGrammar.PrintFollow();
        }

        #region Tests
        public void PrintTable()
        {
            Table.ForEach(x => x.PrintStateParser());
        }
        #endregion
    }
}
