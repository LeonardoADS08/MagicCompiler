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
        public ss_Context _context = new ss_Context();
        private Dictionary<string, ISemanticValidation> _validations = new Dictionary<string, ISemanticValidation>();

        public MatlabSemantic()
        {
            List<ISemanticValidation> validations = new List<ISemanticValidation>()
            {
                new sv_MatrixDefinition(),
                new sv_MatrixAddition(),
                new sv_MatrixSubstract(),
                new sv_MatrixMultiply(),
                new sv_MatrixDivision()
            };
            validations.ForEach(validation => AddValidation(validation));

        }

        private void AddValidation(ISemanticValidation IValidation) => _validations.TryAdd(IValidation.Production, IValidation);
        public bool RequiresEvaluation(Production reduceProduction) => _validations.ContainsKey(reduceProduction.ToString());

        public bool Evaluate(Token[] tokens, Production reduceProduction)
        {
            string production = reduceProduction.ToString();
            if (_validations.ContainsKey(production))
            {
                List<Token> tokenList = tokens.ToList();
                return _validations[production].Evaluate(tokenList, _context);
            }
            else return true;
        }
    }
}



