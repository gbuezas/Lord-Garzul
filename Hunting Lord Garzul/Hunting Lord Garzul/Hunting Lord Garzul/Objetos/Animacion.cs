using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Hunting_Lord_Garzul.Objetos;

namespace Hunting_Lord_Garzul
{
    class Animacion
    {

        #region VARIABLES

        // La imagen con los sprites
        public Texture2D spriteStrip;

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
        Color color;
        
        // El area de la linea de sprite que queremos mostrar
        Rectangle sourceRect = new Rectangle();
        
        // El area donde queremos mostrar el sprite
        Rectangle destinationRect = new Rectangle();
        
        // Ancho de un cuadro dado
        public int FrameWidth;
        
        // Alto de un cuadro dado
        public int FrameHeight;
        
        // El estado de la animacion
        public bool Active;
        
        // Activa o desactiva el loopeo
        public bool Looping;
        
        // Posicion de un cuadro determinado
        public Vector2 Position;

        // Escala de Heroes con respecto al alto de la pantalla
        public float EscalaHeroes = Variables_Generales.AltoViewport/4;
        // Escala de objetos animados
        // public float EscalaObjetosAnimados = Variables_Generales.AltoViewport/5;

        #endregion

        public void Initialize(Texture2D texture, Vector2 position,int frameWidth, int frameHeight,int frameCount,int frametime, Color color, bool looping)
        {
            // Mantiene una copia local de los valores obtenidos
            this.color = color;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.frameCount = frameCount;
            this.oldframeCount = frameCount;
            this.frameTime = frametime;
            
            Looping = looping;
            Position = position;
            spriteStrip = texture;

            // Pone el tiempo en 0
            elapsedTime = 0;
            currentFrame = 0;

            // Cancela la pausa
            pausa = false;

            // Pone la animacion en activa por defecto
            Active = true;
        }

        public void Update(GameTime gameTime)
        {
            // No actualizar la animacion si no esta activa
            if (Active == false)
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
                    if (Looping == false)
                        Active = false;
                }

                // Pongo el tiempo transcurrido en 0
                elapsedTime = 0;
            }

            // Agarro el cuadro correcto en la linea de strip multiplicando el indice del cuadro actual por el ancho del frame
            sourceRect = new Rectangle(currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);

            // Escalo con respecto a la altura que deseo comparando el personaje con la pantalla.
            // Este valor esta en variables generales para simplificar su cambio (EscalaHeroes).
            float AspectRatio = (float)FrameHeight / FrameWidth;
            int Height = (int)((EscalaHeroes) + 0.5f);
            int Width = (int)((Height / AspectRatio) + 0.5f);

            // Seteo el rectangulo donde va a ir con las dimensiones ajustadas.
            destinationRect = new Rectangle((int)Position.X - (int)(Width) / 2,
            (int)Position.Y - (int)(Height) / 2,
            (int)(Width),
            (int)(Height));

        }

        // Dibuja la animacion del strip
        public void Draw(SpriteBatch spriteBatch, Objetos.Variables_Generales.Mirada direccion)
        {
            // Solo dibujar la animacion si esta activa
            if (Active)
            {
                if (direccion == Objetos.Variables_Generales.Mirada.IZQUIERDA)
                {
                    spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, color,
                        0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                }
                else
                {
                    spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, color);
                }
                
            }
        }

    }
}
