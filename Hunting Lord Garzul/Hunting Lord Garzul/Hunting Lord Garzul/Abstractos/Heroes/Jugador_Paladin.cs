using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Hunting_Lord_Garzul.Objetos;

namespace Hunting_Lord_Garzul
{
    class Jugador_Paladin : Jugadores
    {

        # region VARIABLES

        // Accion que se realiza
        private Variables_Generales.Acciones Accion_Actual;
        public Variables_Generales.Acciones accion_actual
        {
            get { return Accion_Actual; }
            set { Accion_Actual = value; }
        }
        // Accion realizada anteriormente
        private Variables_Generales.Acciones Accion_Anterior;
        public Variables_Generales.Acciones accion_anterior
        {
            get { return Accion_Anterior; }
            set { Accion_Anterior = value; }
        }
        
        // Mirada del personaje
        protected Objetos.Variables_Generales.Mirada Direccion;

        // Tipo de cada pieza de armadura
        protected int helm_type;
        protected int greaves_type;
        protected int gauntlets_type;
        protected int breastplate_type;
        protected int tasset_type;
        protected int shield_type;
        protected int sword_type;
        
        // Animacion de cada pieza de armadura
        //protected Animacion shield_anim;
        protected Animacion gauntlet_back_anim;
        protected Animacion greave_back_anim;
        protected Animacion helm_anim;
        protected Animacion breastplate_anim;
        protected Animacion tasset_anim;
        protected Animacion greave_top_anim;
        //protected Animacion sword_anim;
        protected Animacion gauntlet_top_anim;
        
        // Posicion del jugador relativa a la parte superior izquierda de la pantalla.
        // Esta posicion marca donde se encuentra el jugador en la pantalla y no en el mapa donde se esta moviendo,
        // ya que a esta posicion se le aplican los limites de la pantalla. 
        protected Vector2 Position;
        public override Vector2 Posicion()
        {
            return Position;
        }

        // Si esta activa esta instancia o no
        protected bool Active;
        
        // Puntos de vida
        protected int Health;
        
        // Ancho de un cuadro del sprite
        //protected int AnchoPersonaje = 320;
        protected int AnchoPersonaje = 200;
        // Alto de un cuadro del sprite
        //protected int AltoPersonaje = 320;
        protected int AltoPersonaje = 600;
        
        // Velocidad de movimiento del jugador
        protected float VelocidadPersonaje;
        
        // Controladores de tiempo de botones y acciones
        //protected TimeSpan A_Button_Halt;
        //protected TimeSpan X_Button_Halt;
        //protected TimeSpan Tiempo_Golpe_1;

        // Establece tiempo de frame inicial cuando llama al UpdateArmor
        // El UpdateArmor no ocurre en el loop se pide explicitamente
        protected int Tiempo_Frame;

        // Para teclado o Dpad
        //protected bool reposo;
        //protected bool derecha;

        // Teclado del jugador
        //protected KeyboardState Teclado_Jugador;

        // Mensajes de datos
        protected float mensaje1;
        protected float mensaje2;
        protected Variables_Generales.Mirada mensaje3;
        protected Variables_Generales.Acciones mensaje4;
        //protected float mensaje5;

        // Donde se va a alojar el mensaje de chequeo de status
        Vector2 mensaje;
        
        #endregion

        // Inicializar al jugador
        public override void Initialize(Vector2 posicion)
        {
            
            // Posicion al comenzar
            Position = posicion;

            mensaje = Position;
            
            // Activar jugador
            Active = true;
            
            // Vida del jugador
            Health = 100;
            
            // Establecer velocidad
            VelocidadPersonaje = 3.0f;
            
            // Establece la mirada
            Direccion = Objetos.Variables_Generales.Mirada.DERECHA;
            
            // Establece el estado
            accion_actual = Variables_Generales.Acciones.STAND;
            accion_anterior = accion_actual;

            // Control del personaje
            //halt = false;

            // Tiempo default del frame
            Tiempo_Frame = 50;

            // Piezas de la armadura al comenzar
            UpdateArmor("set-1", "set-1", "set-1", "set-1", "set-1", "set-1", "set-1", accion_actual);

            // Asigno control por default al jugador
            controles[(int)Variables_Generales.Controles.ARRIBA] = Keys.W;
            controles[(int)Variables_Generales.Controles.ABAJO] = Keys.S;
            controles[(int)Variables_Generales.Controles.IZQUIERDA] = Keys.A;
            controles[(int)Variables_Generales.Controles.DERECHA] = Keys.D;
            controles[(int)Variables_Generales.Controles.BOTON_1] = Keys.T;
            controles[(int)Variables_Generales.Controles.BOTON_2] = Keys.Y;      
        }

        // Actualizar animacion
        public override void Update(GameTime gameTime)
        {
            //shield_anim.Position = Position;
            //shield_anim.Update(gameTime);

            gauntlet_back_anim.Position = Position;
            gauntlet_back_anim.Update(gameTime);

            greave_back_anim.Position = Position;
            greave_back_anim.Update(gameTime);

            helm_anim.Position = Position;
            helm_anim.Update(gameTime);

            breastplate_anim.Position = Position;
            breastplate_anim.Update(gameTime);

            tasset_anim.Position = Position;
            tasset_anim.Update(gameTime);

            greave_top_anim.Position = Position;
            greave_top_anim.Update(gameTime);

            //sword_anim.Position = Position;
            //sword_anim.Update(gameTime);

            gauntlet_top_anim.Position = Position;
            gauntlet_top_anim.Update(gameTime);
        }

        // Dibujar Jugador
        // Algo estaa pasando que me dibuja mal el orden de las cosas
        // Chequear porque no esta dibjando la cintura como corresponde.
        public override void Draw(SpriteBatch spriteBatch)
        {
            gauntlet_back_anim.Draw(spriteBatch, Direccion);
            greave_back_anim.Draw(spriteBatch, Direccion);
            helm_anim.Draw(spriteBatch, Direccion);
            breastplate_anim.Draw(spriteBatch, Direccion);
            tasset_anim.Draw(spriteBatch, Direccion);
            greave_top_anim.Draw(spriteBatch, Direccion);
            gauntlet_top_anim.Draw(spriteBatch, Direccion);
            
            // Si no separo este proceso de dibujo desconcha las posiciones de las capas del jugador
            //spriteBatch.DrawString(Variables_Generales.CheckStatusVar_2,
            //"Frame Actual = " + mensaje1.ToString() + System.Environment.NewLine +
            //"Fila Actual = " + mensaje2.ToString() + System.Environment.NewLine +
            //"Direccion Actual = " + mensaje3.ToString() + System.Environment.NewLine +
            //"Accion Actual = " + mensaje4.ToString() + System.Environment.NewLine +
            //"Zoom = " + Variables_Generales.mensaje5.ToString(),
            //mensaje, Color.DarkRed);

        }

        /// <summary>
        /// Actualizar cosas del jugador antes del update general. Aca va todo lo que es la logica del mismo, saltar pegar, etc.
        /// </summary>
        /// <param name="gameTime">El gametime del juego.</param>
        /// <param name="currentKeyboardState">Para el teclado.</param>
        /// <param name="currentGamePadState">Para los gamepad.</param>
        /// <param name="LimitesPantalla">Los limites que puso la camara con respecto a la pantalla que vemos.</param>
        /// <param name="AltoNivel">La altura total del escenario.</param>
        /// <param name="AnchoNivel">El ancho total del escenario.</param>
        public override void UpdatePlayer(GameTime gameTime, int Jugador ,Rectangle LimitesPantalla, int AltoNivel, int AnchoNivel)
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

            #region Obtener boton X

            //if (Variables_Generales.currentGamePadState[Jugador].Buttons.X == ButtonState.Pressed 
            //    && Variables_Generales.previousGamePadState[Jugador].Buttons.X == ButtonState.Released)
            //{
            //    Accion = Variables_Generales.Acciones.GOLPE_1;
            //    Tiempo_Golpe_1 = gameTime.TotalGameTime;
            //    //halt = true;
            //}

           

            //if (Accion == Variables_Generales.Acciones.GOLPE_1 &&
            //        gameTime.TotalGameTime - Tiempo_Golpe_1 > TimeSpan.FromSeconds(.5f) &&
            //        Player_Halt == true)
            //{
            //    Player_Halt = false;
            //    //PausarAnimacion(false);
            //}

            mensaje1 = gauntlet_top_anim.CurrentFrame;
            mensaje2 = gauntlet_top_anim.FrameCount;
            mensaje3 = Direccion;
            mensaje4 = accion_actual;

            #endregion

            # region movimiento joystick
            // Movimiento del personaje
            //if(Player_Halt == false)
            //{
            //    Position.X += Variables_Generales.currentGamePadState[Jugador].ThumbSticks.Left.X * VelocidadPersonaje;
            //    Position.Y -= Variables_Generales.currentGamePadState[Jugador].ThumbSticks.Left.Y * VelocidadPersonaje;

            //    // Para que los mensajes de chequeo sigan al personaje
            //    mensaje.X = Position.X - 50;
            //    mensaje.Y = Position.Y - 150;

            //    // Si mira a la derecha o a la izquierda dependiendo de para donde movimos el stick
            //    if (Variables_Generales.currentGamePadState[Jugador].ThumbSticks.Left.X > 0.04)
            //    {
            //        Direccion = Variables_Generales.Mirada.DERECHA;
            //        Accion = Variables_Generales.Acciones.CAMINA;
            //    }
            //    else if (Variables_Generales.currentGamePadState[Jugador].ThumbSticks.Left.X < -0.04)
            //    {
            //        Direccion = Variables_Generales.Mirada.IZQUIERDA;
            //        Accion = Variables_Generales.Acciones.CAMINA;
            //    }
            //    else if (Player_Halt == false)
            //    {
            //        Accion = Variables_Generales.Acciones.QUIETO;
            //    }

            //    // Si sube o si baja
            //    if (Variables_Generales.currentGamePadState[Jugador].ThumbSticks.Left.Y > 0.04 ||
            //        Variables_Generales.currentGamePadState[Jugador].ThumbSticks.Left.Y < -0.04)
            //    {
            //        Accion = Variables_Generales.Acciones.CAMINA;
            //    }
            //}
            #endregion

            // Logica del movimiento, muy importante
            #region Obtener teclado

            Variables_Generales.currentKeyboardState = Keyboard.GetState();

            if (Variables_Generales.currentKeyboardState.IsKeyDown(controles[(int)Variables_Generales.Controles.IZQUIERDA]))
            {
                Position.X -= VelocidadPersonaje;
                Direccion = Variables_Generales.Mirada.IZQUIERDA;
                accion_actual = Variables_Generales.Acciones.WALK;
            }
            else if (Variables_Generales.currentKeyboardState.IsKeyDown(controles[(int)Variables_Generales.Controles.DERECHA]))
            {
                Position.X += VelocidadPersonaje;
                Direccion = Variables_Generales.Mirada.DERECHA;
                accion_actual = Variables_Generales.Acciones.WALK;
            }
            else if (Variables_Generales.currentKeyboardState.IsKeyDown(controles[(int)Variables_Generales.Controles.ARRIBA]))
            {
                Position.Y -= VelocidadPersonaje;
                accion_actual = Variables_Generales.Acciones.WALK;
            }
            else if (Variables_Generales.currentKeyboardState.IsKeyDown(controles[(int)Variables_Generales.Controles.ABAJO]))
            {
                Position.Y += VelocidadPersonaje;
                accion_actual = Variables_Generales.Acciones.WALK;
            }
            else
            {
                accion_actual = Variables_Generales.Acciones.STAND;
            }

            #endregion

            // Hacer que el jugador no salga de la pantalla. 
            // Tomamos el rectangulo que genera la camara para acomodar al jugador.
            Position.X = MathHelper.Clamp(Position.X, LimitesPantalla.Left + AnchoPersonaje / 2, LimitesPantalla.Width - AnchoPersonaje / 2);
            //Position.Y = MathHelper.Clamp(Position.Y, AltoNivel * 0.50f, AltoNivel - AltoPersonaje / 2);
            Position.Y = MathHelper.Clamp(Position.Y, 3*AltoNivel/5, AltoNivel);
            
            // Acomoda la cantidad de frames y la fila del sprite de acuerdo a la accion ejecuntandose
            #region Acomoda la cantidad de frames de la animacion correspondiente y la fila

            switch (accion_actual)
            {
                case Variables_Generales.Acciones.STAND:
                    {
                        foreach (Texturas textura_actual in Variables_Generales.TexturasPaladin)
                        {
                            if (textura_actual.nombre_textura == "set-1_stand_gauntlet-top")
                            {
                                gauntlet_top_anim.spriteStrip = textura_actual.textura;
                            }
                            if (textura_actual.nombre_textura == "set-1_stand_gauntlet-back")
                            {
                                gauntlet_back_anim.spriteStrip = textura_actual.textura;
                            }
                            if (textura_actual.nombre_textura == "set-1_stand_greave-top")
                            {
                                greave_top_anim.spriteStrip = textura_actual.textura;
                            }
                            if (textura_actual.nombre_textura == "set-1_stand_greave-back")
                            {
                                greave_back_anim.spriteStrip = textura_actual.textura;
                            }
                            if (textura_actual.nombre_textura == "set-1_stand_tasset")
                            {
                                tasset_anim.spriteStrip = textura_actual.textura;
                            }
                            if (textura_actual.nombre_textura == "set-1_stand_breastplate")
                            {
                                breastplate_anim.spriteStrip = textura_actual.textura;
                            }
                            if (textura_actual.nombre_textura == "set-1_stand_helm")
                            {
                                helm_anim.spriteStrip = textura_actual.textura;
                            }
                        }
                        
                        PausarAnimacion(false);
                        break;
                    }

                case Variables_Generales.Acciones.WALK:
                    {
                        foreach (Texturas textura_actual in Variables_Generales.TexturasPaladin)
                        {
                            if (textura_actual.nombre_textura == "set-1_walk_gauntlet-top")
                            {
                                gauntlet_top_anim.spriteStrip = textura_actual.textura;
                            }
                            if (textura_actual.nombre_textura == "set-1_walk_gauntlet-back")
                            {
                                gauntlet_back_anim.spriteStrip = textura_actual.textura;
                            }
                            if (textura_actual.nombre_textura == "set-1_walk_greave-top")
                            {
                                greave_top_anim.spriteStrip = textura_actual.textura;
                            }
                            if (textura_actual.nombre_textura == "set-1_walk_greave-back")
                            {
                                greave_back_anim.spriteStrip = textura_actual.textura;
                            }
                            if (textura_actual.nombre_textura == "set-1_walk_tasset")
                            {
                                tasset_anim.spriteStrip = textura_actual.textura;
                            }
                            if (textura_actual.nombre_textura == "set-1_walk_breastplate")
                            {
                                breastplate_anim.spriteStrip = textura_actual.textura;
                            }
                            if (textura_actual.nombre_textura == "set-1_walk_helm")
                            {
                                helm_anim.spriteStrip = textura_actual.textura;
                            }
                        }

                        PausarAnimacion(false);
                        break;
                    }

                case Variables_Generales.Acciones.HIT_1:
                    {
                        break;
                    }

                default:break;
            }
            
            // Vuelve a 0 el frame de la animacion si cambio de accion
            if (accion_anterior != accion_actual)
            {
                greave_back_anim.CurrentFrame = 0;
                greave_top_anim.CurrentFrame = 0;
                helm_anim.CurrentFrame = 0;
                breastplate_anim.CurrentFrame = 0;
                gauntlet_back_anim.CurrentFrame = 0;
                gauntlet_top_anim.CurrentFrame = 0;
                tasset_anim.CurrentFrame = 0;
                //sword_anim.CurrentFrame = 0;
                //shield_anim.CurrentFrame = 0;

                accion_anterior = accion_actual;
            }

            #endregion
        }

        /// <summary>
        /// Actualiza las piezas del personaje. Se le pasa el nombre de cada pieza que va a estar alojada en el objeto de la misma
        /// y despues se busca ese nombre de la lista de texturas y se actualiza
        /// </summary>
        /// <param name="helm">Establece el casco del jugador.</param>
        /// <param name="gauntlets">Establece los brazos del jugador.</param>
        /// <param name="breastplate">Establece el torso del jugador.</param>
        /// <param name="greaves">Establece las piernas y pies del jugador.</param>
        /// <param name="shield">Establece el escudo del jugador.</param>
        /// <param name="sword">Establece el arma del jugador.</param>
        /// <param name="tasset">Establece la cadera del jugador.</param>
        /// <param name="accion_actual">Establece la accion del jugador y sus frames</param>
        public void UpdateArmor(String helm, String gauntlets, String breastplate, String greaves, String shield, String sword, String tasset, Variables_Generales.Acciones accion_actual)
        {
            // Restablezco las variables
            gauntlets_type = Variables_Generales.Tipo_Armadura.IndexOf(gauntlets);
            helm_type = Variables_Generales.Tipo_Armadura.IndexOf(helm);
            greaves_type = Variables_Generales.Tipo_Armadura.IndexOf(greaves);
            breastplate_type = Variables_Generales.Tipo_Armadura.IndexOf(breastplate);
            shield_type = Variables_Generales.Tipo_Armadura.IndexOf(shield);
            sword_type = Variables_Generales.Tipo_Armadura.IndexOf(sword);
            tasset_type = Variables_Generales.Tipo_Armadura.IndexOf(tasset);
            
            // Restlabezco las animaciones
            foreach (Texturas textura in Objetos.Variables_Generales.TexturasPaladin)
            {
                if (textura.pieza == "gauntlet-back" && 
                    textura.accion == accion_actual.ToString().ToLower() &&
                    textura.armadura == (Variables_Generales.Tipo_Armadura[gauntlets_type]))
                {
                    gauntlet_back_anim = new Animacion();
                    gauntlet_back_anim.Initialize(textura.textura, Position, AnchoPersonaje, AltoPersonaje, (int)accion_actual, Tiempo_Frame, Color.White, true);
                }

                if (textura.pieza == "greave-back" &&
                    textura.accion == accion_actual.ToString().ToLower() &&
                    textura.armadura == Variables_Generales.Tipo_Armadura[greaves_type])
                {
                    greave_back_anim = new Animacion();
                    greave_back_anim.Initialize(textura.textura, Position, AnchoPersonaje, AltoPersonaje, (int)accion_actual, Tiempo_Frame, Color.White, true);
                }
                
                if (textura.pieza == "helm" &&
                   textura.accion == accion_actual.ToString().ToLower() && 
                   textura.armadura == Variables_Generales.Tipo_Armadura[helm_type])
                {
                    helm_anim = new Animacion();
                    helm_anim.Initialize(textura.textura, Position, AnchoPersonaje, AltoPersonaje, (int)accion_actual, Tiempo_Frame, Color.White, true);
                }
                
                if (textura.pieza == "breastplate" &&
                    textura.accion == accion_actual.ToString().ToLower() && 
                    textura.armadura == Variables_Generales.Tipo_Armadura[breastplate_type])
                {
                    breastplate_anim = new Animacion();
                    breastplate_anim.Initialize(textura.textura, Position, AnchoPersonaje, AltoPersonaje, (int)accion_actual, Tiempo_Frame, Color.White, true);
                }

                if (textura.pieza == "tasset" &&
                    textura.accion == accion_actual.ToString().ToLower() && 
                    textura.armadura == Variables_Generales.Tipo_Armadura[tasset_type])
                {
                    tasset_anim = new Animacion();
                    tasset_anim.Initialize(textura.textura, Position, AnchoPersonaje, AltoPersonaje, (int)accion_actual, Tiempo_Frame, Color.White, true);
                }

                if (textura.pieza == "greave-top" &&
                    textura.accion == accion_actual.ToString().ToLower() &&
                    textura.armadura == Variables_Generales.Tipo_Armadura[greaves_type])
                {
                    greave_top_anim = new Animacion();
                    greave_top_anim.Initialize(textura.textura, Position, AnchoPersonaje, AltoPersonaje, (int)accion_actual, Tiempo_Frame, Color.White, true);
                }

                if (textura.pieza == "gauntlet-top" &&
                    textura.accion == accion_actual.ToString().ToLower() &&
                    textura.armadura == (Variables_Generales.Tipo_Armadura[gauntlets_type]))
                {
                    gauntlet_top_anim = new Animacion();
                    gauntlet_top_anim.Initialize(textura.textura, Position, AnchoPersonaje, AltoPersonaje, (int)accion_actual, Tiempo_Frame, Color.White, true);
                }
            }
        }

        /// <summary>
        /// Establece el tiempo de frame en ejecucion
        /// </summary>
        /// <param name="Tiempo">El tiempo que va a durar el frame en pantalla de las distintas animaciones del personaje</param>
        void TiempoFrameEjecucion(int Tiempo)
        {
            gauntlet_back_anim.frameTime = Tiempo;
            greave_back_anim.frameTime = Tiempo;
            helm_anim.frameTime = Tiempo;
            breastplate_anim.frameTime = Tiempo;
            tasset_anim.frameTime = Tiempo;
            greave_top_anim.frameTime = Tiempo;
            gauntlet_top_anim.frameTime = Tiempo;
        }

        /// <summary>
        /// Pausa la animacion en el frame actual
        /// </summary>
        /// <param name="desactivar">pone o quita la pausa segun este parametro</param>
        void PausarAnimacion(bool desactivar)
        {
            gauntlet_back_anim.pausa = desactivar;
            greave_back_anim.pausa = desactivar;
            helm_anim.pausa = desactivar;
            breastplate_anim.pausa = desactivar;
            tasset_anim.pausa = desactivar;
            greave_top_anim.pausa = desactivar;
            gauntlet_top_anim.pausa = desactivar;
        }

    }
}
