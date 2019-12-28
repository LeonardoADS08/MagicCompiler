/*
 * sv  = Semantic Validation
 * cg  = Code Generator
 * ISemanticValidation = Common interface for validations
 */

using MagicCompiler.Semantic.Interfaces;
using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MagicCompiler.MatLab
{
    public class MatLabJS : ISemanticAnalyzer, ITranslator
    {
        private Dictionary<string, List<ISemanticValidation>> _validations = new Dictionary<string, List<ISemanticValidation>>();
        private Dictionary<string, ICodeGenerator> _translations = new Dictionary<string, ICodeGenerator>();

        private string FILE_OUTPUT => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"result.js");

        public MatLabJS()
        {
            List<ISemanticValidation> validations = new List<ISemanticValidation>()
            {
                new sv_Assignation(),
                new sv_FunctionCall(),
                new sv_FunctionNoReturnDefinition(),
                new sv_FunctionParameterlessCall(),
                new sv_FunctionReturnDefinition(),
                new sv_IDCall(),
                new sv_MatrixDefinition(),
                new sv_MultiAssignation()
            };
            validations.ForEach(validation => AddValidation(validation));

            List<ICodeGenerator> translations = new List<ICodeGenerator>()
            {
                new cg_AritExp_Operations(),
                new cg_Core_Sentence(),
                new cg_CtrlStr_Control(),
                new cg_CtrlStr_Flow(),
                new cg_FuncDef_Function(),
                new cg_FuncDef_Llamada(),
                new cg_FuncDef_Parameters(),
                new cg_FuncDef_SeqID(),
                new cg_LogExp_Operations(),
                new cg_MatDef_Matrix(),
                new cg_NumExp_Assignation(),
                new cg_NumExp_Termino()
            };
            translations.ForEach(translation => AddTranslation(translation));

            StreamWriter writer = new StreamWriter(FILE_OUTPUT, false);
            writer.Dispose();
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

        private void AddTranslation(ICodeGenerator translation)
        {
            foreach (var item in translation.Productions)
            {
                if (!_translations.ContainsKey(item))
                {
                    _translations.Add(item, translation);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Duplicated translation for: {0}", item);
                    Console.ResetColor();
                }
            }
        }

        public bool RequiresEvaluation(Production reduceProduction) => _validations.ContainsKey(reduceProduction.ToString());
        public bool RequiresTranslation(Production reduceProduction) => _translations.ContainsKey(reduceProduction.ToString());

        public List<SemanticAnswer> Evaluate(List<Token> tokens, Production reduceProduction)
        {
            string production = reduceProduction.ToString();
            if (_validations.ContainsKey(production))
            {
                List<SemanticAnswer> result = new List<SemanticAnswer>();
                _validations[production].ForEach(validation => result.AddRange(validation.Evaluate(tokens)));
                return result;
            }
            else return new List<SemanticAnswer>();
        }
        
        public void Translate(List<Token> tokens, Production reduceProduction)
        {
            string production = reduceProduction.ToString();
            if (_translations.ContainsKey(production))
            {
                string code = _translations[production].Translate(tokens, reduceProduction);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(code);
                Console.ResetColor();
                
                using (StreamWriter writer = new StreamWriter(FILE_OUTPUT, true))
                {
                    while (Context.Instance.FinalTranslation.Count != 0)
                    {
                        writer.WriteLine(Context.Instance.FinalTranslation.First());
                        Context.Instance.FinalTranslation.RemoveAt(0);
                    }
                }
            }
        }

    }
}



