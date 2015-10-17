using System.Collections.Generic;
using Hunting_Lord_Garzul.Generales;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
// ReSharper disable LoopCanBePartlyConvertedToQuery
// ReSharper disable InvertIf
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Hunting_Lord_Garzul.Abstractos.Estados
{
    public abstract class Estados
    {
        public Globales.EstadosJuego EstadoEjecutandose { get; set; }

        // Inicializar estado
        public abstract void Initialize();

        // Cargar la camara y otras cosas
        public abstract void Load(Viewport viewport);

        // Actualizar estado
        public abstract void Update(GameTime gameTime);

        // Dibujar estado
        public abstract void Draw(SpriteBatch spriteBatch);

        // Actualizar comportamiento de estado
        public abstract void UpdateState(GameTime gameTime);

        // Ordenar lista de personajes segun su eje Y
        // Ahora con los enemigos se manejan 2 listas lo que complico las cosas a la hora de dibujar el eje Y ordenadamente.
        public void Reordenar_Personajes(SpriteBatch spriteBatch)
        {
            // Genero una lista para todas las coordenadas de los personajes y las agrego
            var listaCoordenadas = new List<float>();

            // Agrego personajes y enemigos
            foreach (var jugador in Globales.Players)
            {
                listaCoordenadas.Add(jugador.GetPosition().Y);
                
                // Reseteo el estado de dibujado
                jugador.Drawn = false;
            }

            // Ordeno la lista y la invierto para obtener el efecto buscado
            listaCoordenadas.Sort();
            
            // Ahora por cada elemento de la lista ordenada comparo quien tiene el eje del primer elemento y lo dibujo
            // despues con el segundo y asi sucesivamente
            foreach (var coordenada in listaCoordenadas)
            {
                foreach (var jugador in Globales.Players)
                {
                    if (jugador.Drawn == false && jugador.GetPosition().Y == coordenada)
                    {
                        jugador.Draw(spriteBatch);

                        // Lo pongo como dibujado para que no lo repita
                        jugador.Drawn = true;
                    }
                }
            }
        }

        // Obtiene los gamepads y teclado que se tocaron y se estan tocando
        public void Input_Management()
        {
            // Guarda los estados anteriores del joystick y del teclado
            Globales.PreviousKeyboardState = Globales.CurrentKeyboardState;
            
            //for (int i = 0; i < 4;i++ )
            //{
            //    Variables_Generales.previousGamePadState[i] = Variables_Generales.currentGamePadState[i];
            //    Variables_Generales.currentGamePadState[i] = GamePad.GetState((PlayerIndex)i);
            //}

        }
    }
}
