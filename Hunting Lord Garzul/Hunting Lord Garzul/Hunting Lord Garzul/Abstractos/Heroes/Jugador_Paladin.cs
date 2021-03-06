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

            #region CONTROLES

                /// <summary>
                /// Accion que se realiza 
                /// </summary>
                private Globales.Actions CurrentAction;

                /// <summary>
                /// Accion realizada anteriormente
                /// </summary>
                private Globales.Actions OldAction;
                
                /// <summary>
                /// Animacion de cada pieza de armadura:
                /// 1.shield
                /// 2.gauntletback
                /// 3.greaveback
                /// 4.helm
                /// 5.breastplate
                /// 6.tasset
                /// 7.greavetop
                /// 8.sword
                /// 9.gauntlettop
                /// </summary>
                private Animacion[] Pieces_Anim = new Animacion[9];
    
                /// <summary>
                /// Posicion del jugador relativa a la parte superior izquierda de la pantalla.
                /// Esta posicion marca donde se encuentra el jugador en la pantalla y no en el mapa donde se esta moviendo,
                /// y es a esta posicion a la que se le aplican los limites de la pantalla.  
                /// </summary>
                protected Vector2 Position;
                
                /// <summary>
                /// Ancho y alto de un cuadro del sprite
                /// </summary>
                protected int FrameWidth = Globales.FrameWidth;
                protected int FrameHeight = Globales.FrameHeight;

            #endregion

            #region JUGABILIDAD

                /// <summary>
                /// Tipo de cada pieza de armadura:
                /// Shield, gauntletback, greaveback, helm, breastplate, tasset, greavetop, sword, gauntlettop. 
                /// </summary>
                protected Pieces_Sets pieces_armor = new Pieces_Sets();
                protected List<Piece_Set> pieces_armor_new = new List<Piece_Set>();
        
                /// <summary>
                /// Velocidad de movimiento del jugador 
                /// </summary>
                protected float PlayerSpeed;

                /// <summary>
                /// Establece tiempo de frame inicial cuando llama al UpdateArmor
                /// El UpdateArmor no ocurre en el loop se pide explicitamente 
                /// </summary>
                protected int FrameTime;
            
            #endregion

            #region DATOS

                // Mensajes de datos
                protected float mensaje1;
                protected float mensaje2;
                protected Globales.Mirada mensaje3;
                protected Globales.Actions mensaje4;
                protected float mensaje5;
                protected float mensaje6;
                protected float mensaje7;
                protected float mensaje8;
                protected float mensaje9;

                // Donde se va a alojar el mensaje de chequeo de status
                Vector2 mensaje;
            
            #endregion

        #endregion

        #region METODOS
            
            #region GET-SET

                public Globales.Actions currentAction
                {
                    get { return CurrentAction; }
                    set { CurrentAction = value; }
                }

                public Globales.Actions oldAction
                {
                    get { return OldAction; }
                    set { OldAction = value; }
                }

                public Animacion[] pieces_anim
                {
                    get { return Pieces_Anim; }
                    set { Pieces_Anim = value; }
                }

            #endregion

            #region ABSTRACTAS

                /// <summary>
                /// Inicializar al jugador
                /// </summary>
                /// <param name="posicion"></param>
                public override void Initialize(Vector2 posicion)
                {

                    // Establezco variables por default para comenzar
                    Position = posicion;
                    mensaje = Position;
                    PlayerSpeed = 3.0f;
                    direction = Objetos.Globales.Mirada.RIGHT;
                    currentAction = Globales.Actions.STAND;
                    oldAction = currentAction;
                    FrameTime = 50;
                    health += 500;

                    // Establezco las banderas de da�ados
                    ResetInjured();
                    
                    // Inicializo partes de armadura actual
                    pieces_armor.Initialize();

                    // Inicializo las piezas de animacion
                    for (int i = 0; i < Globales.PiecesPaladin.Length; i++)
                    {
                        Pieces_Anim[i] = new Animacion();
                        Pieces_Anim[i].Initialize(Globales.PiecesPaladin[i]);
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

                    this.animations = this.Pieces_Anim;

                    // Asigno control por default al jugador
                    controls[(int)Globales.Controls.UP] = Keys.W;
                    controls[(int)Globales.Controls.DOWN] = Keys.S;
                    controls[(int)Globales.Controls.LEFT] = Keys.A;
                    controls[(int)Globales.Controls.RIGHT] = Keys.D;
                    controls[(int)Globales.Controls.BUTTON_1] = Keys.T;
                    controls[(int)Globales.Controls.BUTTON_2] = Keys.Y;

                    // Ralentizar los cuadros por segundo del personaje
                    // TiempoFrameEjecucion(1);
                }

                /// <summary>
                /// Actualizar animacion
                /// </summary>
                /// <param name="gameTime"></param>
                public override void Update(GameTime gameTime)
                {
                    foreach (Animacion piezaAnimada in Pieces_Anim)
                    {
                        piezaAnimada.position = Position;
                        piezaAnimada.Update(gameTime);

                        // Para los stats de cada personaje (borrar mas tarde) GAB
                        mensaje = Position;
                    }
                }

                /// <summary>
                /// Dibujar Jugador
                /// </summary>
                /// <param name="spriteBatch"></param>
                public override void Draw(SpriteBatch spriteBatch)
                {
                    foreach (Animacion piezaAnimada in Pieces_Anim)
                    {
                        piezaAnimada.Draw(spriteBatch, direction, piezaAnimada.color);
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
                    "Y = " + mensaje8.ToString() + System.Environment.NewLine +
                    "Vida = " + mensaje9.ToString(),
                    mensaje, Color.DarkRed);

                    // rectangulos de colision para chequear
                    if (Globales.EnableRectangles)
                    {
                        DrawRectangle(this.pieces_anim[0].GetPosition(), Globales.Punto_Blanco, spriteBatch);
                        DrawRectangle(Globales.Rectangle_Collision, Globales.Punto_Blanco, spriteBatch);
                        DrawRectangle(Globales.Rectangle_Collision_2, Globales.Punto_Blanco, spriteBatch);
                    }
                }

                /// <summary>
                /// Actualizar cosas del jugador antes del update general. Aca va todo lo que es la logica del mismo, saltar pegar, etc.
                /// </summary>
                /// <param name="gameTime">El gametime del juego.</param>
                /// <param name="LimitesPantalla">Los limites que puso la camara con respecto a la pantalla que vemos.</param>
                /// <param name="AltoNivel">La altura total del escenario.</param>
                /// <param name="AnchoNivel">El ancho total del escenario.</param>
                public override void UpdatePlayer(GameTime gameTime, Rectangle LimitesPantalla, int AltoNivel, int AnchoNivel)
                {

                    Update(gameTime);

                    // Obtengo teclas presionadas
                    Globales.currentKeyboardState = Keyboard.GetState();

                    // Logica de las acciones, moverse, pegar, etc
                    ActionLogic();

                    // Logica de las colisiones al golpear
                    CollisionLogic();

                    // Aqui aplicamos los da�os y todo lo correspondiente a los efectos de las acciones hechas anteriormente
                    EffectLogic();

                    // Hace que el jugador no salga de la pantalla reacomodandolo dentro de la misma.
                    // Tomamos como pantalla el rectangulo que genera la camara para acomodar al jugador y limitamos de acuerdo a estas medidas.
                    // El FrameEscalado es para acomodar al personaje de acuerdo a la nueva escala adquirida dependiendo de la pantalla fisica donde se ejecuta el juego.
                    Rectangle FrameEscalado = this.animations[0].GetPosition();
                    Position.X = MathHelper.Clamp(Position.X, LimitesPantalla.Left + FrameEscalado.Width / 2, LimitesPantalla.Width - FrameEscalado.Width / 2);
                    Position.Y = MathHelper.Clamp(Position.Y, AltoNivel - AltoNivel / 2, AltoNivel - FrameEscalado.Height / 2);

                    // No es necesario mas acomodar la fila ya que todos vienen con fila 0
                    // Solo se acomoda la cantidad de frames por animacion y que animacion va en cada pieza segun la accion ejecutandose.
                    #region ANIMACION POR PIEZA

                    foreach (Animacion piezaAnimacion in Pieces_Anim)
                    {
                        foreach (Texturas textura in Globales.PaladinTextures)
                        {
                            if (textura.piece == piezaAnimacion.pieceName &&
                                textura.set == pieces_armor.Get_Set(textura.piece) &&
                                textura.action == currentAction.ToString().ToLower())
                            {
                                piezaAnimacion.LoadTexture(textura);
                            }
                        }
                    }

                    // Vuelve a 0 el frame de la animacion si cambio de accion
                    if (oldAction != currentAction)
                    {
                        foreach (Animacion animacion in Pieces_Anim)
                        {
                            animacion.CurrentFrame = 0;
                        }

                        oldAction = currentAction;
                    }

                    #endregion

                    // Status del personaje
                    mensaje1 = Pieces_Anim[0].CurrentFrame;
                    mensaje2 = Pieces_Anim[0].FrameCount;
                    mensaje3 = direction;
                    mensaje4 = currentAction;
                    mensaje5 = this.Pieces_Anim[0].GetPosition().Height;
                    mensaje6 = this.Pieces_Anim[0].GetPosition().Width;
                    mensaje7 = this.Pieces_Anim[0].GetPosition().X;
                    mensaje8 = this.Pieces_Anim[0].GetPosition().Y;
                }

                /// <summary>
                /// Cargo los set de armadura que corresponden a cada pieza del cuerpo.
                /// </summary>
                /// <param name="set_pieces"> Shield, gauntlets, greaves, helm, breastplate, tasset, sword respectivamente </param> 
                public override void UpdateArmor(List<Piece_Set> set_pieces)
                {
                    // shield, gauntlets, greaves, helm, breastplate, tasset, sword
                    foreach (Piece_Set set_piece in set_pieces)
                    {
                        pieces_armor.Set_Set(set_piece);
                    }

                    foreach (Animacion piezaAnimacion in Pieces_Anim)
                    {
                        foreach (Texturas textura in Globales.PaladinTextures)
                        {
                            if (textura.piece == piezaAnimacion.pieceName &&
                                textura.set == pieces_armor.Get_Set(textura.piece) &&
                                textura.action == currentAction.ToString().ToLower())
                            {
                                piezaAnimacion.LoadTexture(textura, Position, FrameWidth, FrameHeight, FrameTime, Color.White, true);
                            }
                        }
                    }

                }

                /// <summary>
                /// Obtiene la posicion del jugador relativa a la parte superior izquierda de la pantalla
                /// </summary>
                /// <returns> Posicion del jugador </returns>
                public override Vector2 GetPosition()
                {
                    return Position;
                }

                /// <summary>
                /// Cambia el color del personaje entero 
                /// </summary>
                /// <param name="tinte"> Color deseado </param>
                public override void ColorAnimationChange(Color tinte)
                {
                    foreach (Animacion animacion in this.animations)
                    {
                        animacion.ColorChange(tinte);
                    }
                }

                /// <summary>
                /// Cambia el color de una pieza del personaje 
                /// </summary>
                /// <param name="tinte"> Color deseado </param>
                /// <param name="pieza"> Pieza que queremos cambiar el color </param>
                public override void ColorPieceChange(Color tinte, int pieza)
                {
                    this.animations[pieza].ColorChange(tinte);
                }

                /// <summary>
                /// Obtiene el frame actual de la animacion parandose en la primera pieza de la misma [0]
                /// </summary>
                /// <returns> Frame actual de la animacion </returns>
                public override int GetCurrentFrame()
                {
                    return this.animations[0].CurrentFrame;
                }

                /// <summary>
                /// Obtiene cantidad total de frames de la animacion parandose en la primera pieza de la misma [0]
                /// Se le resta 1 al valor ya que como es cantidad no cuenta desde 0, como lo hacen los indices,
                /// de esta manera podemos comparar indices con cantidad.
                /// </summary>
                /// <returns> Frames total de la animacion </returns>
                public override int GetTotalFrames()
                {
                    return this.animations[0].FrameCount - 1;
                }

                /// <summary>
                /// Activa o desactiva al jugador (si no esta activo no se dibuja)
                /// </summary>
                public override void ActivatePlayer(bool active)
                {
                    foreach( Animacion piece in this.animations )
                    {
                        piece.active = active;
                    }
                }

                public override void ResetInjured()
                {
                    for (int i = 0; i < injured.Length; i++)
                    {
                        this.injured[i] = false;
                    }
                }

            #endregion

            #region PROPIOS

                /// <summary>
                /// Establece el tiempo de frame en ejecucion
                /// </summary>
                /// <param name="Tiempo">El tiempo que va a durar el frame en pantalla de las distintas animaciones del personaje</param>
                void FrameSpeed(int Tiempo)
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
                void PauseAnimation(bool desactivar)
                {
                    foreach (Animacion piezaAnimada in Pieces_Anim)
                    {
                        piezaAnimada.pause = desactivar;
                    }
                }

                /// <summary>
                /// Logica de todas las acciones, los movimientos, los golpes, etc.
                /// </summary>
                private void ActionLogic()
                {
                    if (currentAction != Globales.Actions.HIT1 &&
                        currentAction != Globales.Actions.HIT2 &&
                        currentAction != Globales.Actions.HIT3)
                    {

                        #region MOVIMIENTO

                        // Si no se toca nada quedara por default que esta parado
                        currentAction = Globales.Actions.STAND;

                        // Si se presiona alguna tecla de movimiento
                        if (Globales.currentKeyboardState.IsKeyDown(controls[(int)Globales.Controls.LEFT]))
                        {
                            Position.X -= PlayerSpeed;
                            direction = Globales.Mirada.LEFT;
                            currentAction = Globales.Actions.WALK;
                        }
                        else if (Globales.currentKeyboardState.IsKeyDown(controls[(int)Globales.Controls.RIGHT]))
                        {
                            Position.X += PlayerSpeed;
                            direction = Globales.Mirada.RIGHT;
                            currentAction = Globales.Actions.WALK;
                        }

                        if (Globales.currentKeyboardState.IsKeyDown(controls[(int)Globales.Controls.UP]))
                        {
                            Position.Y -= PlayerSpeed;
                            currentAction = Globales.Actions.WALK;
                        }
                        else if (Globales.currentKeyboardState.IsKeyDown(controls[(int)Globales.Controls.DOWN]))
                        {
                            Position.Y += PlayerSpeed;
                            currentAction = Globales.Actions.WALK;
                        }

                        #endregion


                        #region GOLPE

                        // Si presiono golpear cancela todas las demas acciones hasta que esta termine su ciclo
                        // Tambien genera un rango de los 3 diferentes tipos de golpes (algo netamente visual sin impacto en el juego)
                        if (Globales.currentKeyboardState.IsKeyDown(controls[(int)Globales.Controls.BUTTON_1]))
                        {
                            // La aleatoriedad en los golpes depende de como estan almacenados en las variables globales, la primer variable es incluyente y la segunda excluyente.
                            currentAction = (Globales.Actions)Globales.randomly.Next(2, 5);

                        }
                        #endregion
                    }
                    else
                    {
                        // Si esta pegando tiene que terminar su animacion y despues desbloquear otra vez la gama de movimientos
                        // Para esto comparamos el frame actual de la animacion con su frame total
                        if (this.GetCurrentFrame() == this.GetTotalFrames())
                        {
                            currentAction = Globales.Actions.STAND;
                            
                            // Cuando termine la animacion de pegar puede generar da�o de vuelta a alguien que ya haya atacado
                            ResetInjured();
                        }
                    }
                }

                /// <summary>
                /// Logica de las colisiones de los golpes:
                /// 
                ///     Implementamos un chequeo jugador por jugador a la hora de golpear, que cumpla con las siguientes reglas:
                ///     - Si le toca chequear con el mismo se saltea.
                ///     - Si el frame de la animacion no es justo cuando golpea con la espada se saltea.
                ///     - Si fue golpeado anteriormente se saltea
                ///     - Si es fantasma se saltea
                /// </summary>
                private void CollisionLogic()
                {
                    if ((   this.currentAction == Globales.Actions.HIT1 || 
                            this.currentAction == Globales.Actions.HIT2 || 
                            this.currentAction == Globales.Actions.HIT3) && 
                            !this.ghost_mode)
                    {
                        
                        for(int i=0; i < Globales.totalQuant; i++)
                        {
                            // Ver summary
                            if (Globales.players[i] != this && 
                                this.GetCurrentFrame() == 5 && 
                                !this.injured[i] &&
                                !Globales.players[i].ghost_mode)
                            {
                                Rectangle temp = this.Pieces_Anim[0].GetPosition();
                                Rectangle temp2 = Globales.players[i].animations[0].GetPosition();

                                // Si esta dentro del radio del golpe
                                if (CollisionVerifier(ref temp, ref temp2))
                                {
                                    // Cuando la armadura esta detras del efecto de la espada no se puede ver bien el cambio de color
                                    Globales.players[i].ColorAnimationChange(Color.Red);
                                    Globales.players[i].injured_value = 10;
                                    this.injured[i] = true;
                                }
                            }
                        }
                    }
                }

                /// <summary>
                /// L�gica de los efectos de las colisiones y movimientos realizados.
                /// </summary>
                private void EffectLogic()
                {

                    if (!this.ghost_mode)
                    {
                        // Reestablezco su color natural si no va a recibir da�o, de esta manera no permito que vuelva a su color 
                        // demasiado rapido como para que no se vea que fue da�ado
                        if (this.injured_value == 0)
                            this.ColorAnimationChange(Color.White);
                        
                        // Hago la resta necesaria a la health
                        this.health -= this.injured_value;

                        // Vuelvo el contador de da�o a 0 y quito que este da�ado
                        this.injured_value = 0;
                        
                        // Si pierde toda su HP se vuelve fantasma
                        if (this.health <= 0)
                        {
                            this.ghost_mode = true;
                        }
                    }
                    else
                    {
                        this.ColorAnimationChange(Globales.ColorGhost);

                        if (this.health > 0)
                        {
                            ghost_mode = false;
                        }
                    }

                    // MENSAJES: Veo la health de los personajes
                    mensaje9 = this.health;
                }
                
                /// <summary>
                /// Chequea las colisiones
                /// </summary>
                /// <param name="temp">Rectangulo del primer elemento</param>
                /// <param name="temp2">Rectangulo del segundo elemento</param>
                /// <returns></returns>
                private bool CollisionVerifier(ref Rectangle temp, ref Rectangle temp2)
                {
                    return (temp.X + temp.Width >= temp2.Center.X - Globales.HitRangeX &&
                            temp.X <= temp2.X &&
                            temp.Y >= temp2.Y - Globales.HitRangeY &&
                            temp.Y <= temp2.Y + Globales.HitRangeY &&
                            this.direction == Globales.Mirada.RIGHT)
                            ||
                           (temp.X <= temp2.Center.X + Globales.HitRangeX &&
                            temp.X + temp.Width >= temp2.X + temp2.Width &&
                            temp.Y >= temp2.Y - Globales.HitRangeY &&
                            temp.Y <= temp2.Y + Globales.HitRangeY &&
                            this.direction == Globales.Mirada.LEFT);
                }

                /// <summary>
                /// Dibuja los rectangulos que delimitan las colisiones del personaje.
                /// </summary>
                /// <param name="rec">Rectangulo del personaje</param>
                /// <param name="tex">Textura utilizada para dibujar el rectangulo</param>
                /// <param name="spriteBatch">Proceso de dibujado</param>
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

                /// <summary>
                /// Dibuja lineas rectas
                /// </summary>
                /// <param name="A">Medida A</param>
                /// <param name="B">Medida B</param>
                /// <param name="tex">Textura utilizada para dibujar las lineas</param>
                /// <param name="col">Color de las lineas</param>
                /// <param name="spriteBatch">Proceso de dibujado</param>
                /// <param name="thickness">Grosor de la linea</param>
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
