using MagicCompiler.Scripting;
using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCompiler.MatLab
{
    public class sv_MatrixDefinition : ISemanticValidation
    {
        public string[] Productions => new string[] { "matriz ::= [ fila ]" };

        public bool ValidProduction(Production production) => Productions.Contains(production.ToString());

        public List<SemanticAnswer> Evaluate(List<Token> tokens)
        {
            List<SemanticAnswer> result = new List<SemanticAnswer>();
            Stack<Token> tokenStack = new Stack<Token>(tokens);
            List<Token> reductionTokens = new List<Token>();
            bool finished = false;
            while (!finished && tokenStack.Count != 0)
            {
                var peek = tokenStack.Pop();
                if (peek.IsSymbol(Context.symbol_openBracket))
                    finished = true;
                reductionTokens.Add(peek);
            }
            if (!finished)
            {
                result.Add(new SemanticAnswer(AnswerType.Error, "Matrix definition not found (Parsing error?)"));
                return result;
            }
            reductionTokens.Reverse();

            // Check if matrix is correctly defined
            int rows = 0, columns = -1, auxColumns = 0;
            for (int i = 0; i < reductionTokens.Count; i++)
            {
                if (reductionTokens[i].IsSymbol(Context.symbol_constant))
                {
                    auxColumns++;
                }
                else if (reductionTokens[i].IsSymbol(Context.symbol_closeBracket, Context.symbol_semicolon))
                {
                    if (columns == -1) columns = auxColumns;
                    if (columns != auxColumns)
                    {
                        result.Add(new SemanticAnswer(AnswerType.Error, "Matrix column count not matching"));
                        return result;
                    }
                    auxColumns = 0;
                    rows++;
                }
            }
            result.Add(new SemanticAnswer(AnswerType.Valid, "Matrix defined correctly"));
            return result;
        }

    }
}
