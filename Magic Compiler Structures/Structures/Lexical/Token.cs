using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCompiler.Structures.Lexical
{
    public struct Token
    {
        private readonly static Token _nullInstance = new Token("[NULL]", new Symbol(INVALID_TYPE, "[NULL]"));
        public static Token NULL_TOKEN => _nullInstance;

        public const string INVALID_TYPE = "INVALID";


        public string Lexeme { get; set; }
        public Symbol Symbol { get; set; }

        public Token(string lexeme, Symbol symbol)
        {
            Lexeme = lexeme;
            Symbol = symbol;
        }

        public bool IsSymbol(params string[] symbol)
        {
            for (int i = 0; i < symbol.Length; i++)
                if (symbol[i] == Symbol.Type) return true;
            return false;
        }

        public override string ToString() => Lexeme + " - " + Symbol.Type;

        #region Tests
        public void PrintToken()
        {
            Console.Write(Lexeme + " -> ");
            Symbol.PrintSymbol();
            Console.WriteLine();
        }

        public void PrintTokenForParser()
        {
            Console.WriteLine("{0} -> {1}", Lexeme, Symbol.Type);
        }
        #endregion
    }
}
