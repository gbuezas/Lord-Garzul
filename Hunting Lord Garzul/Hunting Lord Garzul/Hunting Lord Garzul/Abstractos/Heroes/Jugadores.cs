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

        // Esta bandera es para que no se vuelva a dibujar varias veces el mismo objeto,
        // en caso de que el eje Y del mismo se repita con otro objeto
        private Boolean Dibujado = false;
        public Boolean dibujado
        {
            get { return Dibujado; }
            set { Dibujado = value; }
        }
        
        // Controles del jugador
        private Keys[] Controles = new Keys[Enum.GetNames(typeof(Globales.Controls)).Length];
        public Keys[] controles
        {
            get { return Controles; }
            set { Controles = value; }
        }

        // Piezas de animacion para chequear colision. 
        // Esta es una copia de las animaciones que va a usar el hijo de esta clase, en donde se decide que texturas va a utilizar
        // En esta instancia la clase no sabe que textras se van a utilizar.
        private Animacion[] Animaciones = null;
        internal Animacion[] animaciones
        {
            get { return Animaciones; }
            set { Animaciones = value; }
        }

        #endregion

        #region METODOS

        // Inicializar al jugador
        public abstract void Initialize(Vector2 posicion);
        
        // Actualizar animacion
        public abstract void Update(GameTime gameTime);
        
        // Dibujar Jugador
        public abstract void Draw(SpriteBatch spriteBatch);
        
        // Actualizar cosas del jugador
        public abstract void UpdatePlayer(GameTime gameTime, int Jugador, Rectangle LimitesJugador, int AltoNivel, int AnchoNivel);

        // Actualizar armadura
        public abstract void UpdateArmor(List<Piece_Set> set_pieces);

        // Posicion del jugador relativa a la parte superior izquierda de la pantalla
        public abstract Vector2 Posicion();

        #endregion
    }
}
