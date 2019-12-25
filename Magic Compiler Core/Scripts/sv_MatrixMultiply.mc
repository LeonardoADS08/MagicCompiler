using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCompiler.Matlab
{
    public class sv_MatrixMultiply : ISemanticValidation
    {
        public string[] Productions => new string[] { "exparitmetica :: = valor * exparitmetica" };
        public bool ValidProduction(Production production) => Productions.Contains(production.ToString());

        public bool Evaluate(List<Token> tokens, ss_Context context)
        {
            throw new NotImplementedException();
        }
    }
}
