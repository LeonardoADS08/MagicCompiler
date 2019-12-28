using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler
{
    public interface ITranslator
    {
        bool RequiresTranslation(Production reduceProduction);
        void Translate(List<Token> tokens, Production reduceProduction);
    }
}
