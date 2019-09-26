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
        }

        public void Tokenize()
        {
            string word = string.Empty, tempWord;
            
            for (int i = 0; i < _input.Length; i++)
            {
                word += _input[i];

                // Si el lexema actual + el siguiente carácter pueden formar todavia distintos patrones, se sigue leyendo
                if (i + 1 < _input.Length)
                {
                    tempWord = word + _input[i + 1];
                    if (_input[i + 1].ToString() != Environment.NewLine && _patternRepo.MatchToken(tempWord).Valid)
                        continue;
                }

                // Si no existen más posibilidades de tokens:
                Tokens.Add(_patternRepo.MatchToken(word.Trim()).Token);
                word = string.Empty;
            }
        }

    }
}
