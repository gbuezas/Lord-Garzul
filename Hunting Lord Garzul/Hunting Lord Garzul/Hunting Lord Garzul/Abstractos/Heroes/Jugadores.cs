using System;
using System.Collections.Generic;
using Hunting_Lord_Garzul.Generales;
using Hunting_Lord_Garzul.Objetos;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Hunting_Lord_Garzul.Abstractos.Heroes
{
    public abstract class Jugadores
    {
        #region VARIABLES

            #region CONTROLES

        /// <summary>
                /// Controles del jugador
                /// </summary>
                private Keys[] _controls = new Keys[Enum.GetNames(typeof(Globales.Controls)).Length];

        #endregion

            #region JUGABILIDAD

        /// <summary>
                /// La vitalidad del personaje
                /// </summary>
                private int _health = 100;

        #endregion

        #endregion

        #region METODOS

            #region GET-SET

                #region CONTROLES

                    public bool Drawn { get; set; }

        public bool Machine { get; set; }

        public Globales.Mirada Direction { get; set; }

        public Keys[] Controls
                    {
                        get { return _controls; }
                        set { _controls = value; }
                    }

                    internal Animacion[] Animations { get; set; }

        public int LogicCounter { get; set; }

        #endregion

                #region JUGABILIDAD

                    public bool Injured { get; set; }

        public int InjuredValue { get; set; }

        public int Health
                    {
                        get { return _health; }
                        set { _health = value; }
                    }

                    public bool GhostMode { get; set; }

        #endregion

            #endregion

            #region ABSTRACTAS

                // Inicializar al jugador
                public abstract void Initialize(Vector2 posicion);

                // Actualizar animacion
                public abstract void Update(GameTime gameTime);

                // Dibujar Jugador
                public abstract void Draw(SpriteBatch spriteBatch);

                // Actualizar cosas del jugador - GAB retocar
                public abstract void UpdatePlayer(GameTime gameTime, Rectangle limitesJugador, int altoNivel, int anchoNivel);

                // Carga los set de armadura que corresponden a cada pieza del cuerpo.
                public abstract void UpdateArmor(List<PieceSet> setPieces);

                // Obtiene posicion del jugador en pantalla
                public abstract Vector2 GetPosition();

                // Cambiar color a la animacion
                public abstract void ColorAnimationChange(Color tinte);

                // Cambiar color a una pieza de la animacion
                public abstract void ColorPieceChange(Color tinte, int pieza);

                // Obtener frame actual de la animacion, se posa en la primer pieza del vector para obtenerla
                public abstract int GetCurrentFrame();

                // Obtener frame totales de la animacion, se posa en la primer pieza del vector para obtenerla
                public abstract int GetAnimationFrames();

                // Activa o desactiva al jugador (si no esta activo no se dibuja)
                public abstract void ActivatePlayer(bool active);

            #endregion

        #endregion
    }
}
