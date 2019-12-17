using MagicCompiler.Semantic.Interfaces;
using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.Preprocessing
{
    class Preprocessor : ISemanticAnalyzer
    {

        public bool Analyze(Token[] tokens, Production reduceProduction)
        {
            List<Token> tokenList = new List<Token>(tokens);

            switch (reduceProduction.ToString())
            {
                case "seqelementos ::= valor":
                    break;
            }
            return true;
        }

        public bool Evaluate(Token[] tokens, Production reduceProduction)
        {
            throw new NotImplementedException();
        }

        public bool RequiresEvaluation(Production reduceProduction)
        {
            throw new NotImplementedException();
        }
    }
}
