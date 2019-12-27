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
        public string Production => "llamadafuncion ::= id ( parametros )";

        public string Translate(List<Token> tokens)
        {
            throw new NotImplementedException();
        }

       
    }
}
