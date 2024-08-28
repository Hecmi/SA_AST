using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SA_AST.Arbol_expresion
{
    public class Nodo
    {
        public string Valor { get; set; }
        public Nodo Izquierda { get; set; }
        public Nodo Derecha { get; set; }

        public Nodo(string valor)
        {
            Valor = valor;
            Izquierda = null;
            Derecha = null;
        }
    }
}
