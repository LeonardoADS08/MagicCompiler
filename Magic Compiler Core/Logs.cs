using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MagicCompiler
{

    public static class Logs
    {
        private static bool _started = false;
        public const string DEFAULT_LOG = "[Default]";

        public static void SaveLog(string message, string direction, bool chronology = true, string logName = DEFAULT_LOG)
        {
            try
            {
                string logFile = Path.Combine(AppContext.BaseDirectory, string.Format(string.Format("{0} - {1}", logName, "Log {0}.log"), DateTime.Today.ToString("dd-MM-yyyy")));
                if (_started)
                {
                    string texto = "\r\n \r\n______________________________ MagicCompiler - Execution Log: " + DateTime.Now.ToString("dd-MM-yyyy hh:mm") + " ______________________________\r\n";
                    File.AppendAllText(logFile, texto);
                    _started = true;
                }

                // El mensaje que será guardado en el Log
                string final = "";
                if (chronology) final = DateTime.Now.ToString("hh:mm:ss") + ": " + direction + " - " + message + "\r\n";
                else final = direction + " - " + message + "\r\n";

                // Se guarda en el archivo
                File.AppendAllText(logFile, final);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Obtiene el nombre de la funcion de la cual se esta llamando.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetDirection()
        {
            try
            {
                StackTrace st = new StackTrace();
                StackFrame sf = st.GetFrame(1);
                return sf.GetMethod().DeclaringType.Name + "." + sf.GetMethod().Name;
            }
            catch
            {
                return "Direction not encountered.";
            }
        }
    }
}
