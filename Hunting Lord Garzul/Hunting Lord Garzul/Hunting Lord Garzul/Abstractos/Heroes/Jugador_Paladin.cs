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
        private Globales.Actions AccionActual;
        public Globales.Actions accionActual
        {
            get { return AccionActual; }
            set { AccionActual = value; }
        }
       
        // Accion realizada anteriormente
        private Globales.Actions AccionAnterior;
        public Globales.Actions accionAnterior
        {
            get { return AccionAnterior; }
            set { AccionAnterior = value; }
        }
        
        // Mirada del personaje
        protected Objetos.Globales.Mirada Direccion;

        // Tipo de cada pieza de armadura
        // shield, gauntletback, greaveback, helm, breastplate, tasset, greavetop, sword, gauntlettop.
        protected Pieces_Sets pieces_armor = new Pieces_Sets();
        protected List<Piece_Set> pieces_armor_new = new List<Piece_Set>();

        // Animacion de cada pieza de armadura
        // 1.shield
        // 2.gauntletback
        // 3.greaveback
        // 4.helm
        // 5.breastplate
        // 6.tasset
        // 7.greavetop
        // 8.sword
        // 9.gauntlettop
        protected Animacion[] pieces_anim = new Animacion[9];
        
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
        
        // Ancho de un cuadro del sprite
        protected int AnchoPersonaje = 320;
        //protected int AnchoPersonaje = 200;
        // Alto de un cuadro del sprite
        //protected int AltoPersonaje = 320;
        protected int AltoPersonaje = 600;
        
        // Velocidad de movimiento del jugador
        protected float VelocidadPersonaje;
        
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
        protected Globales.Mirada mensaje3;
        protected Globales.Actions mensaje4;
        
        // Donde se va a alojar el mensaje de chequeo de status
        Vector2 mensaje;
        
        #endregion

        #region METODOS

        // Inicializar al jugador
        public override void Initialize(Vector2 posicion)
        {
            
            // Establezco variables por default para comenzar
            Position = posicion;
            mensaje = Position;
            Active = true;
            VelocidadPersonaje = 3.0f;
            Direccion = Objetos.Globales.Mirada.DERECHA;
            accionActual = Globales.Actions.STAND;
            accionAnterior = accionActual;
            Tiempo_Frame = 50;

            // Inicializo partes de armadura actual
            pieces_armor.Initialize();
            
            // Inicializo las piezas de animacion
            for (int i = 0; i < Globales.Piezas.Length; i++ )
            {
                pieces_anim[i] = new Animacion();
                pieces_anim[i].Initialize(Globales.Piezas[i]);
            }

            #region cambiar armadura (solo para probar, borrar mas tarde)
            // Coloco un recambio de armadura, que en el juego orginal esto tiene que pasar al obtener armaduras nuevas
            // por lo tanto se haria chequeando el inventario.
            //Piece_Set recambio = new Piece_Set();
            //recambio.Initialize("shield", "set1");
            //pieces_armor_new.Add(recambio);

            //recambio = new Piece_Set();
            //recambio.Initialize("gauntletback", "set1");
            //pieces_armor_new.Add(recambio);

            //recambio = new Piece_Set();
            //recambio.Initialize("greaveback", "set1");
            //pieces_armor_new.Add(recambio);

            //recambio = new Piece_Set();
            //recambio.Initialize("helm", "set1");
            //pieces_armor_new.Add(recambio);

            //recambio = new Piece_Set();
            //recambio.Initialize("breastplate", "set1");
            //pieces_armor_new.Add(recambio);

            //recambio = new Piece_Set();
            //recambio.Initialize("tasset", "set1");
            //pieces_armor_new.Add(recambio);

            //recambio = new Piece_Set();
            //recambio.Initialize("greavetop", "set1");
            //pieces_armor_new.Add(recambio);

            //recambio = new Piece_Set();
            //recambio.Initialize("sword", "set1");
            //pieces_armor_new.Add(recambio);

            //recambio = new Piece_Set();
            //recambio.Initialize("gauntlettop", "set1");
            //pieces_armor_new.Add(recambio);
            #endregion

            // Piezas de la armadura al comenzar
            UpdateArmor(pieces_armor_new);

            // Asigno control por default al jugador
            controles[(int)Globales.Controls.ARRIBA] = Keys.W;
            controles[(int)Globales.Controls.ABAJO] = Keys.S;
            controles[(int)Globales.Controls.IZQUIERDA] = Keys.A;
            controles[(int)Globales.Controls.DERECHA] = Keys.D;
            controles[(int)Globales.Controls.BOTON_1] = Keys.T;
            controles[(int)Globales.Controls.BOTON_2] = Keys.Y;      
        }

        // Actualizar animacion
        public override void Update(GameTime gameTime)
        {
            foreach(Animacion piezaAnimada in pieces_anim)
            {
                piezaAnimada.position = Position;
                piezaAnimada.Update(gameTime);
                
                // Para los stats de cada personaje (borrar mas tarde)
                mensaje = Position;
            }
        }

        // Dibujar Jugador
        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Animacion piezaAnimada in pieces_anim)
            {
                piezaAnimada.Draw(spriteBatch, Direccion);
            }
            
            // Si no separo este proceso de dibujo desconcha las posiciones de las capas del jugador
            spriteBatch.DrawString(Globales.CheckStatusVar_2,
            "Frame Actual = " + mensaje1.ToString() + System.Environment.NewLine +
            "Frame Total = " + mensaje2.ToString() + System.Environment.NewLine +
            "Direccion Actual = " + mensaje3.ToString() + System.Environment.NewLine +
            "Accion Actual = " + mensaje4.ToString() + System.Environment.NewLine,
            mensaje, Color.DarkRed);

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

            // Obtengo teclas presionadas
            Globales.currentKeyboardState = Keyboard.GetState();

            // Logica de las acciones
            ActionLogic();
                        
            // Hacer que el jugador no salga de la pantalla. 
            // Tomamos el rectangulo que genera la camara para acomodar al jugador.
            Position.X = MathHelper.Clamp(Position.X, LimitesPantalla.Left + AnchoPersonaje / 2, LimitesPantalla.Width - AnchoPersonaje / 2);
            //Position.Y = MathHelper.Clamp(Position.Y, AltoNivel * 0.50f, AltoNivel - AltoPersonaje / 2);
            Position.Y = MathHelper.Clamp(Position.Y, 3*AltoNivel/5, AltoNivel);
            
            // No es necesario mas acomodar la fila ya que todos vienen con fila 0
            // Solo se acomoda la cantidad de frames por animacion y que animacion va en cada pieza segun la accion ejecutandose.
            #region ANIMACION POR PIEZA

            foreach (Animacion piezaAnimacion in pieces_anim)
            {
                foreach (Texturas textura in Globales.TexturasPaladin)
                {
                    string aver = pieces_armor.Get_Set(textura.piece);
                    
                    if (textura.piece == piezaAnimacion.nombrePieza &&
                        textura.set == pieces_armor.Get_Set(textura.piece) &&
                        textura.action == accionActual.ToString().ToLower())
                    {
                        piezaAnimacion.CargarTextura(textura);
                    }
                }
            }

            // Vuelve a 0 el frame de la animacion si cambio de accion
            if (accionAnterior != accionActual)
            {
                foreach (Animacion animacion in pieces_anim)
                {
                    animacion.CurrentFrame = 0;
                }
                
                accionAnterior = accionActual;
            }

            #endregion

            // Status del personaje
            mensaje1 = pieces_anim[0].CurrentFrame;
            mensaje2 = pieces_anim[0].FrameCount;
            mensaje3 = Direccion;
            mensaje4 = accionActual;
        }

        private void ActionLogic()
        {
            if (accionActual != Globales.Actions.HIT1 &&
                accionActual != Globales.Actions.HIT2 &&
                accionActual != Globales.Actions.HIT3)
            {
                
                #region MOVIMIENTO

                // Si no se toca nada quedara por default que esta parado
                accionActual = Globales.Actions.STAND;

                // Si se presiona alguna tecla de movimiento
                if (Globales.currentKeyboardState.IsKeyDown(controles[(int)Globales.Controls.IZQUIERDA]))
                {
                    Position.X -= VelocidadPersonaje;
                    Direccion = Globales.Mirada.IZQUIERDA;
                    accionActual = Globales.Actions.WALK;
                }
                else if (Globales.currentKeyboardState.IsKeyDown(controles[(int)Globales.Controls.DERECHA]))
                {
                    Position.X += VelocidadPersonaje;
                    Direccion = Globales.Mirada.DERECHA;
                    accionActual = Globales.Actions.WALK;
                }

                if (Globales.currentKeyboardState.IsKeyDown(controles[(int)Globales.Controls.ARRIBA]))
                {
                    Position.Y -= VelocidadPersonaje;
                    accionActual = Globales.Actions.WALK;
                }
                else if (Globales.currentKeyboardState.IsKeyDown(controles[(int)Globales.Controls.ABAJO]))
                {
                    Position.Y += VelocidadPersonaje;
                    accionActual = Globales.Actions.WALK;
                }

                #endregion

                // Si presiono golpear cancela todas las demas acciones hasta que esta termine su ciclo
                // Tambien genera un rango de los 3 diferentes tipos de golpes (algo netamente visual sin impacto en el juego)
                if (Globales.currentKeyboardState.IsKeyDown(controles[(int)Globales.Controls.BOTON_1]))
                {
                    Random azar = new Random();
                    //accionActual = (Globales.Actions)azar.Next((int)Globales.Actions.HIT1, (int)Globales.Actions.HIT3);
                    accionActual = (Globales.Actions)azar.Next(2, 5);
                }
            }
            else
            {
                // Si esta pegando tiene que terminar su animacion y despues desbloquear otra vez la gama de movimientos
                if (this.pieces_anim[0].CurrentFrame == this.pieces_anim[0].FrameCount - 1)
                {
                    accionActual = Globales.Actions.STAND;
                }
            }
        }

        /// <summary>
        /// Cargo los set de armadura que corresponden a cada pieza del cuerpo.
        /// </summary>
        /// <param name="set_pieces">Set de shield, gauntlets, greaves, helm, breastplate, tasset, sword respectivamente</param> 
        public override void UpdateArmor(List<Piece_Set> set_pieces)
        {
            // shield, gauntlets, greaves, helm, breastplate, tasset, sword
            foreach (Piece_Set set_piece in set_pieces)
            {
                pieces_armor.Set_Set(set_piece);
            }

            foreach(Animacion piezaAnimacion in pieces_anim)
            {
                foreach (Texturas textura in Globales.TexturasPaladin)
                {
                    if (textura.piece == piezaAnimacion.nombrePieza && 
                        textura.set == pieces_armor.Get_Set(textura.piece) && 
                        textura.action == accionActual.ToString().ToLower())
                    {
                        piezaAnimacion.CargarTextura(textura, Position, AnchoPersonaje, AltoPersonaje, Tiempo_Frame, Color.White, true);
                    }
                }
            }

        }

        /// <summary>
        /// Establece el tiempo de frame en ejecucion
        /// </summary>
        /// <param name="Tiempo">El tiempo que va a durar el frame en pantalla de las distintas animaciones del personaje</param>
        void TiempoFrameEjecucion(int Tiempo)
        {
            foreach (Animacion piezaAnimada in pieces_anim)
            {
                piezaAnimada.frameTime = Tiempo;
            }
        }

        /// <summary>
        /// Pausa la animacion en el frame actual
        /// </summary>
        /// <param name="desactivar">pone o quita la pausa segun este parametro</param>
        void PausarAnimacion(bool desactivar)
        {
            foreach (Animacion piezaAnimada in pieces_anim)
            {
                piezaAnimada.pausa = desactivar;
            }
        }

        #endregion

    }
}
