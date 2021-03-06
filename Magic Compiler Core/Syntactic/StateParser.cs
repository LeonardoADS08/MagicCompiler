﻿using MagicCompiler.Automaton;
using MagicCompiler.Grammars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCompiler.Syntactic
{
    internal class StateParser
    {
        public State State;
        public Dictionary<string, Action> Action = new Dictionary<string, Action>();
        public Dictionary<string, State> Goto = new Dictionary<string, State>();

        public StateParser(State state, IGrammar grammar)
        {
            State = state;

            // Shift or Goto
            foreach (var gotoState in State.Goto)
            {
                if (grammar.IsNonTerminal(gotoState.Key))
                {
                    Goto.Add(gotoState.Key, gotoState.Value);
                }
                else if (grammar.IsTerminal(gotoState.Key))
                {
                    Action.Add(gotoState.Key, new Action()
                    {
                        Type = ActionType.Shift,
                        Shift = gotoState.Value,
                        Reduce = null
                    });
                }
                else Console.WriteLine("Something is going wrong with: " + gotoState.Key);
            }

            state.CurrentItems.ForEach(item =>
            {
                // ACCEPT!
                if (item.Production == grammar.AugmentedProduction && item.DotPosition >= item.Production.Right.Count)
                {
                    Action.Add(item.Production.Left, new Action()
                    {
                        Type = ActionType.Accept,
                        Shift = null,
                        Reduce = null
                    });
                }

                // REDUCE!
                if (item.DotPosition >= item.Production.Right.Count)
                {
                    var followSymbols = grammar.Follow[item.Production.Left];
                    followSymbols.ForEach(symbol =>
                    {
                        if (!Action.ContainsKey(symbol))
                        {
                            Action.Add(symbol, new Syntactic.Action()
                            { 
                                Type = ActionType.Reduce,
                                Shift = null,
                                Reduce = item.Production
                            });
                        }
                    });
                }
            });

            var terminalsAndAccepted = new List<string>(grammar.Terminals);
            terminalsAndAccepted.Add(grammar.AugmentedProduction.Left);
            terminalsAndAccepted.ForEach(symbol =>
            {
                if (!Action.ContainsKey(symbol))
                {
                    Action.Add(symbol, new Syntactic.Action()
                    {
                        Type = ActionType.Error,
                        Shift = null,
                        Reduce = null
                    });
                }
            });
        }

        #region Tests
        public void PrintStateParser()
        {
            Console.WriteLine("State: " + State.Order);
            Console.WriteLine("Action: ");
            foreach (var action in Action)
            {
                Console.ResetColor();
                Console.Write(action.Key + " ->");
                switch (action.Value.Type)
                {
                    case ActionType.Shift:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(" Shift to state: " + action.Value.Shift.Order);
                        break;
                    case ActionType.Reduce:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(" Reduce to production: ");
                        action.Value.Reduce.PrintRule(false);
                        break;
                    case ActionType.Accept:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(" Accept");
                        break;
                    case ActionType.Error:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(" Error");
                        break;
                    default:
                        break;
                }
                Console.ResetColor();
                Console.WriteLine();
            }
            Console.WriteLine("Goto: ");
            foreach (var gotos in Goto)
            {
                Console.WriteLine(gotos.Key + " -> " + gotos.Value.Order);
            }
            Console.WriteLine();
        }

        public string StateToString()
        {
            string res = "";
            res += "State: " + State.Order + ":" + Environment.NewLine;
            foreach (var item in State.CurrentItems)
            {
                res += item.ToString() + Environment.NewLine;
            }
            res += "Action:" + Environment.NewLine;
            foreach (var action in Action)
            {
                res += action.Key + " ->";
                switch (action.Value.Type)
                {
                    case ActionType.Shift:
                        res += " Shift to state: " + action.Value.Shift.Order;
                        break;
                    case ActionType.Reduce:
                        res += " Reduce to production: " + action.Value.Reduce.ToString();
                        break;
                    case ActionType.Accept:
                        res += " Accept";
                        break;
                    case ActionType.Error:
                        res +=  " Error";
                        break;
                    default:
                        break;
                }
                res += Environment.NewLine;
            }
            res += "Goto: ";
            foreach (var gotos in Goto)
            {
                res += gotos.Key + " -> " + gotos.Value.Order + Environment.NewLine;
            }
            res += Environment.NewLine;
            return res;
        }

        #endregion
    }
}
