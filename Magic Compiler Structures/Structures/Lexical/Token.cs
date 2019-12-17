using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.Structures.Lexical
{
    public struct Token
    {
        private readonly static Token _nullInstance = new Token("[NULL]", new Symbol(INVALID_TYPE, "[NULL]", "[NULL]"));
        public static Token NULL_TOKEN => _nullInstance;

        public const string INVALID_TYPE = "INVALID";


        public string Lexeme { get; set; }
        public Symbol Symbol { get; set; }

        public Token(string lexeme, Symbol symbol)
        {
            Lexeme = lexeme;
            Symbol = symbol;
        }

        #region Tests
        public void PrintToken()
        {
            Console.Write(Lexeme + " -> ");
            Symbol.PrintSymbol();
            Console.WriteLine();
        }

        public void PrintTokenForParser()
        {
            Console.WriteLine("{0} -> {1}", Lexeme, Symbol.TSymbol);
        }
        #endregion
    }
}
