using MagicCompiler.Grammars;
using MagicCompiler.Lexical;
using MagicCompiler.Syntactic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler
{
    public class Compiler
    {
        private ILexer _lexer;
        private IParser _parser;

        public Compiler(ILexer lexer, IParser parser)
        {
            _lexer = lexer;
            _parser = parser;
        }

        public bool Compile()
        {
            _lexer.Analyze();
            return _parser.Check();
        }
    }
}
