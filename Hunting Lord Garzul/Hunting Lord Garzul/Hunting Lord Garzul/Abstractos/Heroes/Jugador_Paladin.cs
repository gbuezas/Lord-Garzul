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
        private Animacion[] Pieces_Anim = new Animacion[9];
        public Animacion[] pieces_anim
        {
            get { return Pieces_Anim; }
            set { Pieces_Anim = value; }
        }
        
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
        protected int AltoPersonaje = 320;
        //protected int AltoPersonaje = 600;
        
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
        protected float mensaje5;
        protected float mensaje6;
        protected float mensaje7;
        protected float mensaje8;
        
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
            for (int i = 0; i < Globales.PiezasPaladin.Length; i++ )
            {
                Pieces_Anim[i] = new Animacion();
                Pieces_Anim[i].Initialize(Globales.PiezasPaladin[i]);
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

            this.animaciones = this.Pieces_Anim;

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
            foreach(Animacion piezaAnimada in Pieces_Anim)
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
            foreach (Animacion piezaAnimada in Pieces_Anim)
            {
                piezaAnimada.Draw(spriteBatch, Direccion, piezaAnimada.color);
            }
            
            // Si no separo este proceso de dibujo desconcha las posiciones de las capas del jugador
            // +++ Me parece que esto se soluciono cuando cambie el parametro de dibujo en el draw general +++
            spriteBatch.DrawString(Globales.CheckStatusVar_2,
            "Frame Actual = " + mensaje1.ToString() + System.Environment.NewLine +
            "Frame Total = " + mensaje2.ToString() + System.Environment.NewLine +
            "Direccion Actual = " + mensaje3.ToString() + System.Environment.NewLine +
            "Accion Actual = " + mensaje4.ToString() + System.Environment.NewLine +
            "Alto = " + mensaje5.ToString() + System.Environment.NewLine +
            "Ancho = " + mensaje6.ToString() + System.Environment.NewLine +
            "X = " + mensaje7.ToString() + System.Environment.NewLine +
            "Y = " + mensaje8.ToString() + System.Environment.NewLine,
            mensaje, Color.DarkRed);

            // rectangulos de colision para chequear (borrar)
            //DrawRectangle(this.pieces_anim[7].ObtenerPosicion(), Globales.Punto_Blanco, spriteBatch);
            DrawRectangle(Globales.Rectangulo_Colision, Globales.Punto_Blanco, spriteBatch);
            DrawRectangle(Globales.Rectangulo_Colision_2, Globales.Punto_Blanco, spriteBatch);
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

            // Obtengo teclas presionadas
            Globales.currentKeyboardState = Keyboard.GetState();

            // Logica de las acciones, moverse, pegar, etc
            ActionLogic();

            // Logica de las colisiones al golpear
            ColissionLogic();

            // Hace que el jugador no salga de la pantalla reacomodandolo dentro de la misma
            // Tomamos el rectangulo que genera la camara para acomodar al jugador.
            // 320 x 320 - AltoPersonaje x AnchoPersonaje
            Position.X = MathHelper.Clamp(Position.X, LimitesPantalla.Left + AnchoPersonaje / 2, LimitesPantalla.Width - AnchoPersonaje / 2);
            Position.Y = MathHelper.Clamp(Position.Y, AltoNivel - AltoPersonaje, AltoNivel - AltoPersonaje/5);
            
            // No es necesario mas acomodar la fila ya que todos vienen con fila 0
            // Solo se acomoda la cantidad de frames por animacion y que animacion va en cada pieza segun la accion ejecutandose.
            #region ANIMACION POR PIEZA

            foreach (Animacion piezaAnimacion in Pieces_Anim)
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
                foreach (Animacion animacion in Pieces_Anim)
                {
                    animacion.CurrentFrame = 0;
                }
                
                accionAnterior = accionActual;
            }

            #endregion

            // Status del personaje
            mensaje1 = Pieces_Anim[0].CurrentFrame;
            mensaje2 = Pieces_Anim[0].FrameCount;
            mensaje3 = Direccion;
            mensaje4 = accionActual;
            mensaje5 = this.Pieces_Anim[7].ObtenerPosicion().Height;
            mensaje6 = this.Pieces_Anim[7].ObtenerPosicion().Width;
            mensaje7 = this.Pieces_Anim[7].ObtenerPosicion().X;
            mensaje8 = this.Pieces_Anim[7].ObtenerPosicion().Y;
        }

        /// <summary>
        /// Logica de todas las acciones, los movimientos, los golpes, etc.
        /// </summary>
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
                    Random azar = new Random(System.DateTime.UtcNow.Second);

                    // El rango depende de como estan almacenados en las variables globales, la primer variable es incluyente y la segunda excluyente.
                    accionActual = (Globales.Actions)azar.Next(2, 5);
                }
            }
            else
            {
                // Si esta pegando tiene que terminar su animacion y despues desbloquear otra vez la gama de movimientos
                if (this.Pieces_Anim[0].CurrentFrame == this.Pieces_Anim[0].FrameCount - 1)
                {
                    accionActual = Globales.Actions.STAND;
                }
            }
        }

        /// <summary>
        /// Logica de las colisiones de los golpes.
        /// </summary>
        private void ColissionLogic()
        {
            if(this.accionActual == Globales.Actions.HIT1 ||
               this.accionActual == Globales.Actions.HIT2 ||
               this.accionActual == Globales.Actions.HIT3)
            {
                foreach (Jugadores player in Globales.players)
                {
                    // Chequea jugador por jugador a ver con quien toca, si le toca chequear con el mismo se saltea.
                    // Implementamos un chequeo por radio a la hora de golpear, si alguien esta dentro del radio es golpeado.
                    if (player != this && this.animaciones[7].CurrentFrame >= 3)
                    {
                        Rectangle temp = this.Pieces_Anim[7].ObtenerPosicion();
                        Rectangle temp2 = player.animaciones[7].ObtenerPosicion();

                        int top = Math.Max(temp.Top, temp2.Top);
                        int bottom = Math.Min(temp.Bottom, temp2.Bottom);
                        int left = Math.Max(temp.Left, temp2.Left);
                        int right = Math.Min(temp.Right, temp2.Right);

                        if (temp.X + temp.Width >= temp2.Center.X &&
                           temp.X <= temp2.X &&
                           temp.Y >= temp2.Y - 5 &&
                           temp.Y <= temp2.Y + 5 ||
                           temp.X <= temp2.Center.X &&
                           temp.X + temp.Width > temp2.X + temp2.Width &&
                           temp.Y >= temp2.Y - 5 &&
                           temp.Y <= temp2.Y + 5)
                        {
                            this.pieces_anim[7].CambiarColor(Color.Red);
                        }
                        else
                        {
                            this.pieces_anim[7].CambiarColor(Color.White);
                        }
                    }
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

            foreach(Animacion piezaAnimacion in Pieces_Anim)
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
            foreach (Animacion piezaAnimada in Pieces_Anim)
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
            foreach (Animacion piezaAnimada in Pieces_Anim)
            {
                piezaAnimada.pausa = desactivar;
            }
        }

        //static bool IntersectsPixels(Rectangle rect1, Color[] data1, Rectangle rect2, Color[] data2)
        //{
        //    int top = Math.Max(rect1.Top, rect2.Top);
        //    int bottom = Math.Min(rect1.Bottom,rect2.Bottom);
        //    int left = Math.Max(rect1.Left, rect2.Left);
        //    int right = Math.Min(rect1.Right,rect2.Right);

        //    for (int y = top; y < bottom; y++ )
        //    {
        //        for (int x = left; x < right; x++ )
        //        {
        //            Color colour1 = data1[(x - rect1.Left) + (y - rect1.Top) * rect1.Width];
        //            Color colour2 = data2[(x - rect2.Left) + (y - rect2.Top) * rect2.Width];

        //            // Si los pixeles tienen el valor alfa distintos de 0 entonces tienen color
        //            if(colour1.A != 0 && colour2.A != 0)
        //            {
        //                return true;
        //            }
        //        }
        //    }

        //    return false;
        //}

        #region Rectangulo de colision dibujado

        public static void DrawRectangle(Rectangle rec, Texture2D tex, SpriteBatch spriteBatch)
        {
            Vector2 Position = new Vector2(rec.X, rec.Y);
            int border = 1;

            int borderWidth = (int)(rec.Width) + (border * 2);
            int borderHeight = (int)(rec.Height) + (border);

            DrawStraightLine(new Vector2((int)rec.X, (int)rec.Y), new Vector2((int)rec.X + rec.Width, (int)rec.Y), tex, Color.White, spriteBatch, border);
            DrawStraightLine(new Vector2((int)rec.X, (int)rec.Y + rec.Height), new Vector2((int)rec.X + rec.Width, (int)rec.Y + rec.Height), tex, Color.White, spriteBatch, border);
            DrawStraightLine(new Vector2((int)rec.X, (int)rec.Y), new Vector2((int)rec.X, (int)rec.Y + rec.Height), tex, Color.White, spriteBatch, border);
            DrawStraightLine(new Vector2((int)rec.X + rec.Width, (int)rec.Y), new Vector2((int)rec.X + rec.Width, (int)rec.Y + rec.Height), tex, Color.White, spriteBatch, border);
        }

        public static void DrawStraightLine(Vector2 A, Vector2 B, Texture2D tex, Color col, SpriteBatch spriteBatch, int thickness)
        {
            Rectangle rec;
            if (A.X < B.X)
            {
                rec = new Rectangle((int)A.X, (int)A.Y, (int)(B.X - A.X), thickness);
            }
            else
            {
                rec = new Rectangle((int)A.X, (int)A.Y, thickness, (int)(B.Y - A.Y));
            }

            spriteBatch.Draw(tex, rec, col);
        }
        #endregion

        #endregion

    }
}
