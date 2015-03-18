using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Hunting_Lord_Garzul.Objetos;

namespace Hunting_Lord_Garzul
{
    public abstract class Jugadores
    {
        // Esta bandera es para que no se vuelva a dibujar varias veces el mismo objeto,
        // en caso de que el eje Y del mismo se repita con otro objeto
        private Boolean Dibujado = false;
        public Boolean dibujado
        {
            get { return Dibujado; }
            set { Dibujado = value; }
        }
        
        // Rectangulo donde se pone al jugador, esto es para poder modificar su tamaño,
        // esta no es la altura del dibujo que se toma
        private Rectangle RectanguloJugador;
        public Rectangle RecJugador
        {
          get { return RectanguloJugador; }
          set { RectanguloJugador = value; }
        }

        // Posicion del jugador relativa a la parte superior izquierda de la pantalla
        public abstract Vector2 Posicion();

        // Controles del jugador
        Keys[] Controles = new Keys[Enum.GetNames(typeof(Variables_Generales.Controles)).Length];
        public Keys[] controles
        {
            get { return Controles; }
            set { Controles = value; }
        }

        // Inicializar al jugador
        public abstract void Initialize(Vector2 posicion);
        
        // Actualizar animacion
        public abstract void Update(GameTime gameTime);
        
        // Dibujar Jugador
        public abstract void Draw(SpriteBatch spriteBatch);
        
        // Actualizar cosas del jugador
        public abstract void UpdatePlayer(GameTime gameTime, int Jugador, Rectangle LimitesJugador, int AltoNivel, int AnchoNivel);

    }
}
