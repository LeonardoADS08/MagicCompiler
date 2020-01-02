using MagicCompiler.Grammars;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MagicCompiler.Lexical
{
    public class LexerMC : Lexer
    {
        private string _input;

        public override List<Token> Tokens => _tokens;
        private List<Token> _tokens = new List<Token>();
        private IGrammar _grammar;
        private string FILE_DIRECTION_INPUT => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data/input.txt");

        public LexerMC(IGrammar grammar, ITokenizer tokenizer) : base(tokenizer)
        {
            _grammar = grammar;
            Read();
        }

        protected void Read()
        {
            using (var reader = new StreamReader(FILE_DIRECTION_INPUT))
            {
                _input = reader.ReadToEnd().Trim();
            }
        }

        public override void Analyze()
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
                word = string.Empty;
            }

            var acceptedProduction = _grammar.AugmentedGrammar;
            Tokens.Add(new Token(acceptedProduction.Left, new Symbol(acceptedProduction.Left, acceptedProduction.Left)));
        }
    }
}
