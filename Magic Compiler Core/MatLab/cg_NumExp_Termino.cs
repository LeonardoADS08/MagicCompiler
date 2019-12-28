using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCompiler.MatLab
{
    public class cg_NumExp_Termino : ICodeGenerator
    {
        public string[] Productions => new string[]
        {
            "termino ::= constante",
            "termino ::= id"
        };

        public bool ValidProduction(Production production) => Productions.Contains(production.ToString());

        public string Translate(List<Token> tokens, Production production)
        {
            string prod = production.ToString();
            string res = string.Empty;
            if (prod == "termino ::= constante" || prod == "termino ::= id")
            {
                res = tokens[tokens.Count - 1].Lexeme;
                Context.Instance.Translations.Push(res);
            }
            
            return res;
        }
    }
}
