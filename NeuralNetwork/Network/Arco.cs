using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network
{
    public class Arco
    {
        public Nodo Ini; //Nodo Inicial
        public Nodo Fin; //Nodo Final
        public double W; //Peso

        public Arco(Nodo I, Nodo F, Random R)
        {
            Ini = I;
            Fin = F;
            W = R.NextDouble() / 1000.0;
            Console.WriteLine(W);
        }
    }
}
