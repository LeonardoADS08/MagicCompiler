using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MagicCompiler.Grammar
{
    /*
        El formato de las reglas debería tener la forma:
        (forma 1)
            SIMBOLO_NO_TERMINAL ::= SIMBOLO

        ó (forma 2)
            SIMBOLO_NO_TERMINAL ::= SIMBOLO_1 | SIMBOLO_2 | ... | SIMBOLO_N

        Los simbolos son un conjunto de caracteres, por ejemplo la siguiente producción:
                
            expression ::= expression + term

        donde los simbolos de una regla pueden tener múltiples simbolos no terminales (y no terminales)
        siempre y cuando esten bien separados por espacios, si están juntos se considerará 
        como un solo simbolo:

            [OK] expression ::= expression - term
            [OK] term ::= term * factor
            [OK] expression::= expression - term
                    
            [NO] expression ::= expression-term
            [NO] term ::= term* factor
            [NO] term ::= term *factor
            [NO] factor ::= (expression)

        No hay importancia entre mayusculas y minusculas, dado a que todo se pasará a minusculas para
        mejor manejo de las reglas.

        Cualquier otra forma sera digno de un error :)
    */
    public class Reader
    {
        private string FILE_DIRECTION_GRAMMAR_RULES => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data/grammar_rules.txt");
        private string FILE_DIRECTION_GRAMMAR_CONFIGURATIONS => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data/grammar_config.json");

        private List<string> _rawRules = new List<string>();
        private CFGConfig _config;

        public Reader()
        {
            using (var reader = new StreamReader(FILE_DIRECTION_GRAMMAR_RULES))
            {
                while (!reader.EndOfStream)
                    _rawRules.Add(reader.ReadLine().Trim().ToLower());
            }

            using (var reader = new StreamReader(FILE_DIRECTION_GRAMMAR_CONFIGURATIONS))
            {
                string config = reader.ReadToEnd().Trim();
                _config = Newtonsoft.Json.JsonConvert.DeserializeObject<CFGConfig>(config);
                _config.StartSymbol = _config.StartSymbol.ToLower();
            }

        }

        public CFG Build()
        {
            var cfg = new CFG();

            cfg.Configuration = _config;
            for (int i = 0; i < _rawRules.Count; i++)
            {
                List<string> leftRight = new List<string>(_rawRules[i].Split("::="));
                string left = leftRight[0].Trim(); // added trim because it was reading an extra space char

                List<string> right = new List<string>(leftRight[1].Split("|"));
                for (int j = 0; j < right.Count; j++)
                {
                    Rule duaLipaNewRule = new Rule() { Left = left };
                    duaLipaNewRule.Right.AddRange(right[j].Trim().Split(" "));
                    cfg.Productions.Add(duaLipaNewRule);
                }
            }

            return cfg;
        }
    }
}