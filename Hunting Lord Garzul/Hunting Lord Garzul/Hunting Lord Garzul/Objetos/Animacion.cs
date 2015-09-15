using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Hunting_Lord_Garzul.Objetos;

namespace Hunting_Lord_Garzul
{
    public class Animacion
    {
        #region VARIABLES

        // La textura con los sprites dentro
        public Texturas texturaCargada;

        // Nombre de la pieza a animar
        public string nombrePieza;

        // El tiempo que actualizamos el cuadro por ultima vez
        int elapsedTime;
        
        // El tiempo que mostramos el cuadro antes de cambiarlo
        public int frameTime;

        // Para pausarse en un frame especifico
        public bool pausa;

        // El numero de cuadros que tiene la animacion
        int oldframeCount;
        int frameCount;
        public int FrameCount
        {
            get { return frameCount; }
            set { frameCount = value; }
        }
        
        // El indice del cuadro que estamos mostrando
        int currentFrame;
        public int CurrentFrame
        {
            get { return currentFrame; }
            set { currentFrame = value; }
        }
        
        // El color del cuadro que estamos mostrando
        public Color color;
        
        // El area de la linea de sprite que queremos mostrar
        Rectangle sourceRect = new Rectangle();
        
        // El area donde queremos mostrar el sprite
        Rectangle destinationRect = new Rectangle();
        
        // Ancho y alto de un cuadro dado
        public int frameWidth;
        public int frameHeight;
        
        // El estado de la animacion
        public bool active;
        
        // Activa o desactiva el loopeo
        public bool looping;
        
        // Posicion de un cuadro determinado
        public Vector2 position;

        // Escala de Heroes con respecto al alto de la pantalla
        public float escalaAnimacion = Globales.ViewportHeight / Globales.Scalar;
        
        #endregion

        #region METODOS

        public void Initialize(string nombre)
        {
            nombrePieza = nombre;
        }

        /// <summary>
        /// Carga de textura al cambiar de animacion, es la que se usa durante el juego repetidas veces.
        /// </summary>
        /// <param name="texture"></param>
        public void CargarTextura(Texturas texture)
        {
            this.texturaCargada = texture;
            this.frameCount = int.Parse(texture.frame);
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
        public void CargarTextura(Texturas texture, Vector2 position,int frameWidth, int frameHeight,int frametime, Color color, bool looping)
        {
            // Mantiene una copia local de los valores obtenidos
            this.color = color;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.frameCount = int.Parse(texture.frame);
            this.oldframeCount = frameCount;
            this.frameTime = frametime;
            this.looping = looping;
            this.position = position;
            this.texturaCargada = texture;

            // Pone el tiempo en 0
            this.elapsedTime = 0;
            this.currentFrame = 0;

            // Cancela la pausa
            this.pausa = false;

            // Pone la animacion en activa por defecto
            this.active = true;
        }

        /// <summary>
        /// Rectangulo donde se encuentra esta pieza de animacion en la pantalla
        /// </summary>
        /// <returns></returns>
        public Rectangle ObtenerPosicion()
        {
            return destinationRect;
        }

        public void CambiarColor(Color tinte)
        {
            this.color = tinte;
        }

        public void Update(GameTime gameTime)
        {
            // No actualizar la animacion si no esta activa
            if (active == false)
                return;

            // Actualizar el tiempo transcurrido
            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            // Si el tiempo transcurrido es mayor al tiempo del cuadro tenemos que cambiar de cuadro
            // Si esta activo la pausa no se pasara al siguiente frame pero seguira dibujandose
            if (elapsedTime > frameTime && pausa == false)
            {
                // Ir al otro cuadro
                currentFrame++;

                // Si el cuadro actual es igual a la cuenta total de cuadros pasamos el cuadro actual a 0
                if (currentFrame >= frameCount || frameCount != oldframeCount || currentFrame >= oldframeCount)
                {
                    currentFrame = 0;
                    oldframeCount = frameCount;
                    // Si no hay loopeo desactivo la animacion
                    if (looping == false)
                        active = false;
                }

                // Pongo el tiempo transcurrido en 0
                elapsedTime = 0;
            }

            // Agarro el cuadro correcto en la linea de strip multiplicando el indice del cuadro actual por el ancho del frame
            sourceRect = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);

            // Escalo con respecto a la altura que deseo comparando el personaje con la pantalla.
            // Se cambia el valor de la escala desde Globales.Escalar
            float AspectRatio = (float)frameHeight / frameWidth;
            int Height = (int)((escalaAnimacion) + 0.5f);
            int Width = (int)((Height / AspectRatio) + 0.5f);

            // Seteo el rectangulo donde va a ir con las dimensiones ajustadas.
            destinationRect = new Rectangle((int)position.X - (int)(Width) / 2,
            (int)position.Y - (int)(Height) / 2,
            (int)(Width),
            (int)(Height));

        }

        public void Draw(SpriteBatch spriteBatch, Objetos.Globales.Mirada direccion)
        {
            // Solo dibujar la animacion si esta activa
            if (active)
            {
                if (direccion == Objetos.Globales.Mirada.LEFT)
                {
                    spriteBatch.Draw(texturaCargada.textura, destinationRect, sourceRect, color,
                        0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                }
                else
                {
                    spriteBatch.Draw(texturaCargada.textura, destinationRect, sourceRect, color);
                }
                
            }
        }

        public void Draw(SpriteBatch spriteBatch, Objetos.Globales.Mirada direccion, Color tinte)
        {
            // Solo dibujar la animacion si esta activa
            if (active)
            {
                if (direccion == Objetos.Globales.Mirada.LEFT)
                {
                    spriteBatch.Draw(texturaCargada.textura, destinationRect, sourceRect, tinte,
                        0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                }
                else
                {
                    spriteBatch.Draw(texturaCargada.textura, destinationRect, sourceRect, tinte);
                }

            }
        }

        #endregion
    }
}
