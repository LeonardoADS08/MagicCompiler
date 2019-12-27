using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.MatLab
{
    public interface ICodeGenerator
    {
        string[] Productions { get; }
        bool ValidProduction(Production production);
        string Translate(List<Token> tokens);
    }
}
