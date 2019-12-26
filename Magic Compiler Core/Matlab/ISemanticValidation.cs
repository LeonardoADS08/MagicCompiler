using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.Matlab
{
    public interface ISemanticValidation
    {
        string[] Productions { get; }
        bool ValidProduction(Production production);
        bool Evaluate(List<Token> tokens);
    }
}
