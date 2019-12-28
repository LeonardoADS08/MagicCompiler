using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCompiler.MatLab
{
    public class cg_SeqId : ICodeGenerator
    {
        public string[] Productions => new string[]
        {
            "iniseqid ::= id ,",
            "seqid ::= id , seqid",
            "seqid ::= id"
            

        };

        public bool ValidProduction(Production production) => Productions.Contains(production.ToString());

        public string Translate(List<Token> tokens, Production production)
        {

            if (production.ToString() == "iniseqid ::= id ,")
            {
                string res = "";
                int firstSymbolIndex = tokens.FindLastIndex(token => token.IsSymbol(Context.symbol_id));


                for (int i = firstSymbolIndex; i < tokens.Count; i++)
                {
                    if (tokens[i].IsSymbol(Context.symbol_id))
                    {
                        res += tokens[i].Lexeme;
                    }
                    else if (tokens[i].IsSymbol(Context.symbol_comma))
                    {
                        res += tokens[i].Lexeme;

                    }

                }
                Context.Instance.Translations.Enqueue(res);
                return res;
            }

            if (production.ToString() == "seqid ::= iniseqid  seqid")
            {
                string res = Context.Instance.Translations.Dequeue();
                Context.Instance.Translations.Enqueue(res);
                return res;
            }

            if(production.ToString() == "seqid ::= id")
            {
                string res = "";
                int firstSymbolIndex = tokens.FindLastIndex(token => token.IsSymbol(Context.symbol_id));

                for (int i = firstSymbolIndex; i < tokens.Count; i++)
                {
                    if (tokens[i].IsSymbol(Context.symbol_id))
                    {
                        res +=tokens[i].Lexeme + ", ";
                    }
                    

                }
                Context.Instance.Translations.Enqueue(res);
                return res;
            }
            return "fdf";
        }


    }
}
