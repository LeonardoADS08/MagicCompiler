using MagicCompiler.Automaton;
using MagicCompiler.Grammar;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.Syntactic
{
    public class ParsingTable : List<StateParser>
    {
        public State InitialState;
        public ParsingTable()
        {
            AutomatonBuilder automaton = new AutomatonBuilder();
            automaton.Build();
            automaton.BFS(x =>
            {
                this.Add(new StateParser(x, automaton.KGrammar));
            });
            InitialState = automaton.InitialState;
        }

        public StateParser StateParser(State state) => Find(x => x.State == state);


        #region Tests
        public void PrintTable()
        {
            this.ForEach(x => x.PrintStateParser());
        }
        #endregion
    }
}
