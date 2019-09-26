using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MagicCompiler.LexicalAnalyzer
{
    internal class Tokenizer
    {
        private List<Symbol> _reservedWords;
        private List<Symbol> _regex;

        private const string FILE_DIRECTION_KEYWORDS = @"Data\lexical_keywords.txt";
        private const string FILE_DIRECTION_OPERATORS = @"Data\lexical_operators.txt";
        private const string FILE_DIRECTION_REGEX = @"Data\lexical_regex.txt";

        public Tokenizer()
        {
            _reservedWords = new List<Symbol>();
            _regex = new List<Symbol>();

            using (var reader = new StreamReader(FILE_DIRECTION_KEYWORDS))
            {
                while (!reader.EndOfStream)
                {
                    _reservedWords.Add(new Symbol("Keyword", reader.ReadLine().Trim()));
                }
            }

            using (var reader = new StreamReader(FILE_DIRECTION_OPERATORS))
            {
                while (!reader.EndOfStream)
                {
                    _reservedWords.Add(new Symbol("Operator", reader.ReadLine().Trim()));
                }
            }

            using (var reader = new StreamReader(FILE_DIRECTION_REGEX))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    var rawSymbol = line.Split('-', 2);
                    if (rawSymbol.Length == 2)
                    {
                        _regex.Add(new Symbol(rawSymbol[0].Trim(), rawSymbol[1].Trim()));
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
