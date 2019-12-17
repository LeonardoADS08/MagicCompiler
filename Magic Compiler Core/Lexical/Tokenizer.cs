using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MagicCompiler.Lexical
{
    internal class Tokenizer
    {
        private List<Symbol> _reservedWords;
        private List<Symbol> _regex;

        private string FILE_DIRECTION_KEYWORDS => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data/lexical_keywords.txt");
        private string FILE_DIRECTION_OPERATORS => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data/lexical_operators.txt");
        private string FILE_DIRECTION_REGEX => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data/lexical_regex.txt");

        public const string TYPE_KEYWORD = "KEYWORD";

        public Tokenizer()
        {
            _reservedWords = new List<Symbol>();
            _regex = new List<Symbol>();

            using (var reader = new StreamReader(FILE_DIRECTION_KEYWORDS))
            {
                while (!reader.EndOfStream)
                {
                    string input = reader.ReadLine().Trim().ToLower();
                    _reservedWords.Add(new Symbol("Keyword", input, input));
                }
            }

            using (var reader = new StreamReader(FILE_DIRECTION_OPERATORS))
            {
                while (!reader.EndOfStream)
                {
                    string input = reader.ReadLine().Trim().ToLower();
                    _reservedWords.Add(new Symbol("Operator", input, input));
                }
            }

            using (var reader = new StreamReader(FILE_DIRECTION_REGEX))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().ToLower();
                    var rawSymbol = line.Split('-', 2);
                    if (rawSymbol.Length == 2)
                    {
                        _regex.Add(new Symbol(rawSymbol[0].Trim(), rawSymbol[1].Trim(), rawSymbol[0].Trim()));
                    }
                }
            }
        }

        public TokenizerAnswer MatchToken(string word)
        {
            word = word.Trim();

            // Palabras reservadas
            for (int symbolIndex = 0; symbolIndex < _reservedWords.Count; symbolIndex++)
            {
                if (_reservedWords[symbolIndex].Pattern == word)
                    return new TokenizerAnswer(true, new Token(word, _reservedWords[symbolIndex]));
            }

            // Regex
            for (int i = 0; i < _regex.Count; i++)
            {
                Regex regex = new Regex(_regex[i].Pattern);
                if (regex.IsMatch(word))
                    return new TokenizerAnswer(true, new Token(word, _regex[i]));
            }

            return TokenizerAnswer.NULL_ANSWER;
        }

        #region Test
        public void REGEX_TEST(string test)
        {
            for (int i = 0; i < _regex.Count; i++)
            {
                Regex regex = new Regex(_regex[i].Pattern);
                if (regex.IsMatch(test))
                    Console.WriteLine(test + " - OK -> " + _regex[i].Pattern);
            }
        }
        #endregion
    }
}
