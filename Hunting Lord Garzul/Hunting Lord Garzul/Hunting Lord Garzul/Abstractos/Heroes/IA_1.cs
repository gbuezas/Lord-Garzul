using System.Collections.Generic;
using Hunting_Lord_Garzul.Generales;
using Hunting_Lord_Garzul.Objetos;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hunting_Lord_Garzul.Abstractos.Heroes
{
    class Ia1 : Jugadores
    {

        # region VARIABLES

            #region CONTROLES

                /// <summary>
                /// Accion que se realiza 
                /// </summary>
                private Globales.Actions _currentAction;

                /// <summary>
                /// Accion realizada anteriormente
                /// </summary>
                private Globales.Actions _oldAction;

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
                private Animacion[] _piecesAnim = new Animacion[Globales.PiecesIa1.Length];

                /// <summary>
                /// Posicion del jugador relativa a la parte superior izquierda de la pantalla.
                /// Esta posicion marca donde se encuentra el jugador en la pantalla y no en el mapa donde se esta moviendo,
                /// y es a esta posicion a la que se le aplican los limites de la pantalla.  
                /// </summary>
                protected Vector2 Position;
        
                /// <summary>
                /// Ancho y alto de un cuadro del sprite, el tamaño que esta en la carpeta, el fisico.
                /// </summary>
                //protected int FrameWidth = Globales.FrameWidth;
                //protected int FrameHeight = Globales.FrameHeight;
                protected int FrameWidth = 400;
                protected int FrameHeight = 300;

            #endregion

            #region JUGABILIDAD

                /// <summary>
                /// Tipo de cada pieza de armadura:
                /// Gauntletback, greaveback, helm, breastplate, tasset, greavetop, gauntlettop. 
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

                /// <summary>
                /// Parametro de busqueda de objetivo a atacar
                /// </summary>
                protected Globales.TargetCondition TargetCond;

            #endregion

        #endregion

        #region METODOS

            #region GET-SET

                public Globales.Actions CurrentAction
                {
                    get { return _currentAction; }
                    set { _currentAction = value; }
                }

                public Globales.Actions OldAction
                {
                    get { return _oldAction; }
                    set { _oldAction = value; }
                }

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
                    PlayerSpeed = 3.0f;
                    Direction = Globales.Mirada.Right;
                    CurrentAction = Globales.Actions.Stand;
                    OldAction = CurrentAction;
                    FrameTime = 50;
                    Health -= 70;

                    // Seteo IA
                    Machine = true;

                    // Inicializo partes de armadura actual
                    PiecesArmor.Initialize();

                    // Inicializo las piezas de animacion
                    for (var i = 0; i < Globales.PiecesIa1.Length; i++)
                    {
                        _piecesAnim[i] = new Animacion();
                        _piecesAnim[i].Initialize(Globales.PiecesIa1[i]);
                    }

                    // Piezas de la armadura al comenzar
                    UpdateArmor(PiecesArmorNew);

                    Animations = _piecesAnim;

                    // Seteo condicion de busqueda de objetivo para atacar
                    GetCondition();

                    // Ralentizar los cuadros por segundo del personaje
                    // TiempoFrameEjecucion(1);

                    //this.ActivatePlayer(true);
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
                }

                /// <summary>
                /// Actualizar cosas del jugador antes del update general. Aca va todo lo que es la logica del mismo, saltar pegar, etc.
                /// </summary>
                /// <param name="gameTime">El gametime del juego.</param>
                /// <param name="currentKeyboardState">Para el teclado.</param>
                /// <param name="currentGamePadState">Para los gamepad.</param>
                /// <param name="limitesJugador">Los limites que puso la camara con respecto a la pantalla que vemos.</param>
                /// <param name="altoNivel">La altura total del escenario.</param>
                /// <param name="anchoNivel">El ancho total del escenario.</param>
                public override void UpdatePlayer(GameTime gameTime, Rectangle limitesJugador, int altoNivel, int anchoNivel)
                {

                    Update(gameTime);

                    // Logica de las acciones, moverse, pegar, etc
                    ActionLogic();

                    // Logica de las colisiones al golpear
                    CollisionLogic();

                    // Aqui aplicamos los daños y todo lo correspondiente a los efectos de las acciones hechas anteriormente
                    EffectLogic();

                    // No es necesario mas acomodar la fila ya que todos vienen con fila 0
                    // Solo se acomoda la cantidad de frames por animacion y que animacion va en cada pieza segun la accion ejecutandose.
                    #region ANIMACION POR PIEZA

                    foreach (var piezaAnimacion in _piecesAnim)
                    {
                        foreach (var textura in Globales.Ia1Textures)
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

                }

                /// <summary>
                /// Cargo los set de armadura que corresponden a cada pieza del cuerpo.
                /// </summary>
                /// <param name="setPieces">Set de shield, gauntlets, greaves, helm, breastplate, tasset, sword respectivamente</param> 
                public override void UpdateArmor(List<PieceSet> setPieces)
                {
                    // Gauntletback, greaveback, helm, breastplate, tasset, greavetop, gauntlettop. 
                    foreach (var setPiece in setPieces)
                    {
                        PiecesArmor.Set_Set(setPiece);
                    }

                    foreach (var piezaAnimacion in _piecesAnim)
                    {
                        foreach (var textura in Globales.Ia1Textures)
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
                    foreach (var piece in Animations)
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
                    // Busca blanco ara golpear segun los criterios dados
                    var target = GetTarget(TargetCond);

                    if (CurrentAction != Globales.Actions.Hit1 &&
                        CurrentAction != Globales.Actions.Hit2 &&
                        CurrentAction != Globales.Actions.Hit3)
                    {

                        #region MOVIMIENTO

                        // Si no esta en movimiento por default queda parado
                        CurrentAction = Globales.Actions.Stand;

                        // Dirigirse al blanco, dependiendo de donde esta ele eje del blanco vamos a sumarle la velocidad hacia el.
                        // Tambien se toma el lugar donde la IA va a detenerse y el punto que va a buscar para atacar a cierto personaja.
                        // Para obtener el lugar antes mencionado usamos la variable de HitRange asi se posiciona optimamente para su ataque.
                        // El HitRangeX tiene que ser mayor para que no hostigue tanto al blanco, sino se pega mucho a el
                        if (target.GetPosition().X <= Position.X - Globales.HitRangeX * 4)
                        {
                            // Izquierda
                            Position.X -= PlayerSpeed;
                            Direction = Globales.Mirada.Left;
                            CurrentAction = Globales.Actions.Walk;
                        }
                        else if (target.GetPosition().X >= Position.X + Globales.HitRangeX * 4)
                        {
                            // Derecha
                            Position.X += PlayerSpeed;
                            Direction = Globales.Mirada.Right;
                            CurrentAction = Globales.Actions.Walk;
                        }

                        if (target.GetPosition().Y <= Position.Y - Globales.HitRangeY)
                        {
                            // Arriba
                            Position.Y -= PlayerSpeed;
                            CurrentAction = Globales.Actions.Walk;
                        }
                        else if (target.GetPosition().Y >= Position.Y + Globales.HitRangeY)
                        {
                            // Abajo
                            Position.Y += PlayerSpeed;
                            CurrentAction = Globales.Actions.Walk;
                        }

                        #endregion

                        #region GOLPEAR

                        // Obtengo las posiciones del blanco y nuestra
                        var temp = _piecesAnim[0].GetPosition();
                        var temp2 = target.Animations[0].GetPosition();

                        // Si el blanco esta dentro del rango de golpe se lo ataca
                        if (CollisionVerifier(ref temp, ref temp2))
                        {
                            // El rango depende de como estan almacenados en las variables globales, la primer variable es incluyente y la segunda excluyente.
                            CurrentAction = (Globales.Actions)Globales.Randomly.Next(2, 5);
                        }

                        #endregion
                    }
                    else
                    {
                        // Si esta pegando tiene que terminar su animacion y despues desbloquear otra vez la gama de movimientos
                        if (GetCurrentFrame() == _piecesAnim[0].FrameCount - 1)
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
                ///     - Si es otra IA se saltea
                ///     
                /// 2) Contador de vueltas logicas, independiente de lo que se dibuja por segundo.
                ///    Cuando este contador esta en 1, porque el mismo se resetea por animacion, puede entrar y hacer los calculos necesarios, sino no pasa.
                ///    De esta manera cuando se cambia de animacion se vuelve a empezar de 0 con el contador lógico.
                /// </summary>
                private void CollisionLogic()
                {
                    if ((CurrentAction == Globales.Actions.Hit1 || CurrentAction == Globales.Actions.Hit2 || CurrentAction == Globales.Actions.Hit3)
                       && !GhostMode)
                    {
                        foreach (var jugador in Globales.Players)
                        {
                            // 1) Ver summary
                            if (jugador != this && GetCurrentFrame() == 5 && !jugador.Injured && !jugador.GhostMode && !jugador.Machine)
                            {
                                var temp = _piecesAnim[0].GetPosition();
                                var temp2 = jugador.Animations[0].GetPosition();

                                // Si esta dentro del radio del golpe
                                if (CollisionVerifier(ref temp, ref temp2))
                                {
                                    // Ver summary punto (2)
                                    if (LogicCounter == 0)
                                    {
                                        // Cuando la armadura esta detras del efecto de la espada no se puede ver bien el cambio de color
                                        jugador.ColorAnimationChange(Color.Red);

                                        jugador.Injured = true;
                                        jugador.InjuredValue = 10;
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
                        ColorAnimationChange(Globales.ColorEnemy);
                    }

                    if (GhostMode)
                    {
                        // GAB retocar
                        ColorAnimationChange(Globales.ColorGhost);

                        if (Health > 0)
                        {
                            GhostMode = false;
                        }
                        else
                        {
                            ActivatePlayer(false);
                        }
                    }
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
                /// Obtiene condiciones al azar, se hace en inicializar para que se haga una sola vez en la creacion del personaje
                /// </summary>
                private void GetCondition()
                {
                    TargetCond = (Globales.TargetCondition)Globales.Randomly.Next(0, 4);
                }

                /// <summary>
                /// Setea un objetivo segun los criterios de busqueda que se obtuvieron de GetCondition() en Initialize.
                /// Se hace en cada vuelta logica ya que recalcula los parametros por si hay que cambiar de blanco bajo los mismos criterios.
                /// </summary>
                /// <returns></returns>
                private Jugadores GetTarget(Globales.TargetCondition condition)
                {
                    switch (condition)
                    {

                        case Globales.TargetCondition.Maxhealth:
                            {
                                var vida = 0;
                                // GAB - Tiene que buscar un target nuevo o quedarse quieto cuando mata al que era su target
                                // Si le pongo -1 hace el efecto de querer cambiar de target pero si dejo -1 tira out of index
                                var playerMaxHealth = 0;

                                for (var i = 0; i < Globales.PlayersQuant; i++)
                                {
                                    if (Globales.Players[i].Health > vida && Globales.Players[i].Health > 0)
                                    {
                                        vida = Globales.Players[i].Health;
                                        playerMaxHealth = i;
                                    }
                                }

                                return Globales.Players[playerMaxHealth];
                            }

                        case Globales.TargetCondition.Minhealth:
                            {
                                var vida = 1000;
                                var playerMinHealth = 0;

                                for (var i = 0; i < Globales.PlayersQuant; i++)
                                {
                                    if (Globales.Players[i].Health < vida && Globales.Players[i].Health > 0)
                                    {
                                        vida = Globales.Players[i].Health;
                                        playerMinHealth = i;
                                    }
                                }

                                return Globales.Players[playerMinHealth];
                            }

                        case Globales.TargetCondition.Maxmoney:
                            {
                                return Globales.Players[2];
                            }

                        case Globales.TargetCondition.Minmoney:
                            {
                                return Globales.Players[3];
                            }

                        default: break;

                    }


                    return Globales.Players[0];
                }

            #endregion

        #endregion

    }
}
