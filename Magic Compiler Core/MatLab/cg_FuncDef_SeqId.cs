using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCompiler.MatLab
{
    public class cg_FuncDef_SeqID : ICodeGenerator
    {
        public string[] Productions => new string[]
        {
          "seqid ::= seqid , id",
          "seqid ::= iniseqid id",
          "seqid ::= id",
          "iniseqid ::= id ,"
          
          
        };

        public bool ValidProduction(Production production) => Productions.Contains(production.ToString());

        public string Translate(List<Token> tokens, Production production)
        {
            string prod = production.ToString();
            string res = string.Empty;
            if (prod == "iniseqid ::= id ,")
            {
                int n = tokens.FindLastIndex(token => token.IsSymbol(Context.symbol_id));
                res += tokens[n].Lexeme + " , ";
                Context.Instance.Translations.Push(res);

            }
            else if (prod == "seqid ::= id")
            {
                res = string.Format("{0} ", tokens[tokens.FindLastIndex(token => token.IsSymbol(Context.symbol_id))]);
                Context.Instance.Translations.Push(res);
            }
            else if (prod == "seqid ::= iniseqid id")
            {
                res = string.Format(" {0} {1} ", Context.Instance.Translations.Pop(), tokens[tokens.FindLastIndex(token => token.IsSymbol(Context.symbol_id))].Lexeme);
                Context.Instance.Translations.Push(res);
            }
            else if (prod == "seqid ::= seqid , id")
            {
                res = string.Format(" {0} , {1} ", Context.Instance.Translations.Pop(), tokens[tokens.FindLastIndex(token => token.IsSymbol(Context.symbol_id))].Lexeme);
                Context.Instance.Translations.Push(res);
            }
            return res;
        }
    }
}
