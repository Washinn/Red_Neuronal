using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using NeuralNetwork;
using System.IO;

namespace LDDS
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Funciones[] RedNeural;
        Dictionary<double[], int>[] Test;

        int total;
        int trai;
        int tes;


        SpriteFont texto;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferWidth = 1000;
            this.graphics.PreferredBackBufferHeight = 800;
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            RedNeural = new Funciones[3];
            for (int i = 0; i < RedNeural.Length; ++i)
                RedNeural[i] = new Funciones(7,2 + i,1);

            base.Initialize();
        }

        string File;

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Test = new Dictionary<double[], int>[RedNeural.Length + 1];

            File = "datos.txt";
            string[] A = new string[2];
            A[0] = "Main";
            A[1] = "Content/Grupo/" + File;

            Dictionary<double[], int> datos = RedNeural[0].Entrada2(A);
            total = datos.Count();
            Dictionary<double[], int> test = obtenerDatosTest(datos, 0.7f);

            trai = datos.Count();
            tes = test.Count();

            for (int i = 0; i < RedNeural.Length; ++i)
            {
                Console.WriteLine("Entrenando Red con " + (i + 2) + " Neuronas");
                RedNeural[i].Training(datos);
            }

            Test[0] = test;
            for (int i = 0; i < RedNeural.Length; ++i)
                Test[i + 1] = RedNeural[i].Testing(Test[0], i);
            
            texto = this.Content.Load<SpriteFont>("Imagenes/Fuente");

        }

        Dictionary<double[], int> obtenerDatosTest(Dictionary<double[], int> datos, float proporcion) { 
            Dictionary<double[], int> aux = new Dictionary<double[], int>();
            int cantidadDatosNecesarios = datos.Count() - ((int) (datos.Count()*proporcion));
            Random r = new Random();
            while (aux.Count() < cantidadDatosNecesarios)
            {
                if (r.NextDouble() < 0.5f)
                {
                    KeyValuePair<double[], int> a = datos.ElementAt(r.Next() % datos.Count());
                    aux.Add(a.Key, a.Value);
                    datos.Remove(a.Key);
                }
            }
            return aux;
        }

        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin();

            GraphicsDevice.Clear(Color.Black);

            spriteBatch.DrawString(texto, "Numero de Iteraciones: " + RedNeural[0].Max_Iteracion, new Vector2(600, 550), Color.White);
            spriteBatch.DrawString(texto, "Total " + total, new Vector2(600, 600), Color.White);
            spriteBatch.DrawString(texto, "Training " + trai, new Vector2(600, 650), Color.White);
            spriteBatch.DrawString(texto, "Test " + tes, new Vector2(600, 700), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
