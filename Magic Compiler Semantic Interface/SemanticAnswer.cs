using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler
{
    public struct SemanticAnswer
    {
        public bool Valid;
        public string Message;

        public SemanticAnswer(bool valid, string message)
        {
            Valid = valid;
            Message = message;
        }
    }
}
