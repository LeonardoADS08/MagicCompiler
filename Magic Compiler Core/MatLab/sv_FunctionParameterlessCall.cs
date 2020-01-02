using MagicCompiler.Scripting;
using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCompiler.MatLab
{
    public class sv_FunctionParameterlessCall : ISemanticValidation
    {
        public string[] Productions => new string[] 
        {
            "llamadafuncion ::= id",
            "llamadafuncion ::= id ( )"
        };

        public bool ValidProduction(Production production) => Productions.Contains(production.ToString());

        public List<SemanticAnswer> Evaluate(List<Token> token)
        {
            List<SemanticAnswer> result = new List<SemanticAnswer>();
            var tokenID = token.FindLast(x => x.IsSymbol(Context.symbol_id));

            var functions = Context.Instance.Functions;
            if (functions.Contains(tokenID.Lexeme))
            {
                result.Add(new SemanticAnswer(AnswerType.Valid, string.Format("Function call \"{0}\" OK", tokenID.Lexeme)));
                return result;
            }
            else
            {
                result.Add(new SemanticAnswer(AnswerType.Warning, string.Format("Function call \"{0}\" recursive, extern or not defined", tokenID.Lexeme)));
                return result;
            }
        }
    }
}
