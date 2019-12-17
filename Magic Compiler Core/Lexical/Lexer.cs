using MagicCompiler.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MagicCompiler.Lexical
{
    internal class Lexer
    {
        public List<Token> Tokens = new List<Token>();

        private string _input;
        private Tokenizer _tokenizer = new Tokenizer();
        private int _nextIterator = 0;
        private string FILE_DIRECTION_INPUT => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data/input.txt");

        public Lexer()
        {
            using (var reader = new StreamReader(FILE_DIRECTION_INPUT))
            {
                _input = reader.ReadToEnd().Trim();
            }
        }

        public void Analyze() => Analyze(null);

        public void Analyze(Action<Token> action)
        {
            string word = string.Empty, tempWord;
            
            for (int i = 0; i < _input.Length; i++)
            {
                word += _input[i];

                // Si el lexema actual + el siguiente carácter pueden formar todavia distintos patrones, se sigue leyendo
                if (i + 1 < _input.Length)
                {
                    tempWord = word + _input[i + 1];
                    if (_input[i + 1].ToString() != Environment.NewLine && _tokenizer.MatchToken(tempWord).Valid)
                        continue;
                }

                // Si no existen más posibilidades de tokens:
                var token = _tokenizer.MatchToken(word.Trim()).Token;
                Tokens.Add(token);
                action?.Invoke(token);
                word = string.Empty;
            }

            var acceptedProduction = CFG.Instance.Configuration.AugmentedGrammar;
            Tokens.Add(new Token(acceptedProduction.Left, new Symbol("Acceptation symbol", acceptedProduction.Left, acceptedProduction.Left)));
        }

        public Token Next()
        {
            if (_nextIterator + 1 < Tokens.Count)
            {
                var result = Tokens[_nextIterator];
                _nextIterator++;
                return result;
            }
            else return Tokens[_nextIterator];
        }
    }
}
