using MagicCompiler.Scripting;
using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCompiler.MatLab
{
    public class sv_IDCall : ISemanticValidation
    {
        public string[] Productions => new string[]
        {
            "asignacion ::= id = id",
            "parametros ::= id",
            "seqelementos ::= id",
            "termino ::= id"
        };

        public bool ValidProduction(Production production) => Productions.Contains(production.ToString());
        
        public List<SemanticAnswer> Evaluate(List<Token> token)
        {
            List<SemanticAnswer> result = new List<SemanticAnswer>(); 
            var tokenID = token.FindLast(x => x.IsSymbol(Context.symbol_id));
            var assignations = Context.Instance.Assignations;
            if (assignations.Contains(tokenID.Lexeme))
            {
                result.Add(new SemanticAnswer(AnswerType.Valid, string.Format("\"{0}\" is a valid id.", tokenID.Lexeme)));
                return result;
            }
            else
            {
                result.Add(new SemanticAnswer(AnswerType.Error, string.Format("\"{0}\" id is not defined, maybe is extern", tokenID.Lexeme)));
                return result;
            }
        }
    }
}
