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

            #endregion

            #region JUGABILIDAD

                /// <summary>
                /// Cuando da�o a un personaje lo marco en esta lista.
                /// La resta se hace inmediatamente en vuelta logica, no de dibujado, del damnificado.
                /// Para evitar que se vuelva a generar da�o en un plazo corto se utilizara esta variable que tendra en cuenta a quien se da�o y sera interna de cada atacante, 
                /// la misma se reseteara cuando acabe la animacion del golpe correspondiente.
                /// Siempre tiene que englobar al total de personajes que estan en el juego (tanto jugables como IA).
                /// </summary>
                private Boolean[] Injured = new bool[Globales.totalQuant];
        
                /// <summary>
                /// La cantidad de da�o recibida, en un futuro sera un objeto o un struct que pueda contener distintos tipos de da�o.
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

                    protected bool[] injured
                    {
                        get { return Injured; }
                        set { Injured = value; }
                    }

                    internal Animacion[] animations
                    {
                        get { return Animations; }
                        set { Animations = value; }
                    }
                    
                #endregion

                #region JUGABILIDAD

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
                public abstract int GetTotalFrames();

                // Activa o desactiva al jugador (si no esta activo no se dibuja)
                public abstract void ActivatePlayer(bool active);
                
                // Limpio la lista interna de personajes que da�e, este metodo se usa al terminar una animacion que da�a.
                public abstract void ResetInjured();

            #endregion

        #endregion
    }
}
