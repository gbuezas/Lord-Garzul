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

namespace Hunting_Lord_Garzul
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        // Variables necesarias para dibujar por default
        SpriteBatch spriteBatch;
        GraphicsDeviceManager graphics;

        // Donde se va a alojar el mensaje de chequeo de status
        Vector2 ChkStatVar = new Vector2(50, 50);

        // Check de estado de juego
        Variables_Generales.EstadosJuego Estado_Check;

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
            for (int i = 0; i < 2; i++)
            {
                Variables_Generales.players.Add(new Jugador_Paladin());
            }

            Variables_Generales.Estado_Actual = new Estado_Avance();
            Variables_Generales.Estado_Actual.Estado_ejecutandose = Variables_Generales.EstadosJuego.TITULO;
            
            // Ponemos este estado por defecto en un modo que no es nada, asi cuando va al case detecta incongruencia
            // y acomoda al que corresponde, que seria el que dice arriba en ejecutandose.
            Estado_Check = Variables_Generales.EstadosJuego.GAMEOVER;

            Variables_Generales.Estado_Actual.Initialize();

            // Ralentizar los cuadros por segundo
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

            Variables_Generales.AnchoViewport = GraphicsDevice.Viewport.Width;
            Variables_Generales.AltoViewport = GraphicsDevice.Viewport.Height;

            Variables_Generales.Estado_Actual.Load(GraphicsDevice.Viewport);

            // Cargo todas las texturas de los personas y sus movimientos de cada carpeta.
            // Acordarse que los png tienen que estar en la carpeta DEBUG para el modo DEBUG, y asi con cada modo.
            // Si no hay nada va al catch asi que no pasa nada
            # region TEXTURA_HEROES
            foreach (String heroe in Variables_Generales.Heroes)
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
                                    Variables_Generales.TexturasPaladin.Add(textura);
                                    break;
                                }
                            
                            default:
                                {
                                    break;
                                }

                        }
 
                        if (!Variables_Generales.Armaduras.Contains(Nombre.Split('_')[0]))
                        {
                            Variables_Generales.Armaduras.Add(Nombre.Split('_')[0]);
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

            foreach (String escenario in Variables_Generales.Escenarios)
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
                                    Variables_Generales.TexturasAvance.Add(textura);
                                    break;
                                }

                            case "Versus":
                                {
                                    Variables_Generales.TexturasVersus.Add(textura);
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

            // Cargo titulos y pantallas de presentacion
            Variables_Generales.Pantalla_Titulo = Content.Load<Texture2D>("Titulo/TitleScreen");
            
            // Cargo pantalla de seleccion y selectores
            Variables_Generales.Pantalla_Seleccion = Content.Load<Texture2D>("Seleccion/fondo");
            //Variables_Generales.Selector = Content.Load<Texture2D>("Seleccion/Selector");
            
            // Cargo fuentes
            Variables_Generales.CheckStatusVar = Content.Load<SpriteFont>("Fuente_Prueba");
            Variables_Generales.CheckStatusVar_2 = Content.Load<SpriteFont>("Fuente_Prueba_2");

            // Asigno posiciones iniciales de los jugadores
            Variables_Generales.players[0].Initialize(new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, 
                GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2));

            Variables_Generales.players[1].Initialize(new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X,
                GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2 + 50));

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
            // Permite salir del juego desde el joystick o teclado
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // Chequea en que estado tiene que estar
            if(Estado_Check != Variables_Generales.Estado_Actual.Estado_ejecutandose)
            {
                switch (Variables_Generales.Estado_Actual.Estado_ejecutandose)
                {
                    
                    case Variables_Generales.EstadosJuego.TITULO:
                        {
                            Estado_Check = Variables_Generales.EstadosJuego.TITULO;
                            Variables_Generales.Estado_Actual = new Estado_Titulos();
                            break;
                        }

                    case Variables_Generales.EstadosJuego.SELECCION:
                        {
                            Estado_Check = Variables_Generales.EstadosJuego.SELECCION;
                            Variables_Generales.Estado_Actual = new Estado_Seleccion();
                            break;
                        }

                    case Variables_Generales.EstadosJuego.AVANCE:
                        {
                            Estado_Check = Variables_Generales.EstadosJuego.AVANCE;
                            Variables_Generales.Estado_Actual = new Estado_Avance();
                            break;
                        }
                    
                    default:break;
                        
                }
            }

            Variables_Generales.Estado_Actual.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // Dibuja el estado actual
            Variables_Generales.Estado_Actual.Draw(spriteBatch);

            # region MENSAJES DE ERROR
            spriteBatch.Begin();

            spriteBatch.DrawString(Variables_Generales.CheckStatusVar,
            "altoViewport = " + Variables_Generales.mensaje1.ToString() + System.Environment.NewLine +
            "anchoViewport = " + Variables_Generales.mensaje2.ToString() + System.Environment.NewLine +
            "limitePantallaX = " + Variables_Generales.mensaje3.ToString() + System.Environment.NewLine +
            "limitePantallaAncho = " + Variables_Generales.mensaje4.ToString() + System.Environment.NewLine + 
            "Zoom = " + Variables_Generales.mensaje5.ToString() + System.Environment.NewLine + 
            Variables_Generales.mensaje6,
            ChkStatVar, Color.DarkRed);

            spriteBatch.End();
            #endregion

            base.Draw(gameTime);
        }

    }
}
