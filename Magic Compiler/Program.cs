using MagicCompiler.LexicalAnalyzer;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Magic_Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();
            Console.ReadKey();
        }

        private static async void Test()
        {
            var sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"testFile.txt");
            var text = await sr.ReadToEndAsync();
            var words = text.Split(' ');

            while (!RegexDictionary.Instance.IsReady) await Task.Delay(50);

            for (int i = 0; i < words.Length; i++)
            {
                var token = RegexDictionary.Instance.Tokenize(words[i]);
                Console.WriteLine(token.Lexeme + " - " + token.Value);
            }
        }
    }
}
