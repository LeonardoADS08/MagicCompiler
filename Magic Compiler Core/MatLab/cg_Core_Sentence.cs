using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCompiler.MatLab
{
    public class cg_Core_Sentence : ICodeGenerator
    {
        public string[] Productions => new string[]
        {
            "sentencia ::= asignacion",
            "sentencia ::= llamadafuncion",
            "sentencia ::= funcion",
            "sentencia ::= condicional",
            "sentencia ::= deffor",
            "sentencia ::= defwhile"
        };

        public bool ValidProduction(Production production) => Productions.Contains(production.ToString());

        public string Translate(List<Token> tokens, Production production)
        {
            string prod = production.ToString();
            string res = string.Empty;
            if (prod == "sentencia ::= asignacion" ||
                prod == "sentencia ::= llamadafuncion" ||
                prod == "sentencia ::= funcion")
            {
                res = string.Format("{0};", Context.Instance.Translations.Pop());
            }
            else res = Context.Instance.Translations.Pop();

            if (Context.Instance.BlocskOpen > 0) Context.Instance.Translations.Push(res);
            else Context.Instance.FinalTranslation.Add(res);
            return res;
        }
    }
}
