using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;

namespace MagicCompiler.Semantic.Interfaces
{

    public interface ISemanticAnalyzer
    {
        bool RequiresEvaluation(Production reduceProduction);
        SemanticAnswer Evaluate(List<Token> tokens, Production reduceProduction);
    }
}
