﻿using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCompiler.MatLab
{
    public class cg_MatDef_Matrix : ICodeGenerator
    {
        public string[] Productions => new string[]
        {
            "matriz ::= [ fila ]"
        };

        public bool ValidProduction(Production production) => Productions.Contains(production.ToString());

        public string Translate(List<Token> tokens, Production production)
        {
            string res = "[[";
            int firstSymbolIndex = tokens.FindLastIndex(token => token.IsSymbol(Context.symbol_openBracket));


            for (int i = firstSymbolIndex; i < tokens.Count; i++)
            {
                if (tokens[i].IsSymbol(Context.symbol_constant, Context.symbol_comma))
                {
                    res += tokens[i].Lexeme ;
                }
                else if (tokens[i].IsSymbol(Context.symbol_semicolon))
                {
                    res += "],[";
                }
                else if (tokens[i].IsSymbol(Context.symbol_closeBracket))
                {
                    res += "]]";
                }
            }
            res = string.Format("math.matrix({0})", res);
            Context.Instance.Translations.Push(res);
            return res;
        }


    }
}
