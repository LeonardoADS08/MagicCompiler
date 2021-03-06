﻿using MagicCompiler.Structures.Grammar;
using MagicCompiler.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MagicCompiler.Grammars
{
    /*
        El formato de las reglas debería tener la forma:
        (forma 1)
            SIMBOLO_NO_TERMINAL ::= SIMBOLO

        ó (forma 2)
            SIMBOLO_NO_TERMINAL ::= SIMBOLO_1 @@ SIMBOLO_2 @@ ... @@ SIMBOLO_N

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
    internal class LRReader
    {
        private string FILE_DIRECTION_GRAMMAR_RULES => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data/grammar_rules.txt");
        private string FILE_DIRECTION_GRAMMAR_CONFIGURATIONS => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data/grammar_config.json");

        private List<string> _rawRules = new List<string>();
        private dynamic _config;

        private const string RULE_LEFT_RIGHT_SEPARATOR = "::=";
        private const string INLINE_RULE_SEPARATOR = "@@";
        private const string COMMENT_SYMBOL = "#";

        public LRReader()
        {
            Reader reader = new Reader(FILE_DIRECTION_GRAMMAR_RULES);
            _rawRules = reader.ReadLines(t => t.Trim().ToLower());

            reader.FileDirection = FILE_DIRECTION_GRAMMAR_CONFIGURATIONS;
            string config = reader.ReadAll();
            _config = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(config);
        }

        public void Build(LRGrammar grammar)
        {
            grammar.Configuration = new CFGConfig() 
            { 
                AugmentedProduction = StringToRule((string)_config.AugmentedProduction)[0], 
                Epsilon = _config.EpsilonSymbol
            };

            for (int i = 0; i < _rawRules.Count; i++)
            {
                grammar.Productions.AddRange(StringToRule(_rawRules[i]));
            }
        }

        private List<Production> StringToRule(string rule)
        {
            rule = rule.ToLower();
            List<Production> result = new List<Production>();
            List<string> leftRight = new List<string>(rule.Split(RULE_LEFT_RIGHT_SEPARATOR));
            string left = leftRight[0].Trim(); // added trim because it was reading an extra space char

             List<string> right = new List<string>(leftRight[1].Split(INLINE_RULE_SEPARATOR));
            for (int j = 0; j < right.Count; j++)
            {
                Production duaLipaNewRule = new Production() { Left = left };
                duaLipaNewRule.Right.AddRange(right[j].Trim().Split(" "));
                result.Add(duaLipaNewRule);
            }
            return result;
        }
    }
}