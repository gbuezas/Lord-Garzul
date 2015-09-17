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
using Hunting_Lord_Garzul.Objetos;
using System.IO;
using Hunting_Lord_Garzul.Abstractos.Heroes;

namespace Hunting_Lord_Garzul
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        #region VARIABLES

        // Variables necesarias para dibujar por default
        SpriteBatch spriteBatch;
        GraphicsDeviceManager graphics;

        // Donde se va a alojar el mensaje de chequeo de status
        Vector2 ChkStatVar = new Vector2(50, 550);

        // Check de estado de juego
        Globales.EstadosJuego Estado_Check;

        #endregion

        #region METODOS

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);

            // Establezco la resolucion maxima adecuada para el dispositivo
            // Supuestamente con esta resolucion autoescala a menores
            // Hay que probarlo en algun lado
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            
            // No estoy seguro de si esto me da antialiasing o ya con lo que puse cuando dibujo alcanza
            graphics.PreferMultiSampling = true;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Agrego los personajes a la lista asi se pueden utilizar mas tarde
            for (int i = 0; i < Globales.playersQuant; i++)
            {
                Globales.players.Add(new Jugador_Paladin());
            }

            // Agrego los enemigos a la lista
            for (int i = 0; i < Globales.enemiesQuant; i++)
            {
                //Globales.players.Add(new IA_1((Globales.TargetCondition)azar.Next(0, 4)));
                Globales.players.Add(new IA_1());
            }

            Globales.CurrentState = new Estado_Avance();
            Globales.CurrentState.Estado_ejecutandose = Globales.EstadosJuego.TITULO;
            
            // Ponemos este estado por defecto en un modo que no es nada, asi cuando va al case detecta incongruencia
            // y acomoda al que corresponde, que seria el que dice arriba en ejecutandose.
            Estado_Check = Globales.EstadosJuego.GAMEOVER;

            Globales.CurrentState.Initialize();

            // Ralentizar los cuadros por segundo de todo el juego
            //this.TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / 5);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Globales.ViewportWidth = GraphicsDevice.Viewport.Width;
            Globales.ViewportHeight = GraphicsDevice.Viewport.Height;

            Globales.CurrentState.Load(GraphicsDevice.Viewport);

            // Cargo todas las texturas de los personas y sus movimientos de cada carpeta.
            // Acordarse que los png tienen que estar en la carpeta DEBUG para el modo DEBUG, y asi con cada modo.
            // Si no hay nada va al catch asi que no pasa nada
            # region TEXTURA_HEROES
            foreach (String heroe in Globales.Heroes)
            {
                try
                {
                    // Hago la lista solamente de los archivos PNG (animaciones de las piezas de los heroes) que estan en el content 
                    // para crear las texturas de cada uno.
                    String[] archivos_personajes = Directory.GetFiles("Content/" + heroe, "*.png");
                    
                    foreach (String nombre_personajes in archivos_personajes)
                    {
                        String Nombre = Path.GetFileNameWithoutExtension(nombre_personajes);
                        Texturas textura = new Texturas(Content.Load<Texture2D>(heroe + "/" + Nombre), Nombre);

                        switch (heroe)
                        {

                            case "Paladin":
                                {
                                    Globales.PaladinTextures.Add(textura);
                                    break;
                                }

                            case "IA_1":
                                {
                                    Globales.IA_1Textures.Add(textura);
                                    break;
                                }

                            default:
                                {
                                    break;
                                }

                        }
 
                        if (!Globales.Armors.Contains(Nombre.Split('_')[0]))
                        {
                            Globales.Armors.Add(Nombre.Split('_')[0]);
                        }
                    }
                }
                catch
                { 
                    // Para que aunque no encuentre carpeta ni nada siga igual, 
                    // porque no hace diferencia al juego que no pueda encontrar nada.
                    // Mas tarde si va a importar que no cargue todas las texturas de todos cuando las tengamos.
                    // Pero ahora en esta etapa de prueba donde me faltan un monton de cosas no es necesario.
                }
            }
            #endregion

            // Cargo los niveles
            #region TEXTURA_NIVELES

            foreach (String escenario in Globales.Scenes)
            {
                try
                {
                    String[] archivos_niveles = Directory.GetFiles("Content/" + escenario, "*.png");

                    foreach (String nombre_niveles in archivos_niveles)
                    {
                        String Nombre = Path.GetFileNameWithoutExtension(nombre_niveles);
                        Texturas textura = new Texturas(Content.Load<Texture2D>(escenario + "/" + Nombre), Nombre);

                        switch (escenario)
                        {

                            case "Avance":
                                {
                                    Globales.AvanceTextures.Add(textura);
                                    break;
                                }

                            case "Versus":
                                {
                                    Globales.VersusTextures.Add(textura);
                                    break;
                                }

                            default:
                                {
                                    break;
                                }

                        }

                    }
                }
                catch
                {
                    // Para que aunque no encuentre carpeta ni nada siga igual, 
                    // porque no hace diferencia al juego que no pueda encontrar nada.
                    // Mas tarde si va a importar que no cargue todas las texturas de todos cuando las tengamos.
                    // Pero ahora en esta etapa de prueba donde me faltan un monton de cosas no es necesario.
                }
            }
            
            #endregion

            // Cargo punto blanco
            Globales.Punto_Blanco = Content.Load<Texture2D>("Seleccion/puntoblanco");

            // Cargo titulos y pantallas de presentacion
            Globales.Pantalla_Titulo = Content.Load<Texture2D>("Titulo/TitleScreen");
            
            // Cargo pantalla de seleccion y selectores
            Globales.Pantalla_Seleccion = Content.Load<Texture2D>("Seleccion/fondo");
            //Variables_Generales.Selector = Content.Load<Texture2D>("Seleccion/Selector");
            
            // Cargo fuentes
            Globales.CheckStatusVar = Content.Load<SpriteFont>("Fuente_Prueba");
            Globales.CheckStatusVar_2 = Content.Load<SpriteFont>("Fuente_Prueba_2");

            // Asigno posiciones iniciales de los personajes, tanto jugadores como IA
            int ejeX = 0;
            int ejeY = 0;
            foreach (Jugadores Jugador in Globales.players)
            {


                Jugador.Initialize(new Vector2( GraphicsDevice.Viewport.TitleSafeArea.X + ejeX,
                                                GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2 + ejeY));
                
                ejeX = ejeX + 50;
                ejeY = ejeY + 50;
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Funciones del juego
            QuitGame();
            GiveLife();
            EnableColRec();

            // Acomoda los estados correspondientes del jeugo
            StateSwitch();

            Globales.CurrentState.Update(gameTime);

            base.Update(gameTime);

            Globales.elapsedTime += gameTime.ElapsedGameTime;

            if (Globales.elapsedTime > TimeSpan.FromSeconds(1))
            {
                Globales.elapsedTime -= TimeSpan.FromSeconds(1);
                Globales.frameRate = Globales.frameCounter;
                Globales.frameCounter = 0;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Globales.frameCounter++;

            GraphicsDevice.Clear(Color.White);

            // Dibuja el estado actual
            Globales.CurrentState.Draw(spriteBatch);

            # region MENSAJES DE ERROR
            spriteBatch.Begin();

            spriteBatch.DrawString(Globales.CheckStatusVar,
            "altoViewport = " + Globales.mensaje1.ToString() + System.Environment.NewLine +
            "anchoViewport = " + Globales.mensaje2.ToString() + System.Environment.NewLine +
            "limitePantallaX = " + Globales.mensaje3.ToString() + System.Environment.NewLine +
            "limitePantallaAncho = " + Globales.mensaje4.ToString() + System.Environment.NewLine + 
            "Zoom = " + Globales.mensaje5.ToString() + System.Environment.NewLine + 
            "FPS = " + Globales.frameRate + System.Environment.NewLine,
            ChkStatVar, Color.DarkRed);

            spriteBatch.End();
            #endregion

            base.Draw(gameTime);
        }

        /// <summary>
        /// Añade vida a todos los jugadores
        /// </summary>
        private static void GiveLife()
        {
            // Da vida a los jugadores
            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                foreach (Jugadores jugador in Globales.players)
                {
                    jugador.health += 1;
                }
            }
        }

        /// <summary>
        /// Permite salir del juego desde el joystick o teclado apretando select o escape.
        /// </summary>
        private void QuitGame()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
        }

        /// <summary>
        /// Habilita rectangulo de colisiones.
        /// </summary>
        private static void EnableColRec()
        {
            Globales.previousKeyboardState = Globales.currentKeyboardState;
            Globales.currentKeyboardState = Keyboard.GetState();

            // Acciones que no se tienen que repetir al mantener la tecla
            if (Globales.previousKeyboardState.IsKeyDown(Keys.D2) && Globales.currentKeyboardState.IsKeyUp(Keys.D2))
            {
                if (Globales.EnableRectangles)
                {
                    Globales.EnableRectangles = false;
                }
                else
                {
                    Globales.EnableRectangles = true;
                }
            }
        }

        /// <summary>
        /// Chequea en que estado tiene que estar el juego y lo gestiona.
        /// </summary>
        private void StateSwitch()
        {
            
            if (Estado_Check != Globales.CurrentState.Estado_ejecutandose)
            {
                switch (Globales.CurrentState.Estado_ejecutandose)
                {

                    case Globales.EstadosJuego.TITULO:
                        {
                            Estado_Check = Globales.EstadosJuego.TITULO;
                            Globales.CurrentState = new Estado_Titulos();
                            break;
                        }

                    case Globales.EstadosJuego.SELECCION:
                        {
                            Estado_Check = Globales.EstadosJuego.SELECCION;
                            Globales.CurrentState = new Estado_Seleccion();
                            break;
                        }

                    case Globales.EstadosJuego.AVANCE:
                        {
                            Estado_Check = Globales.EstadosJuego.AVANCE;
                            Globales.CurrentState = new Estado_Avance();
                            break;
                        }

                    default: break;

                }
            }
        }

        #endregion
    }
}
