using MagicCompiler.Scripting;
using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCompiler.MatLab
{
    public class sv_Assignation : ISemanticValidation
    {
        public string[] Productions => new string[] 
        { 
            "asignacion ::= id = llamadafuncion", 
            "asignacion ::= id = id", 
            "asignacion ::= id = constante",
            "asignacion ::= id = termino"
        };

        public bool ValidProduction(Production production) => Productions.Contains(production.ToString());

        public List<SemanticAnswer> Evaluate(List<Token> token)
        {
            List<SemanticAnswer> result = new List<SemanticAnswer>();
            int equalIndex = token.FindLastIndex(token => token.IsSymbol(Context.symbol_equal));
            var idToken = token[equalIndex - 1];
            var assignationSet = Context.Instance.Assignations;
            if (assignationSet.Contains(idToken.Lexeme))
            {
                result.Add(new SemanticAnswer(AnswerType.Warning, string.Format("\"{0}\": Reassignation", idToken.Lexeme)));
                return result;
            }
            else
            {
                assignationSet.Add(idToken.Lexeme);
                result.Add(new SemanticAnswer(AnswerType.Valid, string.Format("\"{0}\": Assignation", idToken.Lexeme)));
                return result;
            }
        }
    }
}
