using MagicCompiler.Automaton;
using MagicCompiler.Grammars;
using MagicCompiler.Lexical;
using MagicCompiler.Properties;
using MagicCompiler.Scripting;
using MagicCompiler.Semantic;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCompiler.Syntactic
{
    public class LRParser : Parser
    {
        private ParsingTable _parsingTable;
        private ISemanticAnalyzer _semanticAnalyzer;
        private ITranslator _translator;

        public bool DEBUG => bool.Parse(Resources.Debug);
        public bool DEBUG_TABLE => DEBUG && bool.Parse(Resources.Debug_Table);
        public bool DEBUG_PARSER => DEBUG && bool.Parse(Resources.Debug_Parser);
        public bool DEBUG_PARSER_ACTION => DEBUG_PARSER && bool.Parse(Resources.Debug_Parser_Action);
        public bool DEBUG_PARSER_TOKEN => DEBUG_PARSER && bool.Parse(Resources.Debug_Parser_Token);
        public bool DEBUG_PARSER_STACK => DEBUG_PARSER && bool.Parse(Resources.Debug_Parser_Stack);
        public bool DEBUG_SEMANTIC => DEBUG && bool.Parse(Resources.Debug_Semantic);
        public bool SEMANTIC_STOPS_PARSER => DEBUG && bool.Parse(Resources.SemanticErrorStopsParser);


        private bool _error = false;
        public LRParser(ILexer lexer, IGrammar grammar) : base(lexer, grammar)
        {
            //var semanticScriptLoader = new SemanticScriptLoader();
            //if (semanticScriptLoader.Assembly == null)
            if (false)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Errors on scripts, can't continue...");
                Console.ResetColor();
                _error = true;
            }
            else
            {
                //_semanticAnalyzer = semanticScriptLoader.GetSemanticAnalyzer();
                var instance = new MatLab.MatLabJS();
                _semanticAnalyzer = instance;
                _translator = instance;

                _parsingTable = new ParsingTable(grammar);
            }

            if (DEBUG_TABLE)
            {
                //_parsingTable.PrintTable();
                _parsingTable.SaveTable();
            }
        }

        public override bool Check()
        {
            if (_error) return false;

            bool finish = false;
            Token token = _lexer.Next();
            List<Token> usedTokens = new List<Token>() { token };
            Stack<State> stateStack = new Stack<State>();
            stateStack.Push(_parsingTable.InitialState);

            while (!finish)
            {
                var state = _parsingTable.StateParser(stateStack.Peek());

                if (DEBUG_PARSER_STACK) DebugStack(stateStack);

                // Action
                if (state.Action.ContainsKey(token.Symbol.TSymbol))
                {
                    if (DEBUG_PARSER_TOKEN) DebugToken(token);

                    var action = state.Action[token.Symbol.TSymbol];
                    switch (action.Type)
                    {
                        case ActionType.Shift:
                            if (DEBUG_PARSER_ACTION) DebugAction(action, stateStack.Peek());
                            stateStack.Push(action.Shift);
                            token = _lexer.Next();
                            usedTokens.Add(token);
                            break;
                        case ActionType.Reduce:
                            for (int i = 0; i < action.Reduce.Right.Count; i++)
                                stateStack.Pop();
                            state = _parsingTable.StateParser(stateStack.Peek());

                            if (DEBUG_PARSER_ACTION) DebugAction(action, stateStack.Peek());

                            if (state.Goto.ContainsKey(action.Reduce.Left))
                            {
                                stateStack.Push(state.Goto[action.Reduce.Left]);
                                if (_semanticAnalyzer.RequiresEvaluation(action.Reduce))
                                {
                                    var tokens = new List<Token>(usedTokens);
                                    tokens.RemoveAt(tokens.Count - 1);

                                    var semanticResult = _semanticAnalyzer.Evaluate(tokens, action.Reduce); // SEMANTIC FAIL == FALSE || 

                                    if (DEBUG_SEMANTIC)
                                    {
                                        Console.WriteLine();
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.WriteLine("Semantic Analysis result:");
                                    }
                                    
                                    semanticResult.ForEach(result =>
                                    {
                                        switch(result.AnswerType)
                                        {
                                            case AnswerType.Error:
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                if (SEMANTIC_STOPS_PARSER) finish = true;
                                                break;
                                            case AnswerType.Warning:
                                                Console.ForegroundColor = ConsoleColor.Yellow;
                                                break;
                                            case AnswerType.Valid:
                                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                                break;
                                        }
                                        if (DEBUG_SEMANTIC) Console.WriteLine(result.Message);

                                    });
                                    if (DEBUG_SEMANTIC)
                                    {
                                        Console.ResetColor();
                                        Console.WriteLine();
                                    }
                                }

                                if (_translator.RequiresTranslation(action.Reduce))
                                {
                                    var tokens = new List<Token>(usedTokens);
                                    tokens.RemoveAt(tokens.Count - 1);
                                    _translator.Translate(tokens, action.Reduce);
                                }
                            }
                            else finish = true;
                            break;
                        case ActionType.Accept:
                            if (DEBUG_PARSER_ACTION) DebugAction(action, stateStack.Peek());
                            finish = true;
                            break;
                        case ActionType.Error:
                            if (DEBUG_PARSER_ACTION) DebugAction(action, stateStack.Peek());
                            finish = true;
                            break;
                    }
                    if (DEBUG_PARSER) Console.WriteLine();
                }
                else // Unknown symbol, error
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Unknonwn symbol '{0}', error!", token.Symbol.TSymbol);
                    Console.ResetColor();
                    finish = true;
                }
            }
            return finish;
        }

        #region Tests
        private void DebugStack(Stack<State> stateStack)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Stack : ");
            var stack = stateStack.ToList();
            stack.Reverse();
            stack.ForEach(x => Console.Write(" " + x.Order));
            Console.WriteLine();
            Console.ResetColor();
        }

        private void DebugToken(Token token)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
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
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
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
        #endregion
    }
}
