using System;
using Hunting_Lord_Garzul.Generales;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Hunting_Lord_Garzul.Abstractos.Estados
{
    class EstadoSeleccion : Estados
    {
        // El area de la linea de sprite que queremos mostrar
        Rectangle _sourceRect;
        readonly Rectangle[] _fichas = new Rectangle[4];

        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        public override void Load(Viewport viewport)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            // Agarro el cuadro correcto
            _sourceRect = new Rectangle(0, 0, Globales.ViewportWidth, 
                Globales.ViewportHeight);

            _fichas[0] = new Rectangle(0, 0, 200, 150);
            _fichas[1] = new Rectangle(200, 0, 200, 150);
            _fichas[2] = new Rectangle(400, 0, 200, 150);
            _fichas[3] = new Rectangle(600, 0, 200, 150);

            // Guarda y lee los estados actuales y anteriores del joystick y teclado
            Input_Management();

            // Actualiza el estado del juego
            UpdateState(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Globales.PantallaSeleccion, _sourceRect, Color.White);
            //spriteBatch.Draw(Variables_Generales.Selector, new Vector2(0,0), Fichas[0], Color.White);
            //spriteBatch.Draw(Variables_Generales.Selector, new Vector2(200,0), Fichas[1], Color.White);
            //spriteBatch.Draw(Variables_Generales.Selector, new Vector2(400,0), Fichas[2], Color.White);
            //spriteBatch.Draw(Variables_Generales.Selector, new Vector2(600,0), Fichas[3], Color.White);
            spriteBatch.End();
        }

        public override void UpdateState(GameTime gameTime)
        {
            //if (Variables_Generales.currentGamePadState[0].Buttons.B == ButtonState.Pressed)
            //{
            //    Variables_Generales.Estado_Actual.Estado_ejecutandose = Variables_Generales.EstadosJuego.AVANCE;
            //}

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                Globales.CurrentState.EstadoEjecutandose = Globales.EstadosJuego.Avance;
            }
        }
    }
}
