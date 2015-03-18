using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Hunting_Lord_Garzul.Objetos;

namespace Hunting_Lord_Garzul
{
    class Jugador_Barbaro : Jugadores
    {

        # region VARIABLES

        // Variable que dice que accion esta haciendo en esta instancia
        Variables_Generales.Acciones Accion;
        // Guardo el viejo estado para resetear los cuadros de frame cuando cambie de estado
        Variables_Generales.Acciones AccionAnterior;

        // Variable que dice para donde mira
        protected Objetos.Variables_Generales.Mirada Direccion;

        // Variable que dice que tipo de armadura corresponde a cada pieza en esta instancia
        int Tipo_Armadura_Cabeza;
        int Tipo_Armadura_Pies;
        int Tipo_Armadura_Brazos;
        int Tipo_Armadura_Torso;
        int Tipo_Armadura_Escudo;
        int Tipo_Armadura_Espada;

        // Animacion representando al jugador en esta instancia
        public Animacion brazo_atras_animacion;
        public Animacion cabeza_animacion;
        public Animacion pies_animacion;
        public Animacion pecho_animacion;
        public Animacion brazo_adelante_animacion;

        // Posicion del jugador relativa a la parte superior izquierda de la pantalla
        protected Vector2 Position;
        public override Vector2 Posicion()
        {
            return Position;
        }

        // Esta verificacion sirve para el reordenamiento de los personajes sobre el eje Y
        public bool Dibujado = false;

        // Si esta activa esta instancia o no
        public bool Active;

        // Puntos de vida
        public int Health;

        // Ancho de un cuadro del sprite
        public int FrameWidth = 320;
        // Alto de un cuadro del sprite
        public int FrameHeight = 320;

        // Velocidad de movimiento del jugador
        protected float VelocidadPersonaje;
        
        // Para que no se pueda apretar ni mantener el boton
        TimeSpan A_Button_Halt;

        #endregion

        // Inicializar al jugador
        public override void Initialize(Vector2 posicion)
        {

            // Posicion al comenzar
            Position = posicion;

            // Activar jugador
            Active = true;

            // Vida del jugador
            Health = 100;

            // Establecer velocidad
            VelocidadPersonaje = 3.0f;

            // Establece la mirada
            Direccion = Objetos.Variables_Generales.Mirada.DERECHA;

            // Establece el estado
            Accion = Variables_Generales.Acciones.STAND;
            AccionAnterior = Accion;

            // Piezas de la armadura al comenzar
            UpdateArmor("ArmaduraInicial", "ArmaduraInicial", "ArmaduraInicial",
                "ArmaduraInicial", "ArmaduraInicial", "ArmaduraInicial");

        }

        // Actualizar animacion
        public override void Update(GameTime gameTime)
        {
            brazo_atras_animacion.Position = Position;
            brazo_atras_animacion.Update(gameTime);
            cabeza_animacion.Position = Position;
            cabeza_animacion.Update(gameTime);
            pies_animacion.Position = Position;
            pies_animacion.Update(gameTime);
            pecho_animacion.Position = Position;
            pecho_animacion.Update(gameTime);
            brazo_adelante_animacion.Position = Position;
            brazo_adelante_animacion.Update(gameTime);
        }

        // Dibujar Jugador
        public override void Draw(SpriteBatch spriteBatch)
        {

            //brazo_atras_animacion.Draw(spriteBatch, Direccion);
            //pies_animacion.Draw(spriteBatch, Direccion);
            //pecho_animacion.Draw(spriteBatch, Direccion);
            //cabeza_animacion.Draw(spriteBatch, Direccion);
            //brazo_adelante_animacion.Draw(spriteBatch, Direccion);

        }

        // Actualizar cosas del jugador antes del update general. Aca va todo lo que es la logica del mismo, saltar pegar, etc.
        public override void UpdatePlayer(  GameTime gameTime,
                                            //KeyboardState currentKeyboardState,
                                            //GamePadState currentGamePadState, 
                                            int Jugador,
                                            Rectangle LimitesJugador,
                                            int AltoNivel, 
                                            int AnchoNivel)
        {

            Update(gameTime);

            #region Obtener boton A
            //if (Variables_Generales.currentGamePadState[Jugador].Buttons.A == ButtonState.Pressed)
            //{

            //    if (Variables_Generales.Tipo_Armadura[Tipo_Armadura_Torso] == "ArmaduraInicial" &&
            //        gameTime.TotalGameTime - A_Button_Halt > TimeSpan.FromSeconds(.50f))
            //    {
            //        UpdateArmor("ArmaduraInicial", "ArmaduraInicial", "ArmaduraVerde", "ArmaduraInicial",
            //            "ArmaduraInicial", "ArmaduraInicial");
            //        A_Button_Halt = gameTime.TotalGameTime;
            //    }
            //    else if (Variables_Generales.Tipo_Armadura[Tipo_Armadura_Torso] == "ArmaduraVerde" &&
            //        gameTime.TotalGameTime - A_Button_Halt > TimeSpan.FromSeconds(.50f))
            //    {
            //        UpdateArmor("ArmaduraInicial", "ArmaduraInicial", "ArmaduraInicial", "ArmaduraInicial",
            //            "ArmaduraInicial", "ArmaduraInicial");
            //        A_Button_Halt = gameTime.TotalGameTime;
            //    }
            //}
            #endregion

            // Obtener analogico del joystick para ubicar en el mapa y la pantalla
            //Position.X += Variables_Generales.currentGamePadState[Jugador].ThumbSticks.Left.X * VelocidadPersonaje;
            //Position.Y -= Variables_Generales.currentGamePadState[Jugador].ThumbSticks.Left.Y * VelocidadPersonaje;
            //Location.X -= currentGamePadState.ThumbSticks.Left.X * playerMoveSpeed;
            //Location.Y += currentGamePadState.ThumbSticks.Left.Y * playerMoveSpeed;

            // Si mira a la derecha o a la izquierda dependiendo de para donde movimos el stick
            //if (Variables_Generales.currentGamePadState[Jugador].ThumbSticks.Left.X > 0.04)
            //{
            //    Direccion = Variables_Generales.Mirada.DERECHA;
            //    Accion = Variables_Generales.Acciones.CAMINA;
            //}
            //else if (Variables_Generales.currentGamePadState[Jugador].ThumbSticks.Left.X < -0.04)
            //{
            //    Direccion = Variables_Generales.Mirada.IZQUIERDA;
            //    Accion = Variables_Generales.Acciones.CAMINA;
            //}
            //else
            //{
            //    Accion = Variables_Generales.Acciones.QUIETO;
            //}

            //if (Variables_Generales.currentGamePadState[Jugador].ThumbSticks.Left.Y > 0.04 ||
            //    Variables_Generales.currentGamePadState[Jugador].ThumbSticks.Left.Y < -0.04)
            //{
            //    Accion = Variables_Generales.Acciones.CAMINA;
            //}

            #region Obtener teclado

            Variables_Generales.currentKeyboardState = Keyboard.GetState();

            if (Variables_Generales.currentKeyboardState.IsKeyDown(controles[(int)Variables_Generales.Controles.IZQUIERDA]))
            {
                Position.X -= VelocidadPersonaje;
                Direccion = Variables_Generales.Mirada.IZQUIERDA;
                Accion = Variables_Generales.Acciones.WALK;
            }
            else if (Variables_Generales.currentKeyboardState.IsKeyDown(controles[(int)Variables_Generales.Controles.DERECHA]))
            {
                Position.X += VelocidadPersonaje;
                Direccion = Variables_Generales.Mirada.DERECHA;
                Accion = Variables_Generales.Acciones.WALK;
            }

            if (Variables_Generales.currentKeyboardState.IsKeyDown(controles[(int)Variables_Generales.Controles.ARRIBA]))
            {
                Position.Y -= VelocidadPersonaje;
                Accion = Variables_Generales.Acciones.WALK;
            }
            else if (Variables_Generales.currentKeyboardState.IsKeyDown(controles[(int)Variables_Generales.Controles.ABAJO]))
            {
                Position.Y += VelocidadPersonaje;
                Accion = Variables_Generales.Acciones.WALK;
            }

            #endregion

            // Hacer que el jugador no salga de la pantalla
            Position.X = MathHelper.Clamp(Position.X, FrameHeight / 2,
                Variables_Generales.AnchoViewport / 3 * 5 - FrameHeight / 2);

            Position.Y = MathHelper.Clamp(Position.Y, Variables_Generales.AltoViewport / 3,
                Variables_Generales.AltoViewport - FrameHeight / 2);

            // Acomoda la cantidad de frames de la animacion correspondiente y la fila
            if (Accion == Variables_Generales.Acciones.STAND)
            {
                pies_animacion.FrameCount = 6;
                //pies_animacion.Fila = FrameHeight;

                cabeza_animacion.FrameCount = 6;
                //cabeza_animacion.Fila = FrameHeight;

                pecho_animacion.FrameCount = 6;
                //pecho_animacion.Fila = FrameHeight;

                brazo_adelante_animacion.FrameCount = 6;
                //brazo_adelante_animacion.Fila = FrameHeight;

                brazo_atras_animacion.FrameCount = 6;
                //brazo_atras_animacion.Fila = FrameHeight;

            }
            else
            {
                pies_animacion.FrameCount = 12;
                //pies_animacion.Fila = 0;

                cabeza_animacion.FrameCount = 12;
                //cabeza_animacion.Fila = 0;

                pecho_animacion.FrameCount = 12;
                //pecho_animacion.Fila = 0;

                brazo_adelante_animacion.FrameCount = 12;
                //brazo_adelante_animacion.Fila = 0;

                brazo_atras_animacion.FrameCount = 12;
                //brazo_atras_animacion.Fila = 0;
            }

            // Vuelve a 0 el frame de la animacion si cambio de accion
            if (AccionAnterior != Accion)
            {
                brazo_adelante_animacion.CurrentFrame = 0;
                brazo_atras_animacion.CurrentFrame = 0;
                pecho_animacion.CurrentFrame = 0;
                cabeza_animacion.CurrentFrame = 0;
                pies_animacion.CurrentFrame = 0;
                AccionAnterior = Accion;
            }
        }

        /// <summary>
        /// Actualiza las piezas del personaje. Se le pasa el nombre de cada pieza que va a estar alojada en el objeto de la misma
        /// y despues se busca ese nombre de la lista de texturas y se actualiza
        /// </summary>
        /// <param name="Cabeza">Establece el casco del jugador.</param>
        /// <param name="Brazos">Establece los brazos del jugador.</param>
        /// <param name="Torso">Establece el torso del jugador.</param>
        /// <param name="Pies">Establece las piernas y pies del jugador.</param>
        /// <param name="Escudo">Establece el escudo del jugador.</param>
        /// <param name="Espada">Establece el arma del jugador.</param>
        public void UpdateArmor(String Cabeza, String Brazos, String Torso, String Pies, String Escudo, String Espada)
        {
            // Restablezco las variables
            Tipo_Armadura_Brazos = Variables_Generales.Tipo_Armadura.IndexOf(Brazos);
            Tipo_Armadura_Cabeza = Variables_Generales.Tipo_Armadura.IndexOf(Cabeza);
            Tipo_Armadura_Pies = Variables_Generales.Tipo_Armadura.IndexOf(Pies);
            Tipo_Armadura_Torso = Variables_Generales.Tipo_Armadura.IndexOf(Torso);
            Tipo_Armadura_Escudo = Variables_Generales.Tipo_Armadura.IndexOf(Escudo);
            Tipo_Armadura_Espada = Variables_Generales.Tipo_Armadura.IndexOf(Espada);

            // Restlabezco las animaciones
            foreach (Texturas item in Objetos.Variables_Generales.TexturasBarbaro)
            {
                if (item.nombre_textura.Contains("BrazoAtras") &&
                    item.nombre_textura.Contains(Variables_Generales.Tipo_Armadura[Tipo_Armadura_Brazos]))
                {
                    brazo_atras_animacion = new Animacion();
                    brazo_atras_animacion.Initialize(item.textura, Position, FrameWidth, FrameHeight, 12, 40, Color.White, true);
                }
            }

            foreach (Texturas item in Objetos.Variables_Generales.TexturasBarbaro)
            {
                if (item.nombre_textura.Contains("Pies") &&
                    item.nombre_textura.Contains(Variables_Generales.Tipo_Armadura[Tipo_Armadura_Pies]))
                {
                    pies_animacion = new Animacion();
                    pies_animacion.Initialize(item.textura, Position, FrameWidth, FrameHeight, 12, 80, Color.White, true);
                }
            }

            foreach (Texturas item in Objetos.Variables_Generales.TexturasBarbaro)
            {
                if (item.nombre_textura.Contains("Torso") &&
                    item.nombre_textura.Contains(Variables_Generales.Tipo_Armadura[Tipo_Armadura_Torso]))
                {
                    pecho_animacion = new Animacion();
                    pecho_animacion.Initialize(item.textura, Position, FrameWidth, FrameHeight, 12, 80, Color.White, true);
                }
            }

            foreach (Texturas item in Objetos.Variables_Generales.TexturasBarbaro)
            {
                if (item.nombre_textura.Contains("Cabeza") &&
                    item.nombre_textura.Contains(Variables_Generales.Tipo_Armadura[Tipo_Armadura_Cabeza]))
                {
                    cabeza_animacion = new Animacion();
                    cabeza_animacion.Initialize(item.textura, Position, FrameWidth, FrameHeight, 12, 80, Color.White, true);
                }
            }

            foreach (Texturas item in Objetos.Variables_Generales.TexturasBarbaro)
            {
                if (item.nombre_textura.Contains("BrazoAdelante") &&
                    item.nombre_textura.Contains(Variables_Generales.Tipo_Armadura[Tipo_Armadura_Brazos]))
                {
                    brazo_adelante_animacion = new Animacion();
                    brazo_adelante_animacion.Initialize(item.textura, Position, FrameWidth, FrameHeight, 12, 80, Color.White, true);
                }
            }

            

        }
    }
}
