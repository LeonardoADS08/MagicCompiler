using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCompiler.MatLab
{
    public class sv_FunctionReturnDefinition : ISemanticValidation
    {
        public string[] Productions => new string[]
        {
            "funcion ::= function [ parametros ] = id ( parametros ) seqsentencias end"
        };

        public bool ValidProduction(Production production) => Productions.Contains(production.ToString());

        public List<SemanticAnswer> Evaluate(List<Token> token)
        {
            List<SemanticAnswer> result = new List<SemanticAnswer>();
            int endCount = 0, index = token.Count - 1;
            do
            {
                if (token[index].IsSymbol(Context.symbol_end))
                    endCount++;
                else if (token[index].IsSymbol(Context.symbol_function, Context.symbol_if, Context.symbol_for, Context.symbol_while))
                    endCount--;
                index--;
            } while (endCount != 0);

            // Find function ID
            bool found = false;
            string functionID = string.Empty;
            for (int i = index; !found && i < token.Count; i++)
            {
                if (token[i].IsSymbol(Context.symbol_equal))
                {
                    functionID = token[i + 1].Lexeme;
                    found = true;
                }
            }

            var functions = Context.Instance.Functions;
            if (functions.Contains(functionID))
            {
                result.Add(new SemanticAnswer(AnswerType.Warning, string.Format("\"{0}\": Function redefinition", functionID)));
            }
            else
            {
                functions.Add(functionID);
                result.Add(new SemanticAnswer(AnswerType.Valid, string.Format("\"{0}\": Function definition", functionID)));
            }
            return result;
        }


    }
}
