using System;
using System.Collections.Generic;
using Hunting_Lord_Garzul.Generales;
using Hunting_Lord_Garzul.Objetos;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Hunting_Lord_Garzul.Abstractos.Heroes
{
    class JugadorPaladin : Jugadores
    {

        # region VARIABLES

            #region CONTROLES

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
                private Animacion[] _piecesAnim = new Animacion[9];
    
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
                protected PiecesSets PiecesArmor = new PiecesSets();
                protected List<PieceSet> PiecesArmorNew = new List<PieceSet>();
        
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
                protected float Mensaje1;
                protected float Mensaje2;
                protected Globales.Mirada Mensaje3;
                protected Globales.Actions Mensaje4;
                protected float Mensaje5;
                protected float Mensaje6;
                protected float Mensaje7;
                protected float Mensaje8;
                protected float Mensaje9;

                // Donde se va a alojar el mensaje de chequeo de status
                Vector2 _mensaje;
            
            #endregion

        #endregion

        #region METODOS
            
            #region GET-SET

                public Globales.Actions CurrentAction { get; set; }

                public Globales.Actions OldAction { get; set; }

                public Animacion[] PiecesAnim
                        {
                            get { return _piecesAnim; }
                            set { _piecesAnim = value; }
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
                    _mensaje = Position;
                    PlayerSpeed = 3.0f;
                    Direction = Globales.Mirada.Right;
                    CurrentAction = Globales.Actions.Stand;
                    OldAction = CurrentAction;
                    FrameTime = 50;
                    Health -= 10;

                    // Inicializo partes de armadura actual
                    PiecesArmor.Initialize();

                    // Inicializo las piezas de animacion
                    for (var i = 0; i < Globales.PiecesPaladin.Length; i++)
                    {
                        _piecesAnim[i] = new Animacion();
                        _piecesAnim[i].Initialize(Globales.PiecesPaladin[i]);
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
                    UpdateArmor(PiecesArmorNew);

                    Animations = _piecesAnim;

                    // Asigno control por default al jugador
                    Controls[(int)Globales.Controls.Up] = Keys.W;
                    Controls[(int)Globales.Controls.Down] = Keys.S;
                    Controls[(int)Globales.Controls.Left] = Keys.A;
                    Controls[(int)Globales.Controls.Right] = Keys.D;
                    Controls[(int)Globales.Controls.Button1] = Keys.T;
                    Controls[(int)Globales.Controls.Button2] = Keys.Y;

                    // Ralentizar los cuadros por segundo del personaje
                    // TiempoFrameEjecucion(1);
                }

                /// <summary>
                /// Actualizar animacion
                /// </summary>
                /// <param name="gameTime"></param>
                public override void Update(GameTime gameTime)
                {
                    foreach (var piezaAnimada in _piecesAnim)
                    {
                        piezaAnimada.Position = Position;
                        piezaAnimada.Update(gameTime);

                        // Para los stats de cada personaje (borrar mas tarde) GAB
                        _mensaje = Position;
                    }
                }

                /// <summary>
                /// Dibujar Jugador
                /// </summary>
                /// <param name="spriteBatch"></param>
                public override void Draw(SpriteBatch spriteBatch)
                {
                    foreach (var piezaAnimada in _piecesAnim)
                    {
                        piezaAnimada.Draw(spriteBatch, Direction, piezaAnimada.Color);
                    }

                    // Si no separo este proceso de dibujo desconcha las posiciones de las capas del jugador
                    // +++ Me parece que esto se soluciono cuando cambie el parametro de dibujo en el draw general +++
                    spriteBatch.DrawString(Globales.CheckStatusVar2,
                    "Frame Actual = " + Mensaje1 + Environment.NewLine +
                    "Frame Total = " + Mensaje2 + Environment.NewLine +
                    "Direccion Actual = " + Mensaje3 + Environment.NewLine +
                    "Accion Actual = " + Mensaje4 + Environment.NewLine +
                    "Alto = " + Mensaje5 + Environment.NewLine +
                    "Ancho = " + Mensaje6 + Environment.NewLine +
                    "X = " + Mensaje7 + Environment.NewLine +
                    "Y = " + Mensaje8 + Environment.NewLine +
                    "Vida = " + Mensaje9,
                    _mensaje, Color.DarkRed);

                    // rectangulos de colision para chequear
                    if (Globales.EnableRectangles)
                    {
                        DrawRectangle(PiecesAnim[0].GetPosition(), Globales.PuntoBlanco, spriteBatch);
                        DrawRectangle(Globales.RectangleCollision, Globales.PuntoBlanco, spriteBatch);
                        DrawRectangle(Globales.RectangleCollision2, Globales.PuntoBlanco, spriteBatch);
                    }
                }

                /// <summary>
                /// Actualizar cosas del jugador antes del update general. Aca va todo lo que es la logica del mismo, saltar pegar, etc.
                /// </summary>
                /// <param name="gameTime">El gametime del juego.</param>
                /// <param name="limitesJugador">Los limites que puso la camara con respecto a la pantalla que vemos.</param>
                /// <param name="altoNivel">La altura total del escenario.</param>
                /// <param name="anchoNivel">El ancho total del escenario.</param>
                public override void UpdatePlayer(GameTime gameTime, Rectangle limitesJugador, int altoNivel, int anchoNivel)
                {

                    Update(gameTime);

                    // Obtengo teclas presionadas
                    Globales.CurrentKeyboardState = Keyboard.GetState();

                    // Logica de las acciones, moverse, pegar, etc
                    ActionLogic();

                    // Logica de las colisiones al golpear
                    CollisionLogic();

                    // Aqui aplicamos los daños y todo lo correspondiente a los efectos de las acciones hechas anteriormente
                    EffectLogic();

                    // Hace que el jugador no salga de la pantalla reacomodandolo dentro de la misma.
                    // Tomamos como pantalla el rectangulo que genera la camara para acomodar al jugador y limitamos de acuerdo a estas medidas.
                    // El FrameEscalado es para acomodar al personaje de acuerdo a la nueva escala adquirida dependiendo de la pantalla fisica donde se ejecuta el juego.
                    var frameEscalado = Animations[0].GetPosition();
                    Position.X = MathHelper.Clamp(Position.X, limitesJugador.Left + frameEscalado.Width / 2, limitesJugador.Width - frameEscalado.Width / 2);
                    Position.Y = MathHelper.Clamp(Position.Y, altoNivel - altoNivel / 2, altoNivel - frameEscalado.Height / 2);

                    // No es necesario mas acomodar la fila ya que todos vienen con fila 0
                    // Solo se acomoda la cantidad de frames por animacion y que animacion va en cada pieza segun la accion ejecutandose.
                    #region ANIMACION POR PIEZA

                    foreach (var piezaAnimacion in _piecesAnim)
                    {
                        foreach (var textura in Globales.PaladinTextures)
                        {
                            var aver = PiecesArmor.Get_Set(textura.Piece);

                            if (textura.Piece == piezaAnimacion.PieceName &&
                                textura.Set == PiecesArmor.Get_Set(textura.Piece) &&
                                textura.Action == CurrentAction.ToString().ToLower())
                            {
                                piezaAnimacion.LoadTexture(textura);
                            }
                        }
                    }

                    // Vuelve a 0 el frame de la animacion si cambio de accion
                    if (OldAction != CurrentAction)
                    {
                        foreach (var animacion in _piecesAnim)
                        {
                            animacion.CurrentFrame = 0;
                        }

                        OldAction = CurrentAction;
                    }

                    #endregion

                    // Status del personaje
                    Mensaje1 = _piecesAnim[0].CurrentFrame;
                    Mensaje2 = _piecesAnim[0].FrameCount;
                    Mensaje3 = Direction;
                    Mensaje4 = CurrentAction;
                    Mensaje5 = _piecesAnim[0].GetPosition().Height;
                    Mensaje6 = _piecesAnim[0].GetPosition().Width;
                    Mensaje7 = _piecesAnim[0].GetPosition().X;
                    Mensaje8 = _piecesAnim[0].GetPosition().Y;
                }

                /// <summary>
                /// Cargo los set de armadura que corresponden a cada pieza del cuerpo.
                /// </summary>
                /// <param name="setPieces"> Shield, gauntlets, greaves, helm, breastplate, tasset, sword respectivamente </param> 
                public override void UpdateArmor(List<PieceSet> setPieces)
                {
                    // shield, gauntlets, greaves, helm, breastplate, tasset, sword
                    foreach (var setPiece in setPieces)
                    {
                        PiecesArmor.Set_Set(setPiece);
                    }

                    foreach (var piezaAnimacion in _piecesAnim)
                    {
                        foreach (var textura in Globales.PaladinTextures)
                        {
                            if (textura.Piece == piezaAnimacion.PieceName &&
                                textura.Set == PiecesArmor.Get_Set(textura.Piece) &&
                                textura.Action == CurrentAction.ToString().ToLower())
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
                    foreach (var animacion in Animations)
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
                    Animations[pieza].ColorChange(tinte);
                }

                /// <summary>
                /// Obtiene el frame actual de la animacion parandose en la primera pieza de la misma [0]
                /// </summary>
                /// <returns> Frame actual de la animacion </returns>
                public override int GetCurrentFrame()
                {
                    return Animations[0].CurrentFrame;
                }

                /// <summary>
                /// Obtiene cantidad total de frames de la animacion parandose en la primera pieza de la misma [0]
                /// </summary>
                /// <returns> Frames total de la animacion </returns>
                public override int GetAnimationFrames()
                {
                    return Animations[0].FrameCount;
                }

                /// <summary>
                /// Activa o desactiva al jugador (si no esta activo no se dibuja)
                /// </summary>
                public override void ActivatePlayer(bool active)
                {
                    foreach( var piece in Animations )
                    {
                        piece.Active = active;
                    }
                }

            #endregion

            #region PROPIOS

                /// <summary>
                /// Establece el tiempo de frame en ejecucion
                /// </summary>
                /// <param name="tiempo">El tiempo que va a durar el frame en pantalla de las distintas animaciones del personaje</param>
                void FrameSpeed(int tiempo)
                {
                    foreach (var piezaAnimada in _piecesAnim)
                    {
                        piezaAnimada.FrameTime = tiempo;
                    }
                }

                /// <summary>
                /// Pausa la animacion en el frame actual
                /// </summary>
                /// <param name="desactivar">pone o quita la pausa segun este parametro</param>
                void PauseAnimation(bool desactivar)
                {
                    foreach (var piezaAnimada in _piecesAnim)
                    {
                        piezaAnimada.Pause = desactivar;
                    }
                }

                /// <summary>
                /// Logica de todas las acciones, los movimientos, los golpes, etc.
                /// </summary>
                private void ActionLogic()
                {
                    if (CurrentAction != Globales.Actions.Hit1 &&
                        CurrentAction != Globales.Actions.Hit2 &&
                        CurrentAction != Globales.Actions.Hit3)
                    {

                        #region MOVIMIENTO

                        // Si no se toca nada quedara por default que esta parado
                        CurrentAction = Globales.Actions.Stand;

                        // Si se presiona alguna tecla de movimiento
                        if (Globales.CurrentKeyboardState.IsKeyDown(Controls[(int)Globales.Controls.Left]))
                        {
                            Position.X -= PlayerSpeed;
                            Direction = Globales.Mirada.Left;
                            CurrentAction = Globales.Actions.Walk;
                        }
                        else if (Globales.CurrentKeyboardState.IsKeyDown(Controls[(int)Globales.Controls.Right]))
                        {
                            Position.X += PlayerSpeed;
                            Direction = Globales.Mirada.Right;
                            CurrentAction = Globales.Actions.Walk;
                        }

                        if (Globales.CurrentKeyboardState.IsKeyDown(Controls[(int)Globales.Controls.Up]))
                        {
                            Position.Y -= PlayerSpeed;
                            CurrentAction = Globales.Actions.Walk;
                        }
                        else if (Globales.CurrentKeyboardState.IsKeyDown(Controls[(int)Globales.Controls.Down]))
                        {
                            Position.Y += PlayerSpeed;
                            CurrentAction = Globales.Actions.Walk;
                        }

                        #endregion


                        #region GOLPE

                        // Si presiono golpear cancela todas las demas acciones hasta que esta termine su ciclo
                        // Tambien genera un rango de los 3 diferentes tipos de golpes (algo netamente visual sin impacto en el juego)
                        if (Globales.CurrentKeyboardState.IsKeyDown(Controls[(int)Globales.Controls.Button1]))
                        {
                            // La aleatoriedad en los golpes depende de como estan almacenados en las variables globales, la primer variable es incluyente y la segunda excluyente.
                            CurrentAction = (Globales.Actions)Globales.Randomly.Next(2, 5);

                        }
                        #endregion
                    }
                    else
                    {
                        // Si esta pegando tiene que terminar su animacion y despues desbloquear otra vez la gama de movimientos
                        if (_piecesAnim[0].CurrentFrame == _piecesAnim[0].FrameCount - 1)
                        {
                            CurrentAction = Globales.Actions.Stand;

                            // Reseteo contador logico
                            LogicCounter = 0;
                        }
                    }
                }

                /// <summary>
                /// Logica de las colisiones de los golpes:
                /// 
                /// 1) Implementamos un chequeo jugador por jugador a la hora de golpear, que cumpla con las siguientes reglas:
                ///     - Si le toca chequear con el mismo se saltea.
                ///     - Si el frame de la animacion no es justo cuando golpea con la espada se saltea.
                ///     - Si fue golpeado anteriormente se saltea
                ///     - Si es fantasma se saltea
                ///     
                /// 2) Contador de vueltas logicas, independiente de lo que se dibuja por segundo.
                ///    Cuando este contador esta en 1, porque el mismo se resetea por animacion, puede entrar y hacer los calculos necesarios, sino no pasa.
                ///    De esta manera cuando se cambia de animacion se vuelve a empezar de 0 con el contador lógico.
                /// </summary>
                private void CollisionLogic()
                {
                    // GAB retocar
                    if ((CurrentAction == Globales.Actions.Hit1 || CurrentAction == Globales.Actions.Hit2 || CurrentAction == Globales.Actions.Hit3)
                       && !GhostMode)
                    {
                        foreach (var player in Globales.Players)
                        {
                            // 1) Ver summary
                            if (player != this && GetCurrentFrame() == 5 && !player.Injured && !player.GhostMode)
                            {
                                var temp = _piecesAnim[0].GetPosition();
                                var temp2 = player.Animations[0].GetPosition();

                                // Si esta dentro del radio del golpe
                                if (CollisionVerifier(ref temp, ref temp2))
                                {
                                    // Ver summary punto (2)
                                    if (LogicCounter == 0)
                                    {
                                        // Cuando la armadura esta detras del efecto de la espada no se puede ver bien el cambio de color
                                        player.ColorAnimationChange(Color.Red);

                                        player.Injured = true;
                                        player.InjuredValue = 10;
                                        LogicCounter++;
                                    }
                                }
                            }
                        }
                    }
                }

                /// <summary>
                /// Lógica de los efectos de las colisiones y movimientos realizados.
                /// </summary>
                private void EffectLogic()
                {

                    if (Injured && !GhostMode)
                    {
                        // Hago la resta necesaria a la health
                        Health -= InjuredValue;

                        // Vuelvo el contador de daño a 0 y quito que este dañado
                        InjuredValue = 0;
                        Injured = false;

                        // Si pierde toda su HP se vuelve fantasma
                        if (Health <= 0)
                        {
                            GhostMode = true;
                        }
                    }
                    else
                    {
                        // Reestablezco su color natural despues de recibir daño
                        ColorAnimationChange(Color.White);
                    }

                    if (GhostMode)
                    {
                        ColorAnimationChange(Globales.ColorGhost);

                        if (Health > 0)
                        {
                            GhostMode = false;
                        }
                    }

                    // MENSAJES: Veo la health de los personajes
                    Mensaje9 = Health;
                    //mensaje6 = Tiempo_Frame;
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
                            Direction == Globales.Mirada.Right)
                            ||
                           (temp.X <= temp2.Center.X + Globales.HitRangeX &&
                            temp.X + temp.Width >= temp2.X + temp2.Width &&
                            temp.Y >= temp2.Y - Globales.HitRangeY &&
                            temp.Y <= temp2.Y + Globales.HitRangeY &&
                            Direction == Globales.Mirada.Left);
                }

                /// <summary>
                /// Dibuja los rectangulos que delimitan las colisiones del personaje.
                /// </summary>
                /// <param name="rec">Rectangulo del personaje</param>
                /// <param name="tex">Textura utilizada para dibujar el rectangulo</param>
                /// <param name="spriteBatch">Proceso de dibujado</param>
                public static void DrawRectangle(Rectangle rec, Texture2D tex, SpriteBatch spriteBatch)
                {
                    var position = new Vector2(rec.X, rec.Y);
                    var border = 1;

                    var borderWidth = rec.Width + (border * 2);
                    var borderHeight = rec.Height + (border);

                    DrawStraightLine(new Vector2(rec.X, rec.Y), new Vector2(rec.X + rec.Width, rec.Y), tex, Color.White, spriteBatch, border);
                    DrawStraightLine(new Vector2(rec.X, rec.Y + rec.Height), new Vector2(rec.X + rec.Width, rec.Y + rec.Height), tex, Color.White, spriteBatch, border);
                    DrawStraightLine(new Vector2(rec.X, rec.Y), new Vector2(rec.X, rec.Y + rec.Height), tex, Color.White, spriteBatch, border);
                    DrawStraightLine(new Vector2(rec.X + rec.Width, rec.Y), new Vector2(rec.X + rec.Width, rec.Y + rec.Height), tex, Color.White, spriteBatch, border);
                }

                /// <summary>
                /// Dibuja lineas rectas
                /// </summary>
                /// <param name="a">Medida A</param>
                /// <param name="b">Medida B</param>
                /// <param name="tex">Textura utilizada para dibujar las lineas</param>
                /// <param name="col">Color de las lineas</param>
                /// <param name="spriteBatch">Proceso de dibujado</param>
                /// <param name="thickness">Grosor de la linea</param>
                public static void DrawStraightLine(Vector2 a, Vector2 b, Texture2D tex, Color col, SpriteBatch spriteBatch, int thickness)
                {
                    var rec = a.X < b.X ? new Rectangle((int)a.X, (int)a.Y, (int)(b.X - a.X), thickness) : 
                        new Rectangle((int)a.X, (int)a.Y, thickness, (int)(b.Y - a.Y));
                    spriteBatch.Draw(tex, rec, col);
                }

        #endregion
    
        #endregion

    }
}
