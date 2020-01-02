using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;

namespace MagicCompiler.Scripting
{

    public interface ISemanticAnalyzer
    {
        bool RequiresEvaluation(Production reduceProduction);
        List<SemanticAnswer> Evaluate(List<Token> tokens, Production reduceProduction);
    }
}
