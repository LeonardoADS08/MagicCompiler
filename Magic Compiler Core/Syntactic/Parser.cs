using MagicCompiler.Automaton;
using MagicCompiler.Grammar;
using MagicCompiler.Lexical;
using MagicCompiler.Properties;
using MagicCompiler.Semantic;
using MagicCompiler.Semantic.Interfaces;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCompiler.Syntactic
{
    public class Parser
    {
        private ParsingTable _parsingTable;
        private Lexer _lexer;

        private ISemanticAnalyzer _semanticAnalyzer;

        public bool DEBUG => bool.Parse(Resources.Debug);

        public Parser()
        {
            _parsingTable = new ParsingTable();
            _lexer = new Lexer();
            _lexer.Analyze();

            var semanticScriptLoader = new SemanticScriptLoader();
            _semanticAnalyzer = semanticScriptLoader.GetSemanticAnalyzer();

            if (DEBUG) _parsingTable.PrintTable();
        }

        private void DebugStack(Stack<State> stateStack)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Stack : ");
            var stack = stateStack.ToList();
            stack.Reverse();
            stack.ForEach(x => Console.Write(" " + x.Order));
            Console.WriteLine();
            Console.ResetColor();
        }

        private void DebugToken(Token token)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Token : ");
            token.PrintTokenForParser();
            Console.ResetColor();
        }

        private void DebugAction(Action action, State state)
        {
            switch (action.Type)
            {
                case ActionType.Shift:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Shift : " + action.Shift.Order);
                    Console.ResetColor();
                    break;
                case ActionType.Reduce:
                    if (state.Goto.ContainsKey(action.Reduce.Left))
                    {
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
                    }
                    break;
                case ActionType.Accept:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Accepted by MagicCompiler");
                    Console.ResetColor();
                    break;
                case ActionType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error!");
                    Console.ResetColor();
                    break;
            }
        }


        public void Check()
        {
            bool finish = false;
            Token token = _lexer.Next();
            List<Token> usedTokens = new List<Token>() { token };
            Stack<State> stateStack = new Stack<State>();
            stateStack.Push(_parsingTable.InitialState);

            while (!finish)
            {
                var state = _parsingTable.StateParser(stateStack.Peek());

                if (DEBUG) DebugStack(stateStack);

                // Action
                if (state.Action.ContainsKey(token.Symbol.TSymbol))
                {
                    if (DEBUG) DebugToken(token);

                    var action = state.Action[token.Symbol.TSymbol];
                    switch (action.Type)
                    {
                        case ActionType.Shift:
                            if (DEBUG) DebugAction(action, stateStack.Peek());
                            stateStack.Push(action.Shift);
                            token = _lexer.Next();
                            usedTokens.Add(token);
                            break;
                        case ActionType.Reduce:
                            for (int i = 0; i < action.Reduce.Right.Count; i++)
                                stateStack.Pop();
                            state = _parsingTable.StateParser(stateStack.Peek());

                            if (DEBUG) DebugAction(action, stateStack.Peek());

                            if (state.Goto.ContainsKey(action.Reduce.Left))
                            {
                                stateStack.Push(state.Goto[action.Reduce.Left]);
                                if (_semanticAnalyzer.RequiresEvaluation(action.Reduce))
                                {
                                    finish = !_semanticAnalyzer.Evaluate(usedTokens.ToArray(), action.Reduce); // SEMANTIC FAIL == FALSE || 
                                    usedTokens.Clear();
                                }
                            }
                            else  finish = true;
                            break;
                        case ActionType.Accept:
                            if (DEBUG) DebugAction(action, stateStack.Peek());
                            finish = true;
                            break;
                        case ActionType.Error:
                            if (DEBUG) DebugAction(action, stateStack.Peek());
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
            }
        }

        
    }
}
