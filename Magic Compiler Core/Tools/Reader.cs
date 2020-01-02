using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MagicCompiler.Tools
{
    public class Reader
    {
        private const string COMMENT_SYMBOL = "#";
        
        public string FileDirection { get; set; }

        public Reader(string fileDirection)
        {
            FileDirection = fileDirection;
        }

        public string ReadAll() => ReadAll(null);

        public string ReadAll(Func<string, string> postProcess)
        {
            string result = string.Empty;
            using (var reader = new StreamReader(FileDirection))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (!line.StartsWith(COMMENT_SYMBOL) && !string.IsNullOrWhiteSpace(line))
                    {
                        if (postProcess != null) line = postProcess.Invoke(line);
                        result += line + Environment.NewLine;
                    }
                }
            }
            return result;
        }

        public List<string> ReadLines() => ReadLines(null);
        public List<string> ReadLines(Func<string, string> postProcess)
        {
            List<string> result = new List<string>();
            using (var reader = new StreamReader(FileDirection))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (!line.StartsWith(COMMENT_SYMBOL) && !string.IsNullOrWhiteSpace(line))
                    {
                        if (postProcess != null) line = postProcess.Invoke(line);
                        result.Add(line);
                    }
                }
            }
            return result;
        }
    }
}
