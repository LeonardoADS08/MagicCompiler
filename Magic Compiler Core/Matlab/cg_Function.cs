using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCompiler.MatLab
{
    public class cg_Function : ICodeGenerator
    {
        public string[] Productions => new string[]
        {
            "inifuncion ::= function id ( seqid )",
            "inifuncion ::= function id = id ( seqid )",
            "inifuncion ::= function [ parametros ] = id ( seqid )",
            "funcion ::= inifuncion seqsentencias end"

        };

        public bool ValidProduction(Production production) => Productions.Contains(production.ToString());

        public string Translate(List<Token> tokens, Production production)
        {
            if (production.ToString() == "inifuncion ::= function id ( seqid )")
            {
                int firstSymbolIndex = tokens.FindLastIndex(token => token.IsSymbol(Context.symbol_function)) + 1;
                int i;
                string res = string.Format("function {0} ({1})", tokens[firstSymbolIndex].Lexeme, Context.Instance.Translations.Pop());
                res += " {";
                Context.Instance.Translations.Push(res);
                return res;
            }
            if (production.ToString() == "inifuncion ::= function id = id ( seqid )")
            {
                int firstSymbolIndex = tokens.FindLastIndex(token => token.IsSymbol(Context.symbol_function)) + 1;
                int i= firstSymbolIndex + 1;
                while (tokens[i].IsSymbol(Context.symbol_id))
                {
                    i++;
                }
                i++;
                string res = string.Format("function {0} = {1} ({2})", tokens[firstSymbolIndex].Lexeme,tokens[i].Lexeme, Context.Instance.Translations.Pop());
                res += " {";
                Context.Instance.Translations.Push(res);
                return res;
            }
            if (production.ToString() == "inifuncion ::= function [ parametros ] = id ( seqid )")
            {
                int firstSymbolIndex = tokens.FindLastIndex(token => token.IsSymbol(Context.symbol_id));
               
                string res = string.Format("function [{2}] = {1} ({0})", Context.Instance.Translations.Pop(), tokens[firstSymbolIndex].Lexeme, Context.Instance.Translations.Pop());
                res += " {";
                Context.Instance.Translations.Push(res);
                return res;
            }
            if (production.ToString() == "funcion ::= inifuncion seqsentencias end")
            {
                int n = 0;
                string res = string.Format("{1} {0}", Context.Instance.Translations.Pop(), Context.Instance.Translations.Pop());
                res += "}";
                Context.Instance.Translations.Push(res);
                return res;
            }
            return "xd";
        }


    }
}
