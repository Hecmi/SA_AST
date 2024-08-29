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

        //Otras variables para definir la aceptación de una raíz
        List<double[]> RAICES_ENCONTRADAS;
        List<double> EVALUACION_RAICES_ENCONTRADAS;

        double UMBRAL;
        double DOM_MIN;
        double DOM_MAX;

        public Recocido(double temperatura_inicial, double tasa_enfriamiento, double temperatura_minima, string funcion_objetivo, double umbral, double dom_min, double dom_max)
        {
            this.TEMPERATURA = temperatura_inicial;
            this.TASA_ENFRIAMIENTO = tasa_enfriamiento;
            this.TEMPERATURA_MINIMA = temperatura_minima;
            
            this.UMBRAL = umbral;
            this.DOM_MIN = dom_min;
            this.DOM_MAX = dom_max;

            this.AST = new AST(funcion_objetivo);
            this.DIMENSION = AST.INCOGNITAS.Count;

            this.MEJOR_SOLUCION_VALORES = new double[DIMENSION];
            this.RAICES_ENCONTRADAS = new List<double[]>();
            this.EVALUACION_RAICES_ENCONTRADAS = new List<double>();
        }

        private double [] obtener_solucion_aleatoria()
        {
            double[] solucion_aleatoria = new double[DIMENSION];
            Random rand = new Random();

            for (int i = 0; i < DIMENSION; i++)
            {
                //Iniciar con valores entre -50 y 50
                //solucion_aleatoria[i] = rand.NextDouble() * 100 - 50;

                solucion_aleatoria[i] = DOM_MIN + rand.NextDouble() * (DOM_MAX - DOM_MIN);
            }

            return solucion_aleatoria;
        }

        private double evaluar_funcion_objetivo(double[] incognitas)
        {
            //Colocar las incógnitas basado en el número de decisiones
            Dictionary<string, double> incognitas_evaluar = new Dictionary<string, double>();
            for (int i = 0; i < DIMENSION; i++)
            {
                incognitas_evaluar[AST.INCOGNITAS[i]] = incognitas[i];
            }

            return AST.evaluar(incognitas_evaluar);
        }

        private bool es_raiz(double valor)
        {
            return valor < UMBRAL;
        }

        private bool es_raiz_encontrada(double[] solucion, double ajuste)
        {
            for (int i = 0; i < RAICES_ENCONTRADAS.Count; i++)
            {
                double[] raiz = RAICES_ENCONTRADAS[i];          
                double distancia = 0;

                //Calcular la distancia euclidiana para verificar
                //si no es una raíz repetida
                for (int j = 0; j < DIMENSION; j++)
                {
                    distancia += Math.Pow(solucion[j] - raiz[j], 2);
                }

                //Sí la distancia euclidiana es menor que el umbral
                //significa que es una solución cercana a la ya encontrada
                if (Math.Sqrt(distancia) < UMBRAL)
                {
                    //Sin embargo, se verifica si el ajuste de la nueva solución
                    //es mejor a la ya determinada
                    if (ajuste < EVALUACION_RAICES_ENCONTRADAS[i])
                    {
                        RAICES_ENCONTRADAS[i] = solucion;
                    }

                    return true;
                }

            }
            return false;
        }

        public void ejecutar()
        {
            Random rand = new Random();
            MEJOR_SOLUCION_VALORES = obtener_solucion_aleatoria();
            MEJOR_SOLUCION = Math.Abs(evaluar_funcion_objetivo(MEJOR_SOLUCION_VALORES));

            double solucion_seleccionada = MEJOR_SOLUCION;

            //Variables para las soluciones obtenidas de forma iterativa
            double[] solucion_valor_actual = new double[DIMENSION];
            double ajuste_actual = 0;

            while (TEMPERATURA > TEMPERATURA_MINIMA)
            {
                //Obtener una solución aleatoria y calcular el ajuste con respecto
                //a la función objetivo
                solucion_valor_actual = obtener_solucion_aleatoria();
                ajuste_actual = Math.Abs(evaluar_funcion_objetivo(solucion_valor_actual));
                
                //Calcular el valor de probabilidad para reconocer si se acepta
                //o no la solución actual
                double p = Math.Exp(-(solucion_seleccionada - ajuste_actual) / TEMPERATURA);
                if (rand.NextDouble() < p)
                {
                    solucion_seleccionada = ajuste_actual;
                }

                //Verificar si el ajuste actual es mejor que el óptimo global para reemplazarlo
                if (ajuste_actual < MEJOR_SOLUCION)
                {
                    MEJOR_SOLUCION = ajuste_actual;
                    MEJOR_SOLUCION_VALORES = solucion_valor_actual;

                    //Console.WriteLine(String.Join(", ", MEJOR_SOLUCION_VALORES));
                    //Console.WriteLine(MEJOR_SOLUCION);
                }

                if (es_raiz(ajuste_actual) && !es_raiz_encontrada(solucion_valor_actual, ajuste_actual))
                {
                    RAICES_ENCONTRADAS.Add((double[])solucion_valor_actual.Clone());
                    EVALUACION_RAICES_ENCONTRADAS.Add(ajuste_actual);
                }

                //Disminuir la temperatura basado en la tasa de enfriamiento definida
                TEMPERATURA = TEMPERATURA * TASA_ENFRIAMIENTO;
            }
        }

        public Dictionary<string, double> get_mejor_solucion()
        {
            //Dictionary<string, double> mejor_solucion = new Dictionary<string, double>();
            //for (int i = 0; i < AST.INCOGNITAS.Count; i++)
            //{
            //    mejor_solucion[AST.INCOGNITAS[i]] = MEJOR_SOLUCION_VALORES[i];
            //}

            for(int i = 0; i < RAICES_ENCONTRADAS.Count; i++)
            {
                Dictionary<string, double> mejor_solucion = new Dictionary<string, double>();
                double[] raiz = RAICES_ENCONTRADAS[i];

                for(int j = 0; j < DIMENSION; j++)
                {
                    mejor_solucion[AST.INCOGNITAS[j]] = raiz[j];
                }

                Console.WriteLine(string.Join(", ", mejor_solucion));
                Console.WriteLine($"f = {evaluar_funcion_objetivo(raiz)}");
            }

            return new Dictionary<string, double>();
        }
    }
}
