using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCompiler.MatLab
{
    public class cg_FuncDef_Parameters : ICodeGenerator
    {
        public string[] Productions => new string[]
        {
            "parametros ::= parametros , termino",
            "parametros ::= iniparametros termino",
            "parametros ::= termino",
            "iniparametro ::= termino ,"
        };

        public bool ValidProduction(Production production) => Productions.Contains(production.ToString());

        public string Translate(List<Token> tokens, Production production)
        {
            string prod = production.ToString();
            string res = string.Empty;
            if (prod == "iniparametro ::= termino ,")
            {
                res = string.Format("{0},", Context.Instance.Translations.Pop());
                Context.Instance.Translations.Push(res);
            }
            else if (prod == "parametros ::= termino")
            {
                res = string.Format("{0} ", Context.Instance.Translations.Pop());
                Context.Instance.Translations.Push(res);
            }
            else if (prod == "parametros::= iniparametros termino")
            {
                res = string.Format("{1}{0}", Context.Instance.Translations.Pop(), Context.Instance.Translations.Pop());
                Context.Instance.Translations.Push(res);
            }
            else if (prod == "parametros ::= parametros , termino")
            {
                res = string.Format("{1}, {0}", Context.Instance.Translations.Pop(), Context.Instance.Translations.Pop());
                Context.Instance.Translations.Push(res);
            }
            return res;
        }
    }
}
