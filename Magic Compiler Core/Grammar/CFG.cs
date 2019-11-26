using System;
using System.Collections.Generic;
using System.Text;

namespace MagicCompiler.Grammar
{
    /*  la 4-tupla de G=(Vt, Vn, P, S) donde:
        Vt = Conjunto finito de terminales
        Vn = Conjunto fnito de no terminales
        P = Conjunto finito de producciones
        S ∈ Vn = Simbolo inicial
    */
    public class CFG
    {
        public CFGConfig Configuration;
        public List<Rule> Productions = new List<Rule>();
    }
}
