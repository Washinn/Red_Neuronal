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

using System.IO;

namespace CasosDePrueba
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random Aleatorio;
        

        SpriteFont texto;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferWidth = 1000;
            this.graphics.PreferredBackBufferHeight = 800;
            Content.RootDirectory = "Content";
            Aleatorio = new Random(DateTime.Now.Millisecond);
        }


        protected override void Initialize()
        {
            

            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            generarCaso(500);
            generarCaso(1000);
            generarCaso(2000);

            texto = this.Content.Load<SpriteFont>("Imagenes/Fuente");

        }


        void generarCaso(int cantidad) {
            List<Vector2> casos = new List<Vector2>();
            string archivo = "Caso" + cantidad + ".txt";
            int mitad = (int) cantidad / 2;
            int zonaA = 0;
            int zonaB = 0;
            
            StreamWriter stream = System.IO.File.CreateText(archivo);
            
            int contador = 0;
            while (contador < cantidad) {
                
                Vector2 Tmp = new Vector2( (float)(Aleatorio.NextDouble() * 20.0), (float)(Aleatorio.NextDouble() * 20.0));


                if (!casos.Contains(Tmp))
                {
                    casos.Add(Tmp);
                    if (verificacion(Tmp.X, Tmp.Y))
                    {
                        if (zonaA < mitad)
                        {
                            stream.WriteLine(Tmp.X + " " + Tmp.Y + " " + (-1));
                            zonaA++;
                            contador++;
                        }
                    }
                    else
                    {
                        if (zonaB < mitad)
                        {
                            stream.WriteLine(Tmp.X + " " + Tmp.Y + " " + 1);
                            zonaB++;
                            contador++;
                        }
                    }
                }
            }
            
            stream.Close();
        }

        bool verificacion(double x, double y) {
            if (Math.Sqrt(Math.Pow(x - 10.0, 2.0) + Math.Pow(y - 10.0, 2.0)) <= 6)
            {
                return true;
            }
            return false;
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

            //spriteBatch.DrawString(texto, "Numero de Iteraciones: " + RedNeural[0].Max_Iteracion, new Vector2(600, 550), Color.White);
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
