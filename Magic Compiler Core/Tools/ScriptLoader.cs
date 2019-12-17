using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace MagicCompiler.Tools
{
    public abstract class ScriptLoader
    {
        protected Assembly _assembly;
        protected ScriptEngine _scriptEngine;
        protected ScriptParams _scriptParams;

        protected abstract string SCRIPT_PARAMS { get; }

        public Assembly Assembly => _assembly;

        public ScriptLoader()
        {
            using (StreamReader reader = new StreamReader(SCRIPT_PARAMS))
            {
                string parameters = reader.ReadToEnd().Trim();
                _scriptParams = Newtonsoft.Json.JsonConvert.DeserializeObject<ScriptParams>(parameters);
            }

            _scriptEngine = new ScriptEngine(_scriptParams);
            _assembly = _scriptEngine.Compile();
        }

    }
}
