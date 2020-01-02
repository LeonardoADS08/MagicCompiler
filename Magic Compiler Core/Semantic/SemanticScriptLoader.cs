using MagicCompiler.Scripting;
using MagicCompiler.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace MagicCompiler.Semantic
{
    public class SemanticScriptLoader : ScriptLoader
    {
        protected override string SCRIPT_PARAMS => Path.Combine(ScriptEngine.FILE_DIRECTION_SCRIPTS, "semanticParams.json");

        public SemanticScriptLoader() : base() { }

        public ISemanticAnalyzer GetSemanticAnalyzer() => _assembly.CreateInstance<ISemanticAnalyzer>(_scriptParams.DefaultClass);

    }
}
