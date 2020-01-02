using MagicCompiler.Scripting;
using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCompiler.MatLab
{
    public class sv_MultiAssignation : ISemanticValidation
    {
        public string[] Productions => new string[] { "asignacion ::= [ seqid ] = llamadafuncion" };

        public bool ValidProduction(Production production) => Productions.Contains(production.ToString());

        public List<SemanticAnswer> Evaluate(List<Token> token)
        {
            List<SemanticAnswer> result = new List<SemanticAnswer>();
            int equalIndex = token.FindLastIndex(token => token.IsSymbol(Context.symbol_equal));
            List<Token> IdTokens = new List<Token>();
            for (int i = equalIndex; !token[i].IsSymbol(Context.symbol_openBracket) && i >= 0; i--)
            {
                if (token[i].IsSymbol(Context.symbol_id))
                    IdTokens.Add(token[i]);
            }
            IdTokens.Reverse();

            var assignationSet = Context.Instance.Assignations;
            IdTokens.ForEach(id =>
            {
                if (assignationSet.Contains(id.Lexeme))
                {
                   result.Add(new SemanticAnswer(AnswerType.Warning, string.Format("\"{0}\": Reassignation", id.Lexeme)));
                }
                else
                {
                    result.Add(new SemanticAnswer(AnswerType.Valid, string.Format("\"{0}\": Assignation", id.Lexeme)));
                }
            });
            return result;
        }
    }
}
