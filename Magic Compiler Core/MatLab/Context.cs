using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.MatLab
{
    public class Context
    {
        #region Singleton
        private static Context _instance;
        public static Context Instance
        {
            get
            {
                if (_instance == null) _instance = new Context();
                return _instance;
            }
        }
        #endregion

        public const string symbol_id = "id";
        public const string symbol_constant = "constante";
        public const string symbol_openParenthesis = "(";
        public const string symbol_closeParenthesis = ")";
        public const string symbol_openBracket = "[";
        public const string symbol_closeBracket = "]";
        public const string symbol_comma = ",";
        public const string symbol_semicolon = ";";
        public const string symbol_equal = "=";
        public const string symbol_function = "function";
        public const string symbol_while = "while";
        public const string symbol_if = "if";
        public const string symbol_for = "for";
        public const string symbol_end = "end";
        
        public HashSet<string> Assignations = new HashSet<string>();
        public HashSet<string> Functions = new HashSet<string>();
        public Stack<string> Translations = new Stack<string>();
        public int BlocskOpen = 0;
        public List<string> FinalTranslation = new List<string>();
    }
}
