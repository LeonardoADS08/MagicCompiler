using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.Matlab
{
    public class ss_Context
    {
        public const string symbol_id = "id";
        public const string symbol_constant = "constante";
        public const string symbol_openBracket = "[";
        public const string symbol_closeBracket = "]";
        public const string symbol_semicolon = ";";

        public List<List<Token>> Tokens;
        public Dictionary<string, sas_Variable<sas_Matrix<double>>> Matrixes;
        public Dictionary<string, sas_Variable<string>> Strings;

        public ss_Context()
        {
            Tokens = new List<List<Token>>();
            Matrixes = new Dictionary<string, sas_Variable<sas_Matrix<double>>>();
            Strings = new Dictionary<string, sas_Variable<string>>();
        }
    }
}
