
using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCompiler.MatLab
{
    public class cg_AritExp_Operations : ICodeGenerator
    {
        public string[] Productions => new string[]
        {
            "exparitmetica ::= ( exparitmetica )",
            "exparitmetica ::= iniexparitmetica termino",
            "iniexparitmetica ::= termino oparitmetico",
            "iniexparitmetica ::= exparitmetica oparitmetico"
        };

        public bool ValidProduction(Production production) => Productions.Contains(production.ToString());

        public string Translate(List<Token> tokens, Production production)
        {
            string res = string.Empty, prod = production.ToString();
            if (prod == "exparitmetica::= ( exparitmetica )")
            {
                res = string.Format("({0})", Context.Instance.Translations.Pop());
                Context.Instance.Translations.Push(res);
            }
            else if (prod == "exparitmetica ::= iniexparitmetica termino")
            {
                res = string.Format("{1} {0})", Context.Instance.Translations.Pop(), Context.Instance.Translations.Pop());
                Context.Instance.Translations.Push(res);
            }
            else if (prod == "iniexparitmetica ::= termino oparitmetico" || prod == "iniexparitmetica ::= exparitmetica oparitmetico")
            {
                res = OperatorToTranslation(tokens.Last().Lexeme, Context.Instance.Translations.Pop());
                Context.Instance.Translations.Push(res);
            }
            return res;
        }

        private string OperatorToTranslation(string op, string term1)
        {
            string translatedOperator = string.Empty;
            switch (op)
            {
                case @"+":
                    translatedOperator = "matrixAdd(";
                    break;
                case @"-":
                    translatedOperator = "matrixSub(";
                    break;
                case @"*":
                    translatedOperator = "matrixMultiply(";
                    break;
                case @".*":
                    translatedOperator = "matrixDotMultiply(";
                    break;
                case @"./":
                    translatedOperator = "matrixDotRightDivide(";
                    break;
                case @".\":
                    translatedOperator = "matrixDotLeftDivide(";
                    break;
                case @"/":
                    translatedOperator = "matrixRightDivide(";
                    break;
                case @"\":
                    translatedOperator = "matrixLeftDivide(";
                    break;
                case @".^":
                    translatedOperator = "matrixDotPower(";
                    break;
                case @"^":
                    translatedOperator = "matrixPower(";
                    break;
                default:
                    translatedOperator =  "matrixAdd(";
                    break;
            }
            return string.Format("{0}{1},", translatedOperator, term1);
        }
    }
}

