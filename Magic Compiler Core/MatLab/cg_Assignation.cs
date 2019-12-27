using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCompiler.MatLab
{
    public class cg_Assignation : ICodeGenerator
    {
        public string Production => "asignacion ::= termino";

        public string Translate(List<Token> tokens)
        {
            int index = tokens.FindLastIndex(Token => Token.IsSymbol(Context.symbol_equal)) - 1;
            string res = string.Format("{0}={1}", tokens[index].Lexeme, Context.Instance.Translations.Dequeue());
            Context.Instance.Translations.Enqueue(res);
            return res;
        }

       
    }
}
