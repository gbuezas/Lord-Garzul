using System;
using System.IO;
using Hunting_Lord_Garzul.Abstractos.Estados;
using Hunting_Lord_Garzul.Abstractos.Heroes;
using Hunting_Lord_Garzul.Generales;
using Hunting_Lord_Garzul.Objetos;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Hunting_Lord_Garzul
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        #region VARIABLES

        // Variables necesarias para dibujar por default
        SpriteBatch _spriteBatch;

        // Donde se va a alojar el mensaje de chequeo de status
        readonly Vector2 _chkStatVar = new Vector2(50, 550);

        // Check de estado de juego
        Globales.EstadosJuego _estadoCheck;

        #endregion

        #region METODOS

        public Game()
        {
            var graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720,
                PreferMultiSampling = true
            };

            // Establezco la resolucion maxima adecuada para el dispositivo
            // Supuestamente con esta resolucion autoescala a menores
            // Hay que probarlo en algun lado

            // No estoy seguro de si esto me da antialiasing o ya con lo que puse cuando dibujo alcanza
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
            for (var i = 0; i < Globales.PlayersQuant; i++)
            {
                Globales.Players.Add(new JugadorPaladin());
            }

            // Agrego los enemigos a la lista
            for (var i = 0; i < Globales.EnemiesQuant; i++)
            {
                //Globales.players.Add(new IA_1((Globales.TargetCondition)azar.Next(0, 4)));
                Globales.Players.Add(new Ia1());
            }

            Globales.CurrentState = new EstadoAvance {EstadoEjecutandose = Globales.EstadosJuego.Titulo};

            // Ponemos este estado por defecto en un modo que no es nada, asi cuando va al case detecta incongruencia
            // y acomoda al que corresponde, que seria el que dice arriba en ejecutandose.
            _estadoCheck = Globales.EstadosJuego.Gameover;

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
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Globales.ViewportWidth = GraphicsDevice.Viewport.Width;
            Globales.ViewportHeight = GraphicsDevice.Viewport.Height;

            Globales.CurrentState.Load(GraphicsDevice.Viewport);

            // Cargo todas las texturas de los personas y sus movimientos de cada carpeta.
            // Acordarse que los png tienen que estar en la carpeta DEBUG para el modo DEBUG, y asi con cada modo.
            // Si no hay nada va al catch asi que no pasa nada
            # region TEXTURA_HEROES
            foreach (var heroe in Globales.Heroes)
            {
                try
                {
                    // Hago la lista solamente de los archivos PNG (animaciones de las piezas de los heroes) que estan en el content 
                    // para crear las texturas de cada uno.
                    var archivosPersonajes = Directory.GetFiles("Content/" + heroe, "*.png");
                    
                    foreach (var nombrePersonajes in archivosPersonajes)
                    {
                        var nombre = Path.GetFileNameWithoutExtension(nombrePersonajes);
                        var textura = new Texturas(Content.Load<Texture2D>(heroe + "/" + nombre), nombre);

                        switch (heroe)
                        {

                            case "Paladin":
                                {
                                    Globales.PaladinTextures.Add(textura);
                                    break;
                                }

                            case "IA_1":
                                {
                                    Globales.Ia1Textures.Add(textura);
                                    break;
                                }

                            default:
                                {
                                    break;
                                }

                        }
 
                        if (!Globales.Armors.Contains(nombre.Split('_')[0]))
                        {
                            Globales.Armors.Add(nombre.Split('_')[0]);
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

            foreach (var escenario in Globales.Scenes)
            {
                try
                {
                    var archivosNiveles = Directory.GetFiles("Content/" + escenario, "*.png");

                    foreach (var nombreNiveles in archivosNiveles)
                    {
                        var nombre = Path.GetFileNameWithoutExtension(nombreNiveles);
                        var textura = new Texturas(Content.Load<Texture2D>(escenario + "/" + nombre), nombre);

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
            Globales.PuntoBlanco = Content.Load<Texture2D>("Seleccion/puntoblanco");

            // Cargo titulos y pantallas de presentacion
            Globales.PantallaTitulo = Content.Load<Texture2D>("Titulo/TitleScreen");
            
            // Cargo pantalla de seleccion y selectores
            Globales.PantallaSeleccion = Content.Load<Texture2D>("Seleccion/fondo");
            //Variables_Generales.Selector = Content.Load<Texture2D>("Seleccion/Selector");
            
            // Cargo fuentes
            Globales.CheckStatusVar = Content.Load<SpriteFont>("Fuente_Prueba");
            Globales.CheckStatusVar2 = Content.Load<SpriteFont>("Fuente_Prueba_2");

            // Asigno posiciones iniciales de los personajes, tanto jugadores como IA
            var ejeX = 0;
            var ejeY = 0;
            foreach (var jugador in Globales.Players)
            {


                jugador.Initialize(new Vector2( GraphicsDevice.Viewport.TitleSafeArea.X + ejeX,
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

            Globales.ElapsedTime += gameTime.ElapsedGameTime;

            if (Globales.ElapsedTime <= TimeSpan.FromSeconds(1)) return;
            Globales.ElapsedTime -= TimeSpan.FromSeconds(1);
            Globales.FrameRate = Globales.FrameCounter;
            Globales.FrameCounter = 0;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Globales.FrameCounter++;

            GraphicsDevice.Clear(Color.White);

            // Dibuja el estado actual
            Globales.CurrentState.Draw(_spriteBatch);

            # region MENSAJES DE ERROR
            _spriteBatch.Begin();

            _spriteBatch.DrawString(Globales.CheckStatusVar,
            "altoViewport = " + Globales.Mensaje1 + Environment.NewLine +
            "anchoViewport = " + Globales.Mensaje2 + Environment.NewLine +
            "limitePantallaX = " + Globales.Mensaje3 + Environment.NewLine +
            "limitePantallaAncho = " + Globales.Mensaje4 + Environment.NewLine + 
            "Zoom = " + Globales.Mensaje5 + Environment.NewLine + 
            "FPS = " + Globales.FrameRate + Environment.NewLine,
            _chkStatVar, Color.DarkRed);

            _spriteBatch.End();
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
                foreach (var jugador in Globales.Players)
                {
                    jugador.Health += 1;
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
                Exit();
        }

        /// <summary>
        /// Habilita rectangulo de colisiones.
        /// </summary>
        private static void EnableColRec()
        {
            Globales.PreviousKeyboardState = Globales.CurrentKeyboardState;
            Globales.CurrentKeyboardState = Keyboard.GetState();

            // Acciones que no se tienen que repetir al mantener la tecla
            if (!Globales.PreviousKeyboardState.IsKeyDown(Keys.D2) || !Globales.CurrentKeyboardState.IsKeyUp(Keys.D2))
                return;
            
            Globales.EnableRectangles = !Globales.EnableRectangles;
        }

        /// <summary>
        /// Chequea en que estado tiene que estar el juego y lo gestiona.
        /// </summary>
        private void StateSwitch()
        {
            if (_estadoCheck == Globales.CurrentState.EstadoEjecutandose) return;
            switch (Globales.CurrentState.EstadoEjecutandose)
            {

                case Globales.EstadosJuego.Titulo:
                {
                    _estadoCheck = Globales.EstadosJuego.Titulo;
                    Globales.CurrentState = new EstadoTitulos();
                    break;
                }

                case Globales.EstadosJuego.Seleccion:
                {
                    _estadoCheck = Globales.EstadosJuego.Seleccion;
                    Globales.CurrentState = new EstadoSeleccion();
                    break;
                }

                case Globales.EstadosJuego.Avance:
                {
                    _estadoCheck = Globales.EstadosJuego.Avance;
                    Globales.CurrentState = new EstadoAvance();
                    break;
                }

                case Globales.EstadosJuego.Intro:
                    break;
                case Globales.EstadosJuego.Mapa:
                    break;
                case Globales.EstadosJuego.Vs:
                    break;
                case Globales.EstadosJuego.Pausa:
                    break;
                case Globales.EstadosJuego.Gameover:
                    break;
                case Globales.EstadosJuego.Final:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion
    }
}
