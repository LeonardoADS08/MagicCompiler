using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.Tools
{
    public class ScriptParams
    {
        public string DefaultClass;
        public bool Translator;
        public List<string> References { get; set; }
        public List<string> Scripts { get; set; }
    }
}
