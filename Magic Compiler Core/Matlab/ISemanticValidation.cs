using MagicCompiler.Scripting;
using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.MatLab
{
    public interface ISemanticValidation
    {
        string[] Productions { get; }
        bool ValidProduction(Production production);
        List<SemanticAnswer> Evaluate(List<Token> token);
    }
}
