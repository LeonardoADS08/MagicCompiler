using MagicCompiler.Automaton;
using MagicCompiler.Grammar;
using MagicCompiler.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCompiler.Syntactic
{
    public enum ParserState
    {
        Working, Error, OK
    }

    public class Parser
    {
        public ParserState State;

        private ParsingTable _parsingTable;
        private Lexer _lexer;

        private Stack<State> _stateStack;

        public Parser(Lexer lexer)
        {
            State = ParserState.Working;

            _parsingTable = new ParsingTable();
            _stateStack = new Stack<State>();
            _parsingTable.PrintTable();
            _lexer = lexer;
        }

        public void Check()
        {
            Token token = _lexer.Next();
            _stateStack.Push(_parsingTable.InitialState);
            bool finish = false;
            while (!finish)
            {
                var state = _parsingTable.StateParser(_stateStack.Peek());

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("Stack : ");
                var stack = _stateStack.ToList();
                stack.Reverse();
                stack.ForEach(x => Console.Write(" " + x.Order));
                Console.WriteLine();
                Console.ResetColor();

                // Action
                if (state.Action.ContainsKey(token.Symbol.TSymbol))
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Token : ");
                    token.PrintTokenForParser();
                    Console.ResetColor();

                    var action = state.Action[token.Symbol.TSymbol];
                    switch (action.Type)
                    {
                        case ActionType.Shift:
                            _stateStack.Push(action.Shift);
                            token = _lexer.Next();
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("Shift : " + action.Shift.Order);
                            Console.ResetColor();
                            break;
                        case ActionType.Reduce:
                            for (int i = 0; i < action.Reduce.Right.Count; i++)
                                _stateStack.Pop();
                            state = _parsingTable.StateParser(_stateStack.Peek());

                            if (state.Goto.ContainsKey(action.Reduce.Left))
                            {
                                _stateStack.Push(state.Goto[action.Reduce.Left]);
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write("Reduce: ");
                                action.Reduce.PrintRule(false);
                                Console.WriteLine();
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Error on reduce!");
                                Console.ResetColor();
                                finish = true;
                            }
                            
                            break;
                        case ActionType.Accept:
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Accepted by MagicCompiler");
                            Console.ResetColor();
                            finish = true;
                            break;
                        case ActionType.Error:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Error!");
                            Console.ResetColor();
                            finish = true;
                            break;
                    }
                }
                else // Unknown symbol, error
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Unknonwn symbol '{0}', error!", token.Symbol.TSymbol);
                    Console.ResetColor();
                    finish = true;
                }
                Console.WriteLine();
            }
        }

        private Token ProductionToToken(Rule production) => new Token(production.Left, new Symbol("Reduction", production.RuleToString(), production.Left));

    }
}
