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

namespace Pruebas
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Funciones[] RedNeural;
        Dictionary<double[], int>[] Test;

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
            RedNeural = new Funciones[9];
            
            for (int i = 0; i < RedNeural.Length; ++i)
                RedNeural[i] = new Funciones(3,i+2,1);
            



            base.Initialize();
        }

        RenderTarget2D screenshot;
        string File;

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Test = new Dictionary<double[], int>[RedNeural.Length+1];

            File = "Caso1000.txt";
            string[] A = new string[2];
            A[0] = "Main";
            A[1] = "Content/Grupo_Entrenamiento/"+File;

            

            for (int i = 0; i < RedNeural.Length; ++i)
            {
                Console.WriteLine("Entrenando Red con "+(i+2)+"Neuronas");
                RedNeural[i].Training2(RedNeural[i].Entrada(A));
            }
            

            Test[0] = Validacion();
            for (int i = 0; i < RedNeural.Length; ++i)
                Test[i+1] = RedNeural[i].Testing(Test[0]);

            texto = this.Content.Load<SpriteFont>("Imagenes/Fuente");
            screenshot = new RenderTarget2D(GraphicsDevice,1000,800);
        }


        protected override void UnloadContent()
        {
            screenshot.Dispose();
        }

        int Verif_Circ(double x, double y)
        {
            if (Math.Sqrt(Math.Pow(x - 10.0, 2.0) + Math.Pow(y - 10.0, 2.0)) <= 6)
                return 0;
            else
                return 1;
        }

        Dictionary<double[], int> Validacion()
        {
            Dictionary<double[], int> Test = new Dictionary<double[], int>();

            double dist = 20.0/100.0;
            for (int i = 0; i < 100; i++)
                for (int j = 0; j < 100; j++)
                {
                    double x = dist * i;
                    double y = dist * j;
                    double[] Tmp = { y, x };
                    Test.Add(Tmp, Verif_Circ(y,x));
                }
            return Test;

        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                ScreenShot();
                this.Exit();
            }

            base.Update(gameTime);
        }


        public void ScreenShot()
        {

            int w = GraphicsDevice.PresentationParameters.BackBufferWidth;
            int h = GraphicsDevice.PresentationParameters.BackBufferHeight;

            //force a frame to be drawn (otherwise back buffer is empty)
            Draw(new GameTime());

            //pull the picture from the buffer
            int[] backBuffer = new int[w * h];
            GraphicsDevice.GetBackBufferData(backBuffer);

            //copy into a texture
            Texture2D texture = new Texture2D(GraphicsDevice, w, h, false, GraphicsDevice.PresentationParameters.BackBufferFormat);
            texture.SetData(backBuffer);

            //save to disk
            Stream stream = System.IO.File.OpenWrite("screenshot_" + File + ".png");
            texture.SaveAsPng(stream, w, h);
            stream.Close();

        }
        
        protected override void Draw(GameTime gameTime)
        {

            int H = 2;
            int W = 2;

            Texture2D Textura = new Texture2D(graphics.GraphicsDevice, H, W);
            Color[] dataColor = new Color[W*H];
            for (int i = 0; i < W*H;++i )
                dataColor[i] = Color.White;
            Textura.SetData(dataColor);


            Vector2 posicionTexto = new Vector2(250, 10);



            spriteBatch.Begin();

            GraphicsDevice.Clear(Color.Black);

            spriteBatch.DrawString(texto, "Conjunto de entrenamiento: " + File, posicionTexto, Color.White);
            for (int i = 0; i < Test.Length; ++i)
            {
                int x = i % 4;
                int y = i / 4;
                if(i > 0)
                    spriteBatch.DrawString(texto, i+1 +" neuronas", new Vector2(x*250 + 70 ,y*250 + 50), Color.White);
                else
                    spriteBatch.DrawString(texto, "Objetivo", new Vector2(x * 250 + 70, y * 250 + 50), Color.White);
                foreach (var T in Test[i])
                    if (T.Value == 1)
                        spriteBatch.Draw(Textura, new Vector2(20 + (float)T.Key[0]*10 + 250 * x, 80 + (float)T.Key[1]*10 + 250 * y), Color.Blue);
                    else
                        spriteBatch.Draw(Textura, new Vector2(20 + (float)T.Key[0]*10 + 250 * x, 80 + (float)T.Key[1]*10 + 250 * y), Color.Green);

            }

            spriteBatch.DrawString(texto, "Numero de Iteraciones: "+RedNeural[0].Max_Iteracion, new Vector2 ( 600,750 ), Color.White);
            
            spriteBatch.End();
            

            base.Draw(gameTime);
        }
    }
}
