﻿using SA_AST.SA;
using System;
using System.Collections.Generic;

namespace SA_AST
{
    class Program
    {
        public static void Main()
        {
            string ECUACION = "(10-x)^2+100*(y-x^2)^2";

            Recocido sa = new Recocido(
                99999999999,
                0.99,
                5,
                ECUACION
            );

            sa.ejecutar();
            sa.get_mejor_solucion();
        }
    }
}
