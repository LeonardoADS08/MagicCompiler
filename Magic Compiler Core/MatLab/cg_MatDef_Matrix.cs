using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCompiler.MatLab
{
    public class cg_MatDef_Matrix : ICodeGenerator
    {
        public string Production => "matriz ::= [ fila ]";

        public string Translate(List<Token> tokens)
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
            Context.Instance.Translations.Enqueue(res);
            return res;
        }


    }
}
