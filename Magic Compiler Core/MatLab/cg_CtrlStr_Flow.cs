using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCompiler.MatLab
{
    public class cg_CtrlStr_Flow : ICodeGenerator
    {
        public string[] Productions => new string[]
        {
            "deffor ::= inifor seqsentencias end",
            "inifor ::= for id = termino : termino",
            "defwhile ::= iniwhile seqsentencias end",
            "iniwhile ::= while ( termino )"
        };

        public bool ValidProduction(Production production) => Productions.Contains(production.ToString());

        public string Translate(List<Token> tokens, Production production)
        {
            string prod = production.ToString();
            string res = string.Empty;
            if (prod == "inifor ::= for id = termino : termino")
            {
                var index = tokens.FindLastIndex(token => token.IsSymbol(Context.symbol_for)) + 1;
                var tokenID = tokens[index];
                res = string.Format("for({0} = {2}; {1}; {0}++) ", tokenID.Lexeme, Context.Instance.Translations.Pop(), Context.Instance.Translations.Pop()) + "{";
                Context.Instance.Translations.Push(res);
                Context.Instance.BlocskOpen++;
            }
            else if (prod == "deffor ::= inifor seqsentencias end" || prod == "defwhile ::= iniwhile seqsentencias end")
            {
                while (Context.Instance.Translations.Count != 0)
                {
                    if (Context.Instance.Translations.Count == 1)
                    {
                        res = res.Insert(0, Context.Instance.Translations.Pop() + Environment.NewLine);
                    }
                    else res += Context.Instance.Translations.Pop() + Environment.NewLine;
                }
                res += "}" + Environment.NewLine;
                Context.Instance.BlocskOpen--;
                Context.Instance.Translations.Push(res);
            }
            else if (prod == "iniwhile ::= while ( termino )")
            {
                res = string.Format("while ({0}) ", Context.Instance.Translations.Pop()) + "{";
                Context.Instance.Translations.Push(res);
                Context.Instance.BlocskOpen++;
            }
            return res;
        }
    }
}

