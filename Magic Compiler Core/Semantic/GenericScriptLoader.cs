using MagicCompiler.Scripting;
using MagicCompiler.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace MagicCompiler.Semantic
{
    public class GenericScriptLoader : ScriptLoader
    {
        protected override string SCRIPT_PARAMS => Path.Combine(ScriptEngine.FILE_DIRECTION_SCRIPTS, "scriptParams.json");
        private object _assemblyInstance;
        public GenericScriptLoader() : base() { }

        public ISemanticAnalyzer GetSemanticAnalyzer()
        {
            if (_assemblyInstance == null)
                _assemblyInstance = _assembly.CreateInstance<object>(_scriptParams.DefaultClass);
            return (ISemanticAnalyzer)_assemblyInstance;
        }

        public ITranslator GetTranslator()
        {
            if (_assemblyInstance == null)
                _assemblyInstance = _assembly.CreateInstance<object>(_scriptParams.DefaultClass);
            return (ITranslator)_assemblyInstance;
        }
    }
}
