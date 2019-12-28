using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCompiler.MatLab
{
    public class cg_CtrlStr_Control : ICodeGenerator
    {
        public string[] Productions => new string[]
        {
            "condicional ::= inicondicional fincondicional", // OK
            "condicional ::= inicondicional seqsentencias fincondicional", // OK
            //"condicional ::= inicondicional seqsentencias condicionalanidado fincondicional", // OK
            "condicionalanidado ::= condicionalanidado inicondicionalanidado seqsentencias",  // OK
            "condicionalanidado ::= inicondicionalanidado seqsentencias", // OK
            "inicondicionalanidado ::= elseif ( termino )", // OK
            "inicondicional ::= if ( termino )", // OK
            "fincondicional ::= inifincondicional seqsentencias end",
            "fincondicional ::= end", // OK
            "inifincondicional ::= else" // OK

        };

        public bool ValidProduction(Production production) => Productions.Contains(production.ToString());

        public string Translate(List<Token> tokens, Production production)
        {
            string prod = production.ToString();
            string res = string.Empty;
            if (prod == "inicondicional ::= if ( termino )")
            {
                res = string.Format("if ({0}) ", Context.Instance.Translations.Pop()) + "{" + Environment.NewLine;
                Context.Instance.Translations.Push(res);
                Context.Instance.BlocskOpen++;
            }
            else if (prod == "inifincondicional ::= else")
            {
                while (Context.Instance.Translations.Count != 0)
                {
                    if (Context.Instance.Translations.Count == 1)
                        res = res.Insert(0, Context.Instance.Translations.Pop());
                    else 
                        res += Context.Instance.Translations.Pop() + Environment.NewLine;
                }
                res += "} " + Environment.NewLine + "else {";
                Context.Instance.Translations.Push(res);
            }
            else if (prod == "fincondicional ::= end")
            {
                res = Context.Instance.Translations.Pop() + "}" + Environment.NewLine;
                Context.Instance.Translations.Push(res);
                Context.Instance.BlocskOpen--;
            }
            else if (prod == "fincondicional ::= inifincondicional seqsentencias end")
            {
                while (Context.Instance.Translations.Count != 0)
                {
                    if (Context.Instance.Translations.Count == 1)
                        res = res.Insert(0, Context.Instance.Translations.Pop() + Environment.NewLine);
                    else
                        res += Context.Instance.Translations.Pop() + Environment.NewLine;
                }
                res += "}" + Environment.NewLine;
                Context.Instance.Translations.Push(res);
                Context.Instance.BlocskOpen--;
            }
            else if (prod == "inifincondicional seqsentencias end")
            {
                while (Context.Instance.Translations.Count != 0)
                {
                    if (Context.Instance.Translations.Count == 1)
                        res = res.Insert(0, Context.Instance.Translations.Pop());
                    else
                        res += Context.Instance.Translations.Pop() + Environment.NewLine;
                }
                res += "}" + Environment.NewLine;
                Context.Instance.Translations.Push(res);
                Context.Instance.BlocskOpen--;
            }
            else if (prod == "condicional ::= inicondicional fincondicional")
            {
                res = string.Format("{1}{0}", Context.Instance.Translations.Pop(), Context.Instance.Translations.Pop());
                Context.Instance.Translations.Push(res);
            }
            else if (prod == "condicional ::= inicondicional seqsentencias fincondicional")
            {
                var last = Context.Instance.Translations.Pop();
                while (Context.Instance.Translations.Count != 0)
                {
                    if (Context.Instance.Translations.Count == 1)
                        res = res.Insert(0, Context.Instance.Translations.Pop());
                    else 
                        res += Context.Instance.Translations.Pop() + Environment.NewLine;
                }
                res += last;
                Context.Instance.Translations.Push(res);
            }
            else if (prod == "inicondicionalanidado ::= elseif ( termino )")
            {
                var last = Context.Instance.Translations.Pop();
                while (Context.Instance.Translations.Count != 0)
                {
                    if (Context.Instance.Translations.Count == 1)
                        res = res.Insert(0, Context.Instance.Translations.Pop());
                    else 
                        res += Context.Instance.Translations.Pop() + Environment.NewLine;
                }
                res += "} " + Environment.NewLine + "else if (" + last + ") {";
                Context.Instance.Translations.Push(res);
            }
            else if (prod == "condicionalanidado ::= inicondicionalanidado seqsentencias" ||
                     prod == "condicionalanidado ::= condicionalanidado inicondicionalanidado seqsentencias")
            {
                while (Context.Instance.Translations.Count != 0)
                {
                    if (Context.Instance.Translations.Count == 1)
                        res = res.Insert(0, Context.Instance.Translations.Pop() + Environment.NewLine);
                    else 
                        res += Context.Instance.Translations.Pop() + Environment.NewLine;
                }
                Context.Instance.Translations.Push(res);
            }
            else if (prod == "condicional ::= inicondicional seqsentencias condicionalanidado fincondicional")
            {
                while (Context.Instance.Translations.Count != 0)
                {
                    if (Context.Instance.Translations.Count == 1)
                        res = res.Insert(0, Context.Instance.Translations.Pop() + Environment.NewLine);
                    else 
                        res += Context.Instance.Translations.Pop() + Environment.NewLine;
                }
                res += "}";
                Context.Instance.Translations.Push(res);
            }
            return res;
        }
    }
}

