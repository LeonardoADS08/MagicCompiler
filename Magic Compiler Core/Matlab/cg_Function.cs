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
            "inifuncion ::= function id ( seqid )"
        };

        public bool ValidProduction(Production production) => Productions.Contains(production.ToString());

        public string Translate(List<Token> tokens, Production production)
        {
            string res = "function";
            int firstSymbolIndex = tokens.FindLastIndex(token => token.IsSymbol(Context.symbol_function)) + 1;


            for (int i = firstSymbolIndex; i < tokens.Count; i++)
            {
                if (tokens[i].IsSymbol(Context.symbol_id))
                {
                    res += tokens[i].Lexeme;
                }
                else if (tokens[i].IsSymbol(Context.symbol_openParenthesis))
                {
                    res += tokens[i].Lexeme;
                    res += Context.Instance.Translations.Pop();

                }
                else if (tokens[i].IsSymbol(Context.symbol_closeParenthesis))
                {
                    res += tokens[i].Lexeme;
                }
            }
            Context.Instance.Translations.Push(res);
            return res;
        }


    }
}
