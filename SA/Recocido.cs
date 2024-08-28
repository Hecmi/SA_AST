using SA_AST.Arbol_expresion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SA_AST.SA
{
    public class Recocido
    {
        //Variables del algoritmo SA
        double TEMPERATURA;
        double TASA_ENFRIAMIENTO;
        double TEMPERATURA_MINIMA;

        double [] MEJOR_SOLUCION_VALORES;
        double MEJOR_SOLUCION;
        

        //Variables para definir y calcular la función objetivo
        //basado en el árbol de sintaxis abstracto
        AST AST;

        int DIMENSION;


        public Recocido(double temperatura_inicial, double tasa_enfriamiento, double temperatura_minima, string funcion_objetivo)
        {
            this.TEMPERATURA = temperatura_inicial;
            this.TASA_ENFRIAMIENTO = tasa_enfriamiento;
            this.TEMPERATURA_MINIMA = temperatura_minima;

            this.AST = new AST(funcion_objetivo);
            this.DIMENSION = AST.INCOGNITAS.Count;

            this.MEJOR_SOLUCION_VALORES = new double[DIMENSION];
        }

        private double [] obtener_solucion_aleatoria()
        {
            double[] solucion_aleatoria = new double[DIMENSION];
            Random rand = new Random();

            for (int i = 0; i < DIMENSION; i++)
            {
                solucion_aleatoria[i] = rand.NextDouble() * 10;
            }

            return solucion_aleatoria;
        }

        private double evaluar_funcion_objetivo(double[] x)
        {
            //Colocar las incógnitas basado en el número de decisiones
            Dictionary<string, double> incognitas_evaluar = new Dictionary<string, double>();
            for (int i = 0; i < DIMENSION; i++)
            {
                incognitas_evaluar[AST.INCOGNITAS[i]] = x[i];
            }

            return AST.evaluar(incognitas_evaluar);
        }

        public void ejecutar()
        {
            Random rand = new Random();
            MEJOR_SOLUCION_VALORES = obtener_solucion_aleatoria();
            MEJOR_SOLUCION = evaluar_funcion_objetivo(MEJOR_SOLUCION_VALORES);

            double[] solucion_valores_seleccionada = new double[DIMENSION];
            double solucion_seleccionada = 0;


            //Variables para las soluciones obtenidas de forma iterativa
            double[] solucion_valor_actual = new double[DIMENSION];
            double ajuste_actual = 0;

            while (TEMPERATURA > TEMPERATURA_MINIMA)
            {
                //Obtener una solución aleatoria y calcular el ajuste con respecto
                //a la función objetivo
                solucion_valor_actual = obtener_solucion_aleatoria();
                ajuste_actual = evaluar_funcion_objetivo(solucion_valor_actual);
                
                //Calcular el valor de probabilidad para reconocer si se acepta
                //o no la solución actual
                double p = Math.Exp(-(solucion_seleccionada - ajuste_actual) / TEMPERATURA);
                if (rand.NextDouble() < p)
                {
                    //solucion_valores_seleccionada = solucion_valor_actual;
                    solucion_seleccionada = ajuste_actual;
                }

                //Verificar si el ajuste actual es mejor que el óptimo global para reemplazarlo
                if (ajuste_actual < MEJOR_SOLUCION)
                {
                    MEJOR_SOLUCION = ajuste_actual;
                    MEJOR_SOLUCION_VALORES = solucion_valor_actual;

                    Console.WriteLine(String.Join(", ", MEJOR_SOLUCION_VALORES));
                    Console.WriteLine(MEJOR_SOLUCION);
                }

                //Disminuir la temperatura basado en la tasa de enfriamiento definida
                TEMPERATURA = TEMPERATURA * TASA_ENFRIAMIENTO;
            }

        }
    }
}
