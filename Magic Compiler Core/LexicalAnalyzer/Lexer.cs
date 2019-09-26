using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MagicCompiler.LexicalAnalyzer
{
    public class Lexer
    {
        public List<Token> Tokens = new List<Token>();

        private string _input;
        private Tokenizer _patternRepo = new Tokenizer();

        private const string FILE_DIRECTION_INPUT = @"Data\input.txt";

        public Lexer()
        {
            using (var reader = new StreamReader(FILE_DIRECTION_INPUT))
            {
                _input = reader.ReadToEnd();
            }
            _input.Replace(Environment.NewLine, " ");
        }

        public void Tokenize()
        {
            string word = string.Empty, tempWord;
            
            for (int i = 0; i < _input.Length; i++)
            {
                word += _input[i];
                word = word.Trim();
                if (word == " " && word == Environment.NewLine)
                {
                    word = string.Empty;
                    continue;
                }

                // Si el lexema actual + el siguiente carácter pueden formar todavia distintos patrones, se sigue leyendo
                if (i + 1 < _input.Length && _input[i + 1] != ' ')
                {
                    tempWord = word + _input[i + 1];
                    if (_patternRepo.MatchToken(tempWord).Valid)
                        continue;
                }

                // Si no existen más posibilidades de tokens:
                Tokens.Add(_patternRepo.MatchToken(word).Token);
                word = string.Empty;
            }
        }

    }
}
