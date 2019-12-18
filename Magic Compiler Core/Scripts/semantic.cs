using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using MagicCompiler.Semantic.Interfaces;
using MagicCompiler.Structures.Lexical;
using MagicCompiler.Structures.Grammar;


public class SemanticAnalyzer : ISemanticAnalyzer
{
    public bool RequiresEvaluation(Production reduceProduction)
    {
        switch (reduceProduction.ToString())
        {
            case "matriz ::= [ seqelementos ; fila ]":
                return true;
        }
        return false;
    }

    public bool Evaluate(Token[] tokens, Production reduceProduction)
	{
		List<Token> tokenList = new List<Token>(tokens);
        if (reduceProduction.ToString() == "matriz ::= [ seqelementos ; fila ]")
        { 
            // Check if column count is correct
            int finalCount = 0, auxCount = 0;
            bool counted = false;
            for (int i = 0; i < tokenList.Count; i++)
            {
                if (!counted)
                {
                    if (tokenList[i].Symbol.TSymbol == "constante") finalCount++;
                    else if (tokenList[i].Symbol.TSymbol == ";") counted = true;
                }
                else
                {
                    if (tokenList[i].Symbol.TSymbol == "constante") auxCount++;
                    else if (tokenList[i].Symbol.TSymbol == ";" || tokenList[i].Symbol.TSymbol == "]")
                    {
                        if (auxCount != finalCount) return false;
                        auxCount = 0;
                    }
                }
            }
        }
        return true;
    }
}
