using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Network;

namespace NeuralNetwork
{
    public class Funciones
    {
        public int Max_Iteracion;
        double Ta; // Tasa de Aprendizaje
        Red NN;
        int N_Neuronas;
        int Neuronas_Ocultas;

        public Funciones(int NeuronasEntrada,int NeuronasOcultas,int NeuronasSalida)
        {
            Ta = 0.1;
            Max_Iteracion = 50000;
            Neuronas_Ocultas = NeuronasOcultas;
            //Creamos la Red Neural
            NN = new Red(1, NeuronasEntrada);// Entrada
            NN.AddNodeToLayer(1, NeuronasOcultas); //Unidad Oculta
            NN.AddNodeToLayer(2, NeuronasSalida); // Salida

        }

        #region sigmoidal
        //Funcion de Activacion Sigmoidal
        public double g(double x)
        {
            double Numerador = 1;
            double Denominador = 1 + Math.Pow(Math.E, -x);

            return (Numerador / Denominador);
        }

        //Derivada de la funcion de activacion Sigmoidal
        public double dg(double x)
        {
            return ((g(x) * (1 - g(x))));
        }

        #endregion

        #region Entrada
        public Dictionary<double[], int> Entrada(string[] args)
        {
            Dictionary<double[], int> Examples = new Dictionary<double[], int>();
            N_Neuronas = 0;

            StreamReader objReader = new StreamReader(args[1]);
            string sLine = "";
            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                if (sLine != null)
                {
                    string[] tmp = sLine.Split(' ');
                    N_Neuronas = tmp.Length;

                    double[] Tmp2 = new double[N_Neuronas - 1];

                    for (int i = 0; i < N_Neuronas - 1; i++)
                        Tmp2[i] = double.Parse(tmp[i], System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);

                    Examples.Add(Tmp2, int.Parse(tmp[N_Neuronas - 1]));
                }
            }
            objReader.Close();

            return Examples;
        }

        public Dictionary<double[], int> Entrada2(string[] args)
        {
            Dictionary<double[], int> Examples = new Dictionary<double[], int>();
            N_Neuronas = 0;

            StreamReader objReader = new StreamReader(args[1]);
            string sLine = "";
            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                if (sLine != null)
                {
                    string[] tmp = sLine.Split(',');
                    N_Neuronas = tmp.Length;

                    double[] Tmp2 = new double[N_Neuronas - 1];

                    for (int i = 0; i < N_Neuronas - 1; i++)
                        Tmp2[i] = double.Parse(tmp[i], System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);

                    if (int.Parse(tmp[N_Neuronas - 1]) == 1)
                        Examples.Add(Tmp2, 1);
                    else
                        Examples.Add(Tmp2, -1);
                }
            }
            objReader.Close();

            return Examples;
        }

        #endregion

        #region Training

        public void Training(Dictionary<double[], int> Examples)
        {
            double Error;
            int Iteraciones = 0;

            StreamWriter stream = System.IO.File.CreateText("Log_Training_"+Neuronas_Ocultas+"_"+Max_Iteracion+".txt");

            do
            {
                Iteraciones++;
                Error = 0;
                foreach (var E in Examples)
                {

                    // ForwardPropagate
                    List<Nodo> Aux = NN.GetLayer(0); //Layer de Entrada
                    for (int j = 1; j < Aux.Count; j++)
                        Aux[j].a = E.Key[j - 1];
                    Aux[0].a = 1;

                    for (int j = 1; j < NN.N_Layar(); j++)
                        foreach (Nodo i in NN.GetLayer(j))
                        {
                            double Sum = 0;
                            foreach (Arco A in NN.GetArcs(i))
                                Sum += A.W * A.Ini.a;
                            i.In = Sum;
                            i.a = g(i.In);
                        }
    
                    foreach (Nodo i in NN.GetLayer(NN.N_Layar() - 1))
                    {

                        Error += (E.Value - i.a) * (E.Value - i.a);
                        i.delta = dg(i.In) * (E.Value - i.a);

                    }


                    //BackPropagate
                    for (int l = NN.N_Layar() - 2; l >= 0; l--)
                        foreach (Nodo j in NN.GetLayer(l))
                        {
                            double Sum = 0;
                            foreach (Arco A in NN.GetArcsInv(j))
                                Sum += A.W * A.Fin.delta;
                            j.delta = dg(j.In) * Sum;

                            foreach (Arco A in NN.GetArcsInv(j))
                                A.W += Ta * j.a * A.Fin.delta;
                        }

                }
                Error /= Examples.Count();
                Error = Math.Sqrt(Error);
                
                stream.WriteLine(Error);
            } while (Error > 0.84 && Iteraciones < Max_Iteracion);

            stream.Close();
        }

        public void Training2(Dictionary<double[], int> Examples)
        {
            double Error= 5;
            int Iteraciones = 0;
            StreamWriter stream = System.IO.File.CreateText("Log_Training_" + Neuronas_Ocultas + "_" + Max_Iteracion + ".txt");

            while (Error > 0.3 && Iteraciones < Max_Iteracion)
            {
                Error = 0;
                Iteraciones++;
                foreach (var E in Examples)
                {

                    // Calcular la salida de la red (a)
                    List<Nodo> Aux = NN.GetLayer(0); //Layer de Entrada
                    for (int j = 1; j < Aux.Count; j++)
                        Aux[j].a = E.Key[j - 1];
                    Aux[0].a = 1;

                    for (int j = 1; j < NN.N_Layar(); j++)
                        foreach (Nodo i in NN.GetLayer(j))
                        {
                            double Sum = 0;
                            foreach (Arco A in NN.GetArcs(i))
                                Sum += A.W * A.Ini.a;
                            i.In = Sum;
                            i.a = g(i.In);
                        }


                    //Para cada unidad de Salida k
                    foreach (Nodo k in NN.GetLayer(NN.N_Layar() - 1))
                    {
                        Error += (E.Value - k.a) * (E.Value - k.a);
                        k.delta = k.a * (1 - k.a) * (E.Value - k.a);
                    }

                    // Por cada Unidad Oculta h
                    for (int l = 1 ; l < NN.N_Layar() - 1; l++)
                        foreach (Nodo h in NN.GetLayer(l)) 
                        {
                            double Sum = 0;
                            foreach (Arco A in NN.GetArcsInv(h))
                                Sum += A.W * A.Fin.delta;
                            h.delta = h.a * ( 1 - h.a ) * Sum;
                        }


                    //Actualizar cada peso de la red (W)
                    for (int j = 1; j < NN.N_Layar(); j++)
                        foreach (Nodo i in NN.GetLayer(j))
                            foreach (Arco A in NN.GetArcs(i))
                                A.W += Ta * A.Fin.delta * A.Ini.a;
                }
                Error /= Examples.Count();
                Error = Math.Sqrt(Error);
                stream.WriteLine(Error);
            }
            stream.Close();
        }

        #endregion

        #region Testing
        public Dictionary<double[], int> Testing(Dictionary<double[], int> Test)
        {

            Dictionary<double[], int> Exit = new Dictionary<double[], int>(Test);

            foreach (var E in Test)
            {
                List<Nodo> Aux = NN.GetLayer(0); //Layer de Entrada
                for (int j = 1; j < Aux.Count; j++)
                    Aux[j].a = E.Key[j - 1];
                Aux[0].a = 1;
                //Calculamos la salida de la red para cada E
                for (int j = 1; j < NN.N_Layar(); j++)
                    foreach (Nodo i in NN.GetLayer(j))
                    {
                        double Sum = 0;
                        foreach (Arco A in NN.GetArcs(i))
                            Sum += A.W * A.Ini.a;
                        i.In = Sum;
                        i.a = g(i.In);
                    }

                foreach (Nodo i in NN.GetLayer(NN.N_Layar() - 1))
                    if(i.a>0.5)
                        if(i.a >0.75)
                            Exit[E.Key] = 1;
                        else
                            Exit[E.Key] = 0;
                    else
                        if(i.a >0.25)
                            Exit[E.Key] = 1;
                        else
                            Exit[E.Key] = 0;
            }

            return Exit;

        }

        public Dictionary<double[], int> Testing(Dictionary<double[], int> Test, int num)
        {
            string archivo = "Error" + num + ".txt";
            StreamWriter stream = System.IO.File.CreateText(archivo);
            int contador = 1;

            Dictionary<double[], int> Exit = new Dictionary<double[], int>(Test);

            foreach (var E in Test)
            {
                List<Nodo> Aux = NN.GetLayer(0); //Layer de Entrada
                for (int j = 1; j < Aux.Count; j++)
                    Aux[j].a = E.Key[j - 1];
                Aux[0].a = 1;
                //Calculamos la salida de la red para cada E
                for (int j = 1; j < NN.N_Layar(); j++)
                    foreach (Nodo i in NN.GetLayer(j))
                    {
                        double Sum = 0;
                        foreach (Arco A in NN.GetArcs(i))
                            Sum += A.W * A.Ini.a;
                        i.In = Sum;
                        i.a = g(i.In);
                    }

                foreach (Nodo i in NN.GetLayer(NN.N_Layar() - 1))
                    if (i.a > 0.5)
                        if (i.a > 0.75)
                            Exit[E.Key] = 1;
                        else
                            Exit[E.Key] = -1;
                    else
                        if (i.a > 0.25)
                            Exit[E.Key] = 1;
                        else
                            Exit[E.Key] = -1;

                //stream.WriteLine(contador + " " + (E.Value - Exit[E.Key]));
                stream.WriteLine((E.Value - Exit[E.Key]));
                contador++;
            }
            stream.Close();
            return Exit;

        }
        #endregion


    }
}
