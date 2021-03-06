using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Hunting_Lord_Garzul.Objetos
{
    class Estado_Seleccion : Estados
    {
        // El area de la linea de sprite que queremos mostrar
        Rectangle sourceRect;
        Rectangle[] Fichas = new Rectangle[4];

        int Var_AltoNivel = Globales.ViewportHeight;
        int Var_AnchoNivel = Globales.ViewportWidth;
        
        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        public override void Load(Viewport _viewport)
        {
            
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            // Agarro el cuadro correcto
            sourceRect = new Rectangle(0, 0, Globales.ViewportWidth, 
                Globales.ViewportHeight);

            Fichas[0] = new Rectangle(0, 0, 200, 150);
            Fichas[1] = new Rectangle(200, 0, 200, 150);
            Fichas[2] = new Rectangle(400, 0, 200, 150);
            Fichas[3] = new Rectangle(600, 0, 200, 150);

            // Guarda y lee los estados actuales y anteriores del joystick y teclado
            Input_Management();

            // Actualiza el estado del juego
            UpdateState(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Globales.Pantalla_Seleccion, sourceRect, Color.White);
            //spriteBatch.Draw(Variables_Generales.Selector, new Vector2(0,0), Fichas[0], Color.White);
            //spriteBatch.Draw(Variables_Generales.Selector, new Vector2(200,0), Fichas[1], Color.White);
            //spriteBatch.Draw(Variables_Generales.Selector, new Vector2(400,0), Fichas[2], Color.White);
            //spriteBatch.Draw(Variables_Generales.Selector, new Vector2(600,0), Fichas[3], Color.White);
            spriteBatch.End();
        }

        public override void UpdateState(Microsoft.Xna.Framework.GameTime gameTime)
        {
            //if (Variables_Generales.currentGamePadState[0].Buttons.B == ButtonState.Pressed)
            //{
            //    Variables_Generales.Estado_Actual.Estado_ejecutandose = Variables_Generales.EstadosJuego.AVANCE;
            //}

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                Globales.CurrentState.Estado_ejecutandose = Globales.EstadosJuego.AVANCE;
            }
        }
    }
}
