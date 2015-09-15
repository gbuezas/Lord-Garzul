using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Hunting_Lord_Garzul.Objetos;

namespace Hunting_Lord_Garzul
{
    public abstract class Jugadores
    {
        #region VARIABLES

            #region CONTROLES

                /// <summary>
                /// Esta bandera es para que no se vuelva a dibujar varias veces el mismo objeto, 
                /// en caso de que el eje Y del mismo se repita con otro objeto
                /// </summary>
                private Boolean Drawn = false;
        
                /// <summary>
                /// Si el personaje es manejado por la maquina o por un humano
                /// </summary>
                private Boolean Machine = false;
        
                /// <summary>
                /// Para que lado esta mirando el personaje
                /// </summary>
                private Globales.Mirada Direction;
        
                /// <summary>
                /// Controles del jugador
                /// </summary>
                private Keys[] Controls = new Keys[Enum.GetNames(typeof(Globales.Controls)).Length];
        
                /// <summary>
                /// Esta es una copia de las animaciones que va a usar el hijo de esta clase, en donde se decide que texturas va a utilizar.
                /// En esta instancia la clase no sabe que texturas se van a utilizar. 
                /// </summary>
                private Animacion[] Animations = null;

                /// <summary>
                /// Contador de vuelta lógica, para evitar que se repita la lógica de varias acciones en un mismo frame,
                /// principalmente fue creado para evitar que se quite HP varias veces en el mismo frame cuando
                /// se realiza un ataque.
                /// </summary>
                private int Logic_Counter = 0;

            #endregion

            #region JUGABILIDAD

                /// <summary>
                /// Si el personaje fue dañado 
                /// </summary>
                private Boolean Injured = false;
        
                /// <summary>
                /// La cantidad de daño recibida 
                /// </summary>
                private int Injured_Value = 0;
        
                /// <summary>
                /// La vitalidad del personaje
                /// </summary>
                private int Health = 100;
        
                /// <summary>
                /// Si pierde toda su HP pasa a modo fantasma
                /// </summary>
                private Boolean Ghost_Mode = false;
        
            #endregion

        #endregion

        #region METODOS

            #region GET-SET

                #region CONTROLES

                    public Boolean drawn
                    {
                        get { return Drawn; }
                        set { Drawn = value; }
                    }

                    public Boolean machine
                    {
                        get { return Machine; }
                        set { Machine = value; }
                    }

                    public Globales.Mirada direction
                    {
                        get { return Direction; }
                        set { Direction = value; }
                    }

                    public Keys[] controls
                    {
                        get { return Controls; }
                        set { Controls = value; }
                    }

                    internal Animacion[] animations
                    {
                        get { return Animations; }
                        set { Animations = value; }
                    }

                    public int logic_counter
                    {
                        get { return Logic_Counter; }
                        set { Logic_Counter = value; }
                    }

                #endregion

                #region JUGABILIDAD

                    public Boolean injured
                    {
                        get { return Injured; }
                        set { Injured = value; }
                    }

                    public int injured_value
                    {
                        get { return Injured_Value; }
                        set { Injured_Value = value; }
                    }

                    public int health
                    {
                        get { return Health; }
                        set { Health = value; }
                    }

                    public Boolean ghost_mode
                    {
                        get { return Ghost_Mode; }
                        set { Ghost_Mode = value; }
                    }

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
                public abstract void UpdatePlayer(GameTime gameTime, Rectangle LimitesJugador, int AltoNivel, int AnchoNivel);

                // Carga los set de armadura que corresponden a cada pieza del cuerpo.
                public abstract void UpdateArmor(List<Piece_Set> set_pieces);

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
