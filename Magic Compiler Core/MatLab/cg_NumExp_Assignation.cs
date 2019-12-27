using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCompiler.MatLab
{
    public class cg_NumExp_Assignation : ICodeGenerator
    {
        public string[] Productions => new string[]
        {
            "asignacion ::= termino"
        };

        public bool ValidProduction(Production production) => Productions.Contains(production.ToString());

        public string Translate(List<Token> tokens)
        {
            int index = tokens.FindLastIndex(Token => Token.IsSymbol(Context.symbol_equal)) - 1;
            string res = string.Format("var {0} = {1};", tokens[index].Lexeme, Context.Instance.Translations.Dequeue());
            Context.Instance.Translations.Enqueue(res);
            return res;
        }

       
    }
}
