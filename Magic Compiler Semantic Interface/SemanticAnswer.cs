using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler
{
    
    public struct SemanticAnswer
    {
        public AnswerType AnswerType;
        public string Message;

        public SemanticAnswer(AnswerType answerType, string message)
        {
            AnswerType = answerType;
            Message = message;
        }
    }
}
