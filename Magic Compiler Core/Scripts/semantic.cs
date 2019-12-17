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
            case "matriz ::= [ seqelementos ]":
                return true;
        }
        return false;
    }

    public bool Evaluate(Token[] tokens, Production reduceProduction)
	{
		List<Token> tokenList = new List<Token>(tokens);
        switch(reduceProduction.ToString())
        {
            case "matriz ::= [ seqelementos ]":
                // Check if column count is correct
                int finalCount = 0, auxCount = 0;
                bool counted = false;
                for (int i = 0; i < tokenList.Count; i++)
                {
                    if (!counted)
                    {
                        if (tokenList[i].Symbol.TSymbol == "valor") finalCount++;
                        else if (tokenList[i].Symbol.TSymbol == ";") counted = true;
                    }
                    else
                    {
                        if (tokenList[i].Symbol.TSymbol == "valor") auxCount++;
                        else if (tokenList[i].Symbol.TSymbol == ";")
                        {
                            if (auxCount != finalCount) return false;
                            auxCount = 0;
                        }
                    }
                }
            break;
        }
        return false;
	}
}
