using SA_AST.SA;
using System;
using System.Collections.Generic;

namespace SA_AST
{
    class Program
    {
        static void Main(string[] args)
        {
            //string ECUACION = "(10-x)^2+100*(y-x^2)^2";
            //string ECUACION = "-2*x^2+42*x+3";
            //string ECUACION = "x^(3)-12 * x^(2)+37 * x-25,5";
            string ECUACION = "-(-(-x+1)*(x-8,5)*(x-3))";

            Arbol_expresion.AST ast = new Arbol_expresion.AST(ECUACION);
            Dictionary<string, double> valores_incognitas = new Dictionary<string, double>();
            valores_incognitas["x"] = 2;
            ast.mostrar_arbol();
            Console.WriteLine(ast.evaluar(valores_incognitas));

            //Recocido sa = new Recocido(
            //    999999999,
            //    0.99,
            //    1,
            //    ECUACION,
            //    1,
            //    0,
            //    100
            //);

            //sa.ejecutar();
            //sa.get_mejor_solucion();
        }
    }
}
