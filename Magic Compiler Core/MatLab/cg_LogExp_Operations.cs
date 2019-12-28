using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCompiler.MatLab
{
    public class cg_LogExp_Operations : ICodeGenerator
    {
        public string[] Productions => new string[]
        {
            "explogica ::= ( explogica )",
            "explogica ::= constante",
            "explogica ::= iniexplogica termino",
            "iniexplogica ::= termino oplog",
            "iniexplogica ::= explogica oplog"
        };

        public bool ValidProduction(Production production) => Productions.Contains(production.ToString());

        public string Translate(List<Token> tokens, Production production)
        {
            string res = string.Empty, prod = production.ToString();
            if (prod == "explogica ::= ( explogica )")
            {
                res = string.Format("({0})", Context.Instance.Translations.Pop());
                Context.Instance.Translations.Push(res);
            }
            else if (prod == "explogica ::= iniexplogica termino")
            {
                res = string.Format("{1} {0})", Context.Instance.Translations.Pop(), Context.Instance.Translations.Pop());
                Context.Instance.Translations.Push(res);
            }
            else if (prod == "iniexplogica ::= termino oplog" || prod == "iniexplogica ::= explogica oplog")
            {
                res = OperatorToTranslation(tokens.Last().Lexeme, Context.Instance.Translations.Pop());
                Context.Instance.Translations.Push(res);
            }
            return res;
        }

        private string OperatorToTranslation(string op, string term1)
        {
            string translatedOperator = string.Empty;
            switch (op)
            {
                case @"<":
                    translatedOperator = "lessThan(";
                    break;
                case @">":
                    translatedOperator = "greaterThan(";
                    break;
                case @">=":
                    translatedOperator = "greaterOrEqual(";
                    break;
                case @"<=":
                    translatedOperator = "lessOrEqual(";
                    break;
                case @"==":
                    translatedOperator = "equals(";
                    break;
                case @"~=":
                    translatedOperator = "notEquals(";
                    break;
                default:
                    translatedOperator = "lessThan(";
                    break;
            }
            return string.Format("{0}{1},", translatedOperator, term1);

        }
    }
}
