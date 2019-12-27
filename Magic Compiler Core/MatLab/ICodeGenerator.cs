using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.MatLab
{
    public interface ICodeGenerator
    {
        string Production { get; }
        string Translate(List<Token> tokens);
    }
}
