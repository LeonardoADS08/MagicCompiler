﻿using MagicCompiler.Structures.Grammar;
using MagicCompiler.Structures.Lexical;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.Matlab
{
    public interface ISemanticValidation
    {
        string Production { get; }
        bool Evaluate(List<Token> tokens, ss_Context context);
    }
}