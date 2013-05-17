using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Network;

namespace NeuralNetwork
{
    public class Program
    {
        

        public static void Main(string[] args)
        {


            Funciones Func = new Funciones(3,2,1);

            //Dictionary<double[], int> Examples = Func.Entrada(args);

            Dictionary<double[], int> Examples = new Dictionary<double[], int>();
            double[] ExA = { 1, 1 };
            Examples.Add(ExA, 1);
            double[] ExB = { 1, 0 };
            Examples.Add(ExB, 1);
            double[] ExC = { 0, 1 };
            Examples.Add(ExC, 1);
            double[] ExD = { 0, 0 };
            Examples.Add(ExD, 0);
            

            Func.Training(Examples);

            // Pruebas
            
            Dictionary<double[], int> Test = new Dictionary<double[], int>();
            double[]tempA = {  1 , 1 };
            Test.Add(tempA,1);
            double[]tempB = {  1 ,0 };
            Test.Add(tempB,0);  
            double[]tempC = { 0 , 1 };
            Test.Add(tempC,0);  
            double[]tempD = { 0 ,0 };
            Test.Add(tempD,0);

            foreach (var E in Func.Testing(Test))
                Console.WriteLine(E.Key[0] + " " + E.Key[1] + " " +E.Value);

            Console.ReadLine();

        }
    }
}
