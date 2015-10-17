using Hunting_Lord_Garzul.Generales;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hunting_Lord_Garzul.Objetos
{
    public class Animacion
    {
        #region VARIABLES

        // La textura con los sprites dentro
        public Texturas LoadedTexture;

        // Nombre de la pieza a animar
        public string PieceName;

        // El tiempo que actualizamos el cuadro por ultima vez
        int _elapsedTime;
        
        // El tiempo que mostramos el cuadro antes de cambiarlo
        public int FrameTime;

        // Para pausarse en un frame especifico
        public bool Pause;

        // El numero de cuadros que tiene la animacion
        int _oldFrameCount;
        public int FrameCount { get; set; }

        // El indice del cuadro que estamos mostrando
        public int CurrentFrame { get; set; }

        // El color del cuadro que estamos mostrando
        public Color Color;
        
        // El area de la linea de sprite que queremos mostrar
        Rectangle _sourceRect;
        
        // El area donde queremos mostrar el sprite
        Rectangle _destinationRect;
        
        // Ancho y alto de un cuadro dado
        public int FrameWidth;
        public int FrameHeight;
        
        // El estado de la animacion
        public bool Active;
        
        // Activa o desactiva el loopeo
        public bool Looping;
        
        // Posicion de un cuadro determinado
        public Vector2 Position;

        // Escala de Heroes con respecto al alto de la pantalla
        public float EscalaAnimacion = Globales.ViewportHeight / Globales.Scalar;
        
        #endregion

        #region METODOS

        public void Initialize(string nombre)
        {
            PieceName = nombre;
        }

        /// <summary>
        /// Carga de textura al cambiar de animacion, es la que se usa durante el juego repetidas veces.
        /// </summary>
        /// <param name="texture"></param>
        public void LoadTexture(Texturas texture)
        {
            LoadedTexture = texture;
            FrameCount = int.Parse(texture.Frame);
        }

        /// <summary>
        /// Cargo la textura por primera vez, o cuando cambio el set de alguna pieza.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="frameWidth"></param>
        /// <param name="frameHeight"></param>
        /// <param name="frametime"></param>
        /// <param name="color"></param>
        /// <param name="looping"></param>
        public void LoadTexture(Texturas texture, Vector2 position, int frameWidth, int frameHeight, int frametime, Color color, bool looping)
        {
            // Mantiene una copia local de los valores obtenidos
            Color = color;
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;
            FrameCount = int.Parse(texture.Frame);
            _oldFrameCount = FrameCount;
            FrameTime = frametime;
            Looping = looping;
            Position = position;
            LoadedTexture = texture;

            // Pone el tiempo en 0
            _elapsedTime = 0;
            CurrentFrame = 0;

            // Cancela la pausa
            Pause = false;

            // Pone la animacion en activa por defecto
            Active = true;
        }

        /// <summary>
        /// Rectangulo donde se encuentra esta pieza de animacion en la pantalla
        /// </summary>
        /// <returns></returns>
        public Rectangle GetPosition()
        {
            return _destinationRect;
        }

        public void ColorChange(Color tinte)
        {
            Color = tinte;
        }

        public void Update(GameTime gameTime)
        {
            // No actualizar la animacion si no esta activa
            if (Active == false)
                return;

            // Actualizar el tiempo transcurrido
            _elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            // Si el tiempo transcurrido es mayor al tiempo del cuadro tenemos que cambiar de cuadro
            // Si esta activo la pausa no se pasara al siguiente frame pero seguira dibujandose
            if (_elapsedTime > FrameTime && Pause == false)
            {
                // Ir al otro cuadro
                CurrentFrame++;

                // Si el cuadro actual es igual a la cuenta total de cuadros pasamos el cuadro actual a 0
                if (CurrentFrame >= FrameCount || FrameCount != _oldFrameCount || CurrentFrame >= _oldFrameCount)
                {
                    CurrentFrame = 0;
                    _oldFrameCount = FrameCount;
                    // Si no hay loopeo desactivo la animacion
                    if (Looping == false)
                        Active = false;
                }

                // Pongo el tiempo transcurrido en 0
                _elapsedTime = 0;
            }

            // Agarro el cuadro correcto en la linea de strip multiplicando el indice del cuadro actual por el ancho del frame
            _sourceRect = new Rectangle(CurrentFrame * FrameWidth, 0, FrameWidth, FrameHeight);

            // Escalo con respecto a la altura que deseo comparando el personaje con la pantalla.
            // Se cambia el valor de la escala desde Globales.Escalar
            var aspectRatio = (float)FrameHeight / FrameWidth;
            var height = (int)((EscalaAnimacion) + 0.5f);
            var width = (int)((height / aspectRatio) + 0.5f);

            // Seteo el rectangulo donde va a ir con las dimensiones ajustadas.
            _destinationRect = new Rectangle((int)Position.X - width / 2,
            (int)Position.Y - height / 2,
            width,
            height);

        }

        public void Draw(SpriteBatch spriteBatch, Globales.Mirada direccion)
        {
            // Solo dibujar la animacion si esta activa
            if (!Active) return;

            if (direccion == Globales.Mirada.Left)
            {
                spriteBatch.Draw(LoadedTexture.Textura, _destinationRect, _sourceRect, Color,
                    0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            }
            else
            {
                spriteBatch.Draw(LoadedTexture.Textura, _destinationRect, _sourceRect, Color);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Globales.Mirada direccion, Color tinte)
        {
            // Solo dibujar la animacion si esta activa
            if (!Active) return;

            if (direccion == Globales.Mirada.Left)
            {
                spriteBatch.Draw(LoadedTexture.Textura, _destinationRect, _sourceRect, tinte,
                    0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            }
            else
            {
                spriteBatch.Draw(LoadedTexture.Textura, _destinationRect, _sourceRect, tinte);
            }
        }

        #endregion
    }
}
