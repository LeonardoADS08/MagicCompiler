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
        public string[] Productions => new string[] { "matriz ::= [ seqelementos ; fila ]" };

        public bool ValidProduction(Production production) => Productions.Contains(production.ToString());

        public bool Evaluate(List<Token> tokens, ss_Context context)
        {
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

            if (isAssignation)
            {
                double[,] matrixValues = new double[rows, columns];
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < columns; j++)
                        matrixValues[i, j] = values[i * rows + j];

                context.Matrixes.Add(id, new sas_Variable<sas_Matrix<double>>()
                {
                    Name = id,
                    Value = new sas_Matrix<double>(rows, columns, matrixValues)
                });
            }

            return true;
        }

    }
}
