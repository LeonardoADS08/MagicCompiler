using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.Grammar
{
    public class Rule
    {
        public string Left;
        public List<string> Right = new List<string>(); 
    }
}
