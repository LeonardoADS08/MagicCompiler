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
            "llamadafuncion ::= id ( )",
            "inillamadafunc ::= id (",
            "llamadafuncion ::= inillamadafunc parametros )"
        };

        public bool ValidProduction(Production production) => Productions.Contains(production.ToString());

        public string Translate(List<Token> tokens, Production production)
        {
            string prod = production.ToString();
            string res = string.Empty;
            if (prod == "llamadafuncion ::= id ( )")
            {
                var token = tokens.FindLast(token => token.IsSymbol(Context.symbol_id));
                res = string.Format("{0}();", token.Lexeme);
                Context.Instance.Translations.Push(res);
            }
            else if (prod == "inillamadafunc ::= id (")
            {
                var index = tokens.FindLastIndex(token => token.IsSymbol(Context.symbol_openParenthesis)) - 1;
                var token = tokens[index];
                res = string.Format("{0}(", token.Lexeme);
                Context.Instance.Translations.Push(res);
            }
            else if (prod == "llamadafuncion ::= inillamadafunc parametros )")
            {
                res = string.Format("{1}{0});", Context.Instance.Translations.Pop(), Context.Instance.Translations.Pop());
                Context.Instance.Translations.Push(res);
            }
            return res;
        }
    }
}
