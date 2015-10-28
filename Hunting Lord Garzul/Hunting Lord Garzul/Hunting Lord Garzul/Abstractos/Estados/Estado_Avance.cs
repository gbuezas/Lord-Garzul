using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Hunting_Lord_Garzul.Objetos
{
    class Estado_Avance : Estados
    {
        #region VARIABLES

        #region MAPA Y PARALLAX

        // Mapa de tiles de las capas
        static string[] Mapa_Piso = new string[] { "soil1", "soil1", "soil1", "soil1", "soil1", "soil1", "soil1", "soil1", "soil1", "soil1", "soil1", "soil1", "soil1", "soil1", "soil1", "soil1" };
        static string[] Mapa_Arboles = new string[] { "wood1", "wood1", "wood1", "wood1", "wood1", "wood1", "wood1", "wood1", "wood1", "wood1", "wood1", "wood1", "wood1", "wood1", "wood1", "wood1" };
        static string[] Mapa_Nubes = new string[] { "cloud1", "cloud2", "cloud3", "cloud4", "cloud5", "cloud6", "cloud7", "cloud8", "cloud9", "cloud10", "cloud11", "cloud1", "cloud2", "cloud3", "cloud4", "cloud5", };

        // Velocidades del parallax de cada capa
        Parallax Piso = new Parallax(Mapa_Piso, 1f, 1f);
        Parallax Arboles = new Parallax(Mapa_Arboles, 0.8f, 0.5f);
        Parallax Nubes = new Parallax(Mapa_Nubes, 0.5f, 1f);

        #endregion

        // Genero un vector con la cantidad de rectangulos necesarios para pintar todo el mapa
        Rectangle[] sourceRect = new Rectangle[Mapa_Nubes.Length];

        // Alto del nivel
        int Var_AltoNivel = Globales.ViewportHeight;

        // Ancho del nivel
        int Var_AnchoNivel = Globales.ViewportWidth / 4 * Mapa_Nubes.Length;
        
        // Creo la variable de la camara en estatica
        static Camera Camara;

        #endregion

        #region METODOS

        /// <summary>
        /// Cargo los jugadores segun su clase seleccionada.
        /// Tambien cargo las capas de parallax.
        /// </summary>
        public override void Initialize()
        {
            // Agrego las diferentes capas de parallax
            Globales.Layers.Add(Nubes);
            Globales.Layers.Add(Arboles);
            Globales.Layers.Add(Piso);
        }

        /// <summary>
        /// Para asignarle el viewport en el load del game.
        /// </summary>
        public override void Load(Viewport _viewport)
        {
            // Seteo el viewport correspondiente a la camara
            Camara = new Camera(_viewport, Var_AltoNivel, Globales.ViewportWidth / 4 * Mapa_Nubes.Length);
        }

        public override void Update(GameTime gameTime)
        {
            // Guarda y lee los estados actuales y anteriores del joystick y teclado
            Input_Management();

            // Actualiza jugador
            foreach(Jugadores Jugador in Globales.players)
            {
                Jugador.UpdatePlayer(gameTime, Camara.LimitesPantalla, Var_AltoNivel, Var_AnchoNivel);
            }

            // Ajusto los limites de la camara para que no pueda mostrar mas de este rectangulo
            Camara.Limits = new Rectangle(0, 0, Globales.ViewportWidth / 4 * Mapa_Nubes.Length, Var_AltoNivel);
            
            // Tomo tiempo transcurrido.
            //float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Para poder controlar al otro personaje por separado
            // Si lo saco de aca no me toma los cambios del control
            Globales.players[1].controls[(int)Globales.Controls.UP] = Keys.Up;
            Globales.players[1].controls[(int)Globales.Controls.DOWN] = Keys.Down;
            Globales.players[1].controls[(int)Globales.Controls.LEFT] = Keys.Left;
            Globales.players[1].controls[(int)Globales.Controls.RIGHT] = Keys.Right;
            Globales.players[1].controls[(int)Globales.Controls.BUTTON_1] = Keys.Space;

            Globales.players[2].controls[(int)Globales.Controls.UP] = Keys.I;
            Globales.players[2].controls[(int)Globales.Controls.DOWN] = Keys.K;
            Globales.players[2].controls[(int)Globales.Controls.LEFT] = Keys.J;
            Globales.players[2].controls[(int)Globales.Controls.RIGHT] = Keys.L;
            Globales.players[2].controls[(int)Globales.Controls.BUTTON_1] = Keys.Enter;

            Globales.players[3].controls[(int)Globales.Controls.UP] = Keys.I;
            Globales.players[3].controls[(int)Globales.Controls.DOWN] = Keys.K;
            Globales.players[3].controls[(int)Globales.Controls.LEFT] = Keys.J;
            Globales.players[3].controls[(int)Globales.Controls.RIGHT] = Keys.L;
            Globales.players[3].controls[(int)Globales.Controls.BUTTON_1] = Keys.Enter;

            // Enemigo
            Globales.players[4].controls = null; 
            Globales.players[5].controls = null;
            
            // Hacer un foreach para todos los personajes que quedan en camara, 
            // solo los controlados por humanos, la maquina no, asi pueden salir y no me desconcha toda la camara y el zoom
            Camara.ViewTargets.Clear();
            foreach( Jugadores Jugador in Globales.players )
            {
                if(!Jugador.machine)
                {
                    Camara.ViewTargets.Add(Jugador.GetPosition());
                }
            }
            Camara.CentrarCamara();

            Globales.mensaje1 = Globales.ViewportHeight;
            Globales.mensaje2 = Globales.ViewportWidth;
        }

        /// <summary>
        /// Aca se hace el calculo de que personaje se dibuja primero
        /// Tambien se hace el calculo de la camara y el parallax
        /// </summary>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {

            # region CAPAS

            // El rectangulo contenedor del tile
            int rectangulo;
            // La posicion donde va el siguiente rectangulo
            int posicion;
            
            foreach (Parallax capa in Globales.Layers)
            {
                
                Camara.parallax = new Vector2(capa.parallax_x, capa.parallax_y);

                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.AnisotropicClamp, null, null, null, Camara.ViewMatrix);

                rectangulo = 0;
                posicion = 0;

                foreach (string seccion in capa.capa_parallax)
                {
                    foreach (Texturas avance in Globales.AvanceTextures)
                    {
                        if (seccion == avance.piece)
                        {
                            sourceRect[rectangulo] = new Rectangle(posicion, 0,
                                Globales.ViewportWidth / 4,
                                Globales.ViewportHeight);

                            // Recalculo el rectangulo para que se adapte a la velocidad correspondiente de la capa
                            capa.RectanguloParallax = sourceRect[rectangulo];
                            capa.RectanguloParallax.X += (int)(Camara.LimitesPantalla.X * capa.parallax_x + 0.5f);

                            // Mensajes de chequeo
                            Globales.mensaje3 = Camara.LimitesPantalla.X;
                            Globales.mensaje4 = Camara.LimitesPantalla.Width;

                            // Si no esta dentro de la camara no lo dibujo
                            if (Camara.EnCamara(capa.RectanguloParallax))
                            {
                                spriteBatch.Draw(avance.textura, sourceRect[rectangulo], Color.White);
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
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.AnisotropicClamp, null, null, null, Camara.ViewMatrix); 

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
