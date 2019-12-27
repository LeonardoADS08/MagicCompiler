using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCompiler.MatLab
{
    public class cg_FuncDef_Llamada : ICodeGenerator
    {
        public string[] Productions => new string[]
        {
            "parametros ::= parametros , parametros",
            "parametros ::= termino"
        };

        public bool ValidProduction(Production production) => Productions.Contains(production.ToString());

        public string Translate(List<Token> tokens, Production production)
        {
            string prod = production.ToString();
            string res = string.Empty;
            if (prod == "llamadafuncion ::= id")
            {
                res = string.Format("{0}();", tokens[tokens.Count - 1].Lexeme);
                Context.Instance.Translations.Enqueue(res);
            }
            else if (prod == "llamadafuncion ::= id ( )")
            {
                var token = tokens.FindLast(token => token.IsSymbol(Context.symbol_id));
                res = string.Format("{0}();", token.Lexeme);
            }
            else if (prod == "llamadafuncion ::= id ( parametros )")
            {
                var index = tokens.FindLastIndex(token => token.IsSymbol(Context.symbol_openParenthesis)) - 1;
                var token = tokens[index];
                res = string.Format("{0}({1});", token.Lexeme, Context.Instance.Translations.Dequeue());
            }
            return res;
        }
    }
}
