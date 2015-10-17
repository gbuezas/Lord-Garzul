using System;
using Hunting_Lord_Garzul.Generales;
using Hunting_Lord_Garzul.Objetos;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Hunting_Lord_Garzul.Abstractos.Estados
{
    class EstadoTitulos : Estados
    {
        // El area de la linea de sprite que queremos mostrar
        Rectangle _sourceRect;

        readonly int _varAltoNivel = Globales.ViewportHeight;
        readonly int _varAnchoNivel = Globales.ViewportWidth;

        // Traigo la camara del game
        Camera _camaraTraida;

        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        public override void Load(Viewport viewport)
        {
            _camaraTraida = new Camera(viewport, _varAltoNivel, _varAnchoNivel);
        }

        public override void Update(GameTime gameTime)
        {
            // Agarro el cuadro correcto
            _sourceRect = new Rectangle(0, 0, Globales.ViewportWidth, 
                Globales.ViewportHeight);

            // Guarda y lee los estados actuales y anteriores del joystick y teclado
            Input_Management();

            // Actualiza el estado del juego
            UpdateState(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Globales.PantallaTitulo, _sourceRect, Color.White);
            spriteBatch.End();
        }

        public override void UpdateState(GameTime gameTime)
        {
         
            //if (Variables_Generales.currentGamePadState[0].Buttons.A == ButtonState.Pressed)
            //{
            //    Variables_Generales.Estado_Actual.Estado_ejecutandose = Variables_Generales.EstadosJuego.SELECCION;
            //}

            if ((Keyboard.GetState().IsKeyDown(Keys.A)))
            {
                Globales.CurrentState.EstadoEjecutandose = Globales.EstadosJuego.Seleccion;
            }
            
        }
    }
}
