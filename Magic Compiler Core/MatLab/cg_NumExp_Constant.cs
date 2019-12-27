using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCompiler.MatLab
{
    public class cg_NumExp_Constant : ICodeGenerator
    {
        public string Production => "termino ::= constante";

        public string Translate(List<Token> tokens)
        {
            string res = tokens[tokens.Count - 1].Lexeme;
            Context.Instance.Translations.Enqueue(res);
            return res;
        }
    }
}
