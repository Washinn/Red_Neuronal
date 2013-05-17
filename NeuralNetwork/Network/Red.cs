using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network
{
    public class Red
    {
        public Dictionary<int,List<Nodo>> Layers;
        public Dictionary<Nodo, List<Arco>> Arcos;
        Random Aleatorio;

        public Red(int N_Layers, int NxL)
        {
            Layers = new Dictionary<int, List<Nodo>>();
            Arcos = new Dictionary<Nodo, List<Arco>>();
            Aleatorio = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < N_Layers; i++)
            {
                List<Nodo> tmp = new List<Nodo>();
                for (int j = 0; j < NxL; j++)
                {
                    Nodo TmpN = new Nodo();
                    if (i > 1)
                    {
                        List<Arco> TmpA = new List<Arco>();
                        foreach (Nodo I in Layers[i-1])
                            TmpA.Add(new Arco(I, TmpN, Aleatorio));
                        Arcos.Add(TmpN, TmpA);
                    }
                    tmp.Add(TmpN);
                }
                Layers.Add(i, tmp);
            }
        }

        public void AddNodeToLayer(int Layer,int N_Node){
            for (int j = 0; j < N_Node; j++)
            {
                Nodo TmpN = new Nodo();
                if (Layer > 0)
                {
                    List<Arco> TmpA = new List<Arco>();
                    foreach (Nodo I in Layers[Layer - 1])
                        TmpA.Add(new Arco(I, TmpN, Aleatorio));
                    Arcos.Add(TmpN, TmpA);
                }
                if(!Layers.ContainsKey(Layer))
                    Layers.Add(Layer,new List<Nodo>());
                
                Layers[Layer].Add(TmpN);
            }
        }

        public List<Nodo> GetLayer(int Layer)
        {
            return Layers[Layer];
        }

        public List<Arco> GetArcs(Nodo N)
        {
            return Arcos[N];
        }

        public List<Arco> GetArcsInv(Nodo N)
        {
            List<Arco> Tmp = new List<Arco>();

            foreach (Nodo I in Layers[GetLayer(N) + 1])
                foreach (Arco A in Arcos[I])
                    if (A.Ini.Equals(N))
                        Tmp.Add(A);

            return Tmp;

        }

        int GetLayer(Nodo N)
        {
            foreach (var Pair in Layers)
                if (Pair.Value.Contains(N))
                    return Pair.Key;
            return -1;
        }

        public int N_Layar()
        {
            return Layers.Count();
        }


    }
}
