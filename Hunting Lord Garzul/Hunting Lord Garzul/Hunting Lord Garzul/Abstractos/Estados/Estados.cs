using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Hunting_Lord_Garzul.Objetos;

namespace Hunting_Lord_Garzul.Objetos
{
    public abstract class Estados
    {
        private Globales.EstadosJuego Estado_Ejecutandose;
        public Globales.EstadosJuego Estado_ejecutandose
        {
            get { return Estado_Ejecutandose; }
            set { Estado_Ejecutandose = value; }
        }

        // Inicializar estado
        public abstract void Initialize();

        // Cargar la camara y otras cosas
        public abstract void Load(Viewport _viewport);

        // Actualizar estado
        public abstract void Update(GameTime gameTime);

        // Dibujar estado
        public abstract void Draw(SpriteBatch spriteBatch);

        // Actualizar comportamiento de estado
        public abstract void UpdateState(GameTime gameTime);

        // Ordenar lista de personajes segun su eje Y
        public void Reordenar_Personajes(SpriteBatch spriteBatch)
        {
            // Genero una lista para todas las coordenadas de los personajes y las agrego
            List<float> Lista_Coordenadas = new List<float>();

            foreach (Jugadores Jugador in Globales.players)
            {
                Lista_Coordenadas.Add(Jugador.Posicion().Y);
                
                // Reseteo el estado de dibujado
                Jugador.dibujado = false;
            }

            // Ordeno la lista y la invierto para obtener el efecto buscado
            Lista_Coordenadas.Sort();
            
            // Ahora por cada elemento de la lista ordenada comparo quien tiene el eje del primer elemento y lo dibujo
            // despues con el segundo y asi sucesivamente
            foreach (float Coordenada in Lista_Coordenadas)
            {
                foreach (Jugadores Jugador in Globales.players)
                {
                    if (Jugador.dibujado == false && Jugador.Posicion().Y == Coordenada)
                    {
                        Jugador.Draw(spriteBatch);

                        // Lo pongo como dibujado para que no lo repita
                        Jugador.dibujado = true;
                    }
                }
            }
        }

        // Obtiene los gamepads y teclado que se tocaron y se estan tocando
        public void Input_Management()
        {
            // Guarda los estados anteriores del joystick y del teclado
            Globales.previousKeyboardState = Globales.currentKeyboardState;
            
            //for (int i = 0; i < 4;i++ )
            //{
            //    Variables_Generales.previousGamePadState[i] = Variables_Generales.currentGamePadState[i];
            //    Variables_Generales.currentGamePadState[i] = GamePad.GetState((PlayerIndex)i);
            //}

        }
    }
}
