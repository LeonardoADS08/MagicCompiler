using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCompiler.Matlab
{
    public class sv_MatrixDefinition : ISemanticValidation
    {
        public string[] Productions => new string[] { "matriz ::= [ seqelementos ; fila ]", "Matriz ::= [ SeqElementos ]" };

        public bool ValidProduction(Production production) => Productions.Contains(production.ToString());

        public bool Evaluate(List<Token> tokens)
        {
            Stack<Token> tokenStack = new Stack<Token>(tokens);
            List<Token> reductionTokens = new List<Token>();
            bool finished = false;
            while (!finished && tokenStack.Count != 0)
            {
                var peek = tokenStack.Pop();
                // Matrix completed, check if is assignation
                if (peek.IsSymbol(ss_Context.symbol_openBracket))
                {
                    reductionTokens.Add(peek);
                    
                    // is assignation?
                    if (tokenStack.Peek().IsSymbol(ss_Context.symbol_equal))
                    {
                        reductionTokens.Add(tokenStack.Peek());
                        tokenStack.Pop();
                        reductionTokens.Add(tokenStack.Peek());
                        
                        
                    }

                    finished = true;
                    break;
                }
                reductionTokens.Add(peek);
            }

            // Check if it's an assignation
            string id = tokens.Find(t => t.IsSymbol(ss_Context.symbol_id)).Lexeme;
            bool isAssignation = id != null;
            List<double> values = new List<double>();
            double auxValue;

            // Check if matrix is correctly defined
            int rows = 0, columns = -1, auxColumns = 0;
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].IsSymbol(ss_Context.symbol_constant))
                {
                    auxColumns++;
                    if (isAssignation && Double.TryParse(tokens[i].Lexeme, out auxValue)) values.Add(auxValue);
                    else return false;
                }
                else if (tokens[i].IsSymbol(ss_Context.symbol_closeBracket, ss_Context.symbol_semicolon))
                {
                    if (columns == -1) columns = auxColumns;
                    if (columns != auxColumns) return false;
                    auxColumns = 0;
                    rows++;
                }
            }

            return true;
        }

    }
}
