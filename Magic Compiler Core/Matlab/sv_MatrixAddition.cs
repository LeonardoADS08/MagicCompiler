using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.Matlab
{
    public class sv_MatrixAddition : ISemanticValidation
    {
        public string Production => "exparitmetica ::= valor + exparitmetica";

        public bool Evaluate(List<Token> tokens, ss_Context context)
        {
            throw new NotImplementedException();
        }

        public bool ValidProduction(Production production)
        {
            var productionString = production.ToString();
            return productionString == "exparitmetica ::= valor + exparitmetica";
        }
    }
}
