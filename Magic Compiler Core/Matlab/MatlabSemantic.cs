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

        public SemanticAnswer Evaluate(List<Token> tokens, Production reduceProduction)
        {
            string production = reduceProduction.ToString();
            if (_validations.ContainsKey(production))
            {
                SemanticAnswer result = new SemanticAnswer();
                result.Valid = _validations[production].TrueForAll(validation =>
                {
                    var evaluation = validation.Evaluate(tokens);
                    result.Message += evaluation.Message + Environment.NewLine;
                    return evaluation.Valid;
                });
                if (result.Valid) result.Message = "No semantic issues";
                return result;
            }
            else return new SemanticAnswer(true, "No validation required");
        }
    }
}



