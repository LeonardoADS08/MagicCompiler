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

        public SemanticAnswer Evaluate(List<Token> tokens)
        {
            Stack<Token> tokenStack = new Stack<Token>(tokens);
            List<Token> reductionTokens = new List<Token>();
            bool finished = false;
            while (!finished && tokenStack.Count != 0)
            {
                var peek = tokenStack.Pop();
                if (peek.IsSymbol(ss_Context.symbol_openBracket))
                    finished = true;
                reductionTokens.Add(peek);
            }
            if (!finished) return new SemanticAnswer(false, "Matrix definition not found (Parsing error?)");
            reductionTokens.Reverse();

            // Check if matrix is correctly defined
            int rows = 0, columns = -1, auxColumns = 0;
            for (int i = 0; i < reductionTokens.Count; i++)
            {
                if (reductionTokens[i].IsSymbol(ss_Context.symbol_constant))
                {
                    auxColumns++;
                }
                else if (reductionTokens[i].IsSymbol(ss_Context.symbol_closeBracket, ss_Context.symbol_semicolon))
                {
                    if (columns == -1) columns = auxColumns;
                    if (columns != auxColumns) return new SemanticAnswer(false, "Matrix column count not matching");
                    auxColumns = 0;
                    rows++;
                }
            }

            return new SemanticAnswer(true, "OK");
        }

    }
}
