using Hunting_Lord_Garzul.Generales;
using Hunting_Lord_Garzul.Objetos;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Hunting_Lord_Garzul.Abstractos.Estados
{
    class EstadoAvance : Estados
    {
        #region VARIABLES

        #region MAPA Y PARALLAX

        // Mapa de tiles de las capas
        static readonly string[] MapaPiso = { "soil1", "soil1", "soil1", "soil1", "soil1", "soil1", "soil1", "soil1", "soil1", "soil1", "soil1", "soil1", "soil1", "soil1", "soil1", "soil1" };
        static readonly string[] MapaArboles = { "wood1", "wood1", "wood1", "wood1", "wood1", "wood1", "wood1", "wood1", "wood1", "wood1", "wood1", "wood1", "wood1", "wood1", "wood1", "wood1" };
        static readonly string[] MapaNubes = { "cloud1", "cloud2", "cloud3", "cloud4", "cloud5", "cloud6", "cloud7", "cloud8", "cloud9", "cloud10", "cloud11", "cloud1", "cloud2", "cloud3", "cloud4", "cloud5" };

        // Velocidades del parallax de cada capa
        readonly Parallax _piso = new Parallax(MapaPiso, 1f, 1f);
        readonly Parallax _arboles = new Parallax(MapaArboles, 0.8f, 0.5f);
        readonly Parallax _nubes = new Parallax(MapaNubes, 0.5f, 1f);

        #endregion

        // Genero un vector con la cantidad de rectangulos necesarios para pintar todo el mapa
        readonly Rectangle[] _sourceRect = new Rectangle[MapaNubes.Length];

        // Alto del nivel
        readonly int _varAltoNivel = Globales.ViewportHeight;

        // Ancho del nivel
        readonly int _varAnchoNivel = Globales.ViewportWidth / 4 * MapaNubes.Length;
        
        // Creo la variable de la camara en estatica
        static Camera _camara;

        #endregion

        #region METODOS

        /// <summary>
        /// Cargo los jugadores segun su clase seleccionada.
        /// Tambien cargo las capas de parallax.
        /// </summary>
        public override void Initialize()
        {
            // Agrego las diferentes capas de parallax
            Globales.Layers.Add(_nubes);
            Globales.Layers.Add(_arboles);
            Globales.Layers.Add(_piso);
        }

        /// <summary>
        /// Para asignarle el viewport en el load del game.
        /// </summary>
        public override void Load(Viewport viewport)
        {
            // Seteo el viewport correspondiente a la camara
            _camara = new Camera(viewport, _varAltoNivel, Globales.ViewportWidth / 4 * MapaNubes.Length);
        }

        public override void Update(GameTime gameTime)
        {
            // Guarda y lee los estados actuales y anteriores del joystick y teclado
            Input_Management();

            // Actualiza jugador
            foreach(var jugador in Globales.Players)
            {
                jugador.UpdatePlayer(gameTime, _camara.LimitesPantalla, _varAltoNivel, _varAnchoNivel);
            }

            // Ajusto los limites de la camara para que no pueda mostrar mas de este rectangulo
            _camara.Limits = new Rectangle(0, 0, Globales.ViewportWidth / 4 * MapaNubes.Length, _varAltoNivel);
            
            // Tomo tiempo transcurrido.
            //float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Para poder controlar al otro personaje por separado
            // Si lo saco de aca no me toma los cambios del control
            Globales.Players[1].Controls[(int)Globales.Controls.Up] = Keys.Up;
            Globales.Players[1].Controls[(int)Globales.Controls.Down] = Keys.Down;
            Globales.Players[1].Controls[(int)Globales.Controls.Left] = Keys.Left;
            Globales.Players[1].Controls[(int)Globales.Controls.Right] = Keys.Right;
            Globales.Players[1].Controls[(int)Globales.Controls.Button1] = Keys.Space;

            Globales.Players[2].Controls[(int)Globales.Controls.Up] = Keys.I;
            Globales.Players[2].Controls[(int)Globales.Controls.Down] = Keys.K;
            Globales.Players[2].Controls[(int)Globales.Controls.Left] = Keys.J;
            Globales.Players[2].Controls[(int)Globales.Controls.Right] = Keys.L;
            Globales.Players[2].Controls[(int)Globales.Controls.Button1] = Keys.Enter;

            Globales.Players[3].Controls[(int)Globales.Controls.Up] = Keys.I;
            Globales.Players[3].Controls[(int)Globales.Controls.Down] = Keys.K;
            Globales.Players[3].Controls[(int)Globales.Controls.Left] = Keys.J;
            Globales.Players[3].Controls[(int)Globales.Controls.Right] = Keys.L;
            Globales.Players[3].Controls[(int)Globales.Controls.Button1] = Keys.Enter;

            // Enemigo
            Globales.Players[4].Controls = null; 
            Globales.Players[5].Controls = null;
            
            // Hacer un foreach para todos los personajes que quedan en camara, 
            // solo los controlados por humanos, la maquina no, asi pueden salir y no me desconcha toda la camara y el zoom
            _camara.ViewTargets.Clear();
            foreach( var jugador in Globales.Players )
            {
                if(!jugador.Machine)
                {
                    _camara.ViewTargets.Add(jugador.GetPosition());
                }
            }
            _camara.CentrarCamara();

            Globales.Mensaje1 = Globales.ViewportHeight;
            Globales.Mensaje2 = Globales.ViewportWidth;
        }

        /// <summary>
        /// Aca se hace el calculo de que personaje se dibuja primero
        /// Tambien se hace el calculo de la camara y el parallax
        /// </summary>
        public override void Draw(SpriteBatch spriteBatch)
        {

            # region CAPAS

            foreach (var capa in Globales.Layers)
            {
                
                _camara.Parallax = new Vector2(capa.ParallaxX, capa.ParallaxY);

                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.AnisotropicClamp, null, null, null, _camara.ViewMatrix);

                // El rectangulo contenedor del tile
                var rectangulo = 0;
                // La posicion donde va el siguiente rectangulo
                var posicion = 0;

                foreach (var seccion in capa.CapaParallax)
                {
                    foreach (var avance in Globales.AvanceTextures)
                    {
                        if (seccion == avance.Piece)
                        {
                            _sourceRect[rectangulo] = new Rectangle(posicion, 0,
                                Globales.ViewportWidth / 4,
                                Globales.ViewportHeight);

                            // Recalculo el rectangulo para que se adapte a la velocidad correspondiente de la capa
                            capa.RectanguloParallax = _sourceRect[rectangulo];
                            capa.RectanguloParallax.X += (int)(_camara.LimitesPantalla.X * capa.ParallaxX + 0.5f);

                            // Mensajes de chequeo
                            Globales.Mensaje3 = _camara.LimitesPantalla.X;
                            Globales.Mensaje4 = _camara.LimitesPantalla.Width;

                            // Si no esta dentro de la camara no lo dibujo
                            if (_camara.EnCamara(capa.RectanguloParallax))
                            {
                                spriteBatch.Draw(avance.Textura, _sourceRect[rectangulo], Color.White);
                            }

                            posicion += Globales.ViewportWidth / 4;
                        }

                    }
                    rectangulo++;
                }

                spriteBatch.End();
            }

            #endregion

            #region PERSONAJES

            // SpriteSortMode.Deferred soluciono el problema de que pegaba las capas como se le cantaba el ojete, estaba en BacktoFront
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.AnisotropicClamp, null, null, null, _camara.ViewMatrix); 

            Reordenar_Personajes(spriteBatch);
            
            spriteBatch.End();

            #endregion
            
        }

        public override void UpdateState(GameTime gameTime)
        {

        }

        #endregion
    }
}
