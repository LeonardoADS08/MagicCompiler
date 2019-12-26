/*
 * sas = Semantic Analyzer Structure
 * sv  = Semantic Validation
 * 
 * ISemanticValidation = Common interface for validations
 */

using MagicCompiler.Semantic.Interfaces;
using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCompiler.Matlab
{
    public class MatlabSemantic : ISemanticAnalyzer
    {
        private Dictionary<string, List<ISemanticValidation>> _validations = new Dictionary<string, List<ISemanticValidation>>();

        public MatlabSemantic()
        {
            List<ISemanticValidation> validations = new List<ISemanticValidation>()
            {
                new sv_MatrixDefinition()
            };
            validations.ForEach(validation => AddValidation(validation));
        }

        private void AddValidation(ISemanticValidation IValidation)
        {
            foreach (var item in IValidation.Productions)
            {
                if (_validations.ContainsKey(item)) 
                    _validations[item].Add(IValidation);
                else
                    _validations[item] = new List<ISemanticValidation>() { IValidation };
            }
        }
        public bool RequiresEvaluation(Production reduceProduction) => _validations.ContainsKey(reduceProduction.ToString());

        public bool Evaluate(Token[] tokens, Production reduceProduction)
        {
            string production = reduceProduction.ToString();
            if (_validations.ContainsKey(production))
            {
                List<Token> tokenList = tokens.ToList();
                return _validations[production].TrueForAll(validation => validation.Evaluate(tokenList));
            }
            else return true;
        }
    }
}



