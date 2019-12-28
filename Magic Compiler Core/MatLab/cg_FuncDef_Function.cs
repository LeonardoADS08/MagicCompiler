using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCompiler.MatLab
{
    public class cg_FuncDef_Function : ICodeGenerator
    {
        public static List<Tuple<int, string>> ReturnBlocks = new List<Tuple<int, string>>();
        public static int FuncCount = 0;

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
            string res = string.Empty, temp = string.Empty;
            if (production.ToString() == "inifuncion ::= function id ( seqid )")
            {
                int firstSymbolIndex = tokens.FindLastIndex(token => token.IsSymbol(Context.symbol_function)) + 1;
                res = string.Format("function {0} ({1})", tokens[firstSymbolIndex].Lexeme, Context.Instance.Translations.Pop());
                res += " {";
                if (Context.Instance.BlocskOpen > 0)
                {
                    temp = Context.Instance.Translations.Pop() + Environment.NewLine;
                    while (Context.Instance.BlockTranslation.Count != 0)
                    {
                        temp += Context.Instance.BlockTranslation.Dequeue() + Environment.NewLine;
                    }
                    Context.Instance.Translations.Push(temp);
                }
                Context.Instance.Translations.Push(temp +res);
                Context.Instance.BlocskOpen++;
                FuncCount++;
            }
            if (production.ToString() == "inifuncion ::= function id = id ( seqid )")
            {
                int index = tokens.FindLastIndex(token => token.IsSymbol(Context.symbol_equal));
                var firstToken = tokens[index - 1];
                var secondToken = tokens[index +1];
                res = string.Format("function {0} = {1} ({2})", firstToken.Lexeme, secondToken.Lexeme, Context.Instance.Translations.Pop());
                res += " {";
                if (Context.Instance.BlocskOpen > 0)
                {
                    temp = Context.Instance.Translations.Pop() + Environment.NewLine;
                    while (Context.Instance.BlockTranslation.Count != 0)
                    {
                        temp += Context.Instance.BlockTranslation.Dequeue() + Environment.NewLine;
                    }
                    Context.Instance.Translations.Push(temp);
                }
                Context.Instance.Translations.Push(temp + res);
                Context.Instance.BlocskOpen++;
                FuncCount++;
                ReturnBlocks.Add(new Tuple<int, string>(FuncCount, string.Format("return {0};", firstToken.Lexeme)));
            }
            if (production.ToString() == "inifuncion ::= function [ parametros ] = id ( seqid )")
            {
                int firstSymbolIndex = tokens.FindLastIndex(token => token.IsSymbol(Context.symbol_id));
                res = string.Format("function [{2}] = {1} ({0})", Context.Instance.Translations.Pop(), tokens[firstSymbolIndex].Lexeme, Context.Instance.Translations.Pop());
                res += " {";
                if (Context.Instance.BlocskOpen > 0)
                {
                    temp = Context.Instance.Translations.Pop() + Environment.NewLine;
                    while (Context.Instance.BlockTranslation.Count != 0)
                    {
                        temp += Context.Instance.BlockTranslation.Dequeue() + Environment.NewLine;
                    }
                }
                Context.Instance.Translations.Push(temp + res);
                Context.Instance.BlocskOpen++;
            }
            if (production.ToString() == "funcion ::= inifuncion seqsentencias end")
            {
                res = Context.Instance.Translations.Pop() + Environment.NewLine;
                while (Context.Instance.BlockTranslation.Count != 0)
                {
                    res += Context.Instance.BlockTranslation.Dequeue() + Environment.NewLine;
                }
                res += "}" + Environment.NewLine;
                Context.Instance.Translations.Push(res);
                Context.Instance.BlocskOpen--;
            }
            return res;
        }


    }
}
