using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MagicCompiler.LexicalAnalyzer
{
    public class RegexDictionary
    {
        #region MyRegion
        private static RegexDictionary _instance;
        public static RegexDictionary Instance
        {
            get
            {
                if (_instance == null) _instance = new RegexDictionary();
                return _instance;
            }
        }
        #endregion

        private const char SEPARATOR_SYMBOL = '-';
        private readonly string DATA_FILE = AppDomain.CurrentDomain.BaseDirectory + @"Data\lexicalRegex.mc";

        private bool _isReady = false;
        public bool IsReady => _isReady;

        private List<LexicalRegex> _regexList;

        public RegexDictionary()
        {
            Initialize();
        }

        private async void Initialize()
        {
            ConcurrentBag<LexicalRegex> tokenList = new ConcurrentBag<LexicalRegex>();
            ConcurrentBag<string> notRecognizedTokens = new ConcurrentBag<string>();
            var parallelLoopResult = Parallel.ForEach(await File.ReadAllLinesAsync(DATA_FILE), (line, loopState, lineNumber) =>
            {
                var rawToken = line.Split(SEPARATOR_SYMBOL, 2);
                if (rawToken.Length == 2)
                {
                    tokenList.Add(new LexicalRegex(rawToken[0].Trim(), rawToken[1].Trim()));
                }
                else notRecognizedTokens.Add(line);
            });

            while (!parallelLoopResult.IsCompleted) await Task.Delay(10);
            _isReady = true;

            _regexList = new List<LexicalRegex>(tokenList.ToArray());
        }

        public Token Tokenize(string expression)
        {
            foreach (var regex in _regexList)
            {
                if (regex.IsMatch(expression))
                {
                   return new Token(regex.Alias, expression);
                }
            }
            return Token.INVALID_TOKEN;
        }

    }
}
