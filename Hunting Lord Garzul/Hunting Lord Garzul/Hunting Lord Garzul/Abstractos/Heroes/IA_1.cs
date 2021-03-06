﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Hunting_Lord_Garzul.Objetos;

namespace Hunting_Lord_Garzul.Abstractos.Heroes
{
    class IA_1 : Jugadores
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
                private Animacion[] Pieces_Anim = new Animacion[Globales.PiecesIA_1.Length];

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

                /// <summary>
                /// Parametro de busqueda de objetivo a atacar
                /// </summary>
                protected Globales.TargetCondition TargetCond;

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
                    PlayerSpeed = 3.0f;
                    direction = Objetos.Globales.Mirada.RIGHT;
                    currentAction = Globales.Actions.STAND;
                    oldAction = currentAction;
                    FrameTime = 50;
                    health -= 70;

                    // Establezco las banderas de dañados
                    ResetInjured();

                    // Seteo IA
                    machine = true;

                    // Inicializo partes de armadura actual
                    pieces_armor.Initialize();

                    // Inicializo las piezas de animacion
                    for (int i = 0; i < Globales.PiecesIA_1.Length; i++)
                    {
                        Pieces_Anim[i] = new Animacion();
                        Pieces_Anim[i].Initialize(Globales.PiecesIA_1[i]);
                    }

                    // Piezas de la armadura al comenzar
                    UpdateArmor(pieces_armor_new);

                    this.animations = this.Pieces_Anim;

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
                    foreach (Animacion piezaAnimada in Pieces_Anim)
                    {
                        piezaAnimada.position = Position;
                        piezaAnimada.Update(gameTime);
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
                public override void UpdatePlayer(GameTime gameTime, Rectangle LimitesPantalla, int AltoNivel, int AnchoNivel)
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

                    foreach (Animacion piezaAnimacion in Pieces_Anim)
                    {
                        foreach (Texturas textura in Globales.IA_1Textures)
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

                }

                /// <summary>
                /// Cargo los set de armadura que corresponden a cada pieza del cuerpo.
                /// </summary>
                /// <param name="set_pieces">Set de shield, gauntlets, greaves, helm, breastplate, tasset, sword respectivamente</param> 
                public override void UpdateArmor(List<Piece_Set> set_pieces)
                {
                    // Gauntletback, greaveback, helm, breastplate, tasset, greavetop, gauntlettop. 
                    foreach (Piece_Set set_piece in set_pieces)
                    {
                        pieces_armor.Set_Set(set_piece);
                    }

                    foreach (Animacion piezaAnimacion in Pieces_Anim)
                    {
                        foreach (Texturas textura in Globales.IA_1Textures)
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
                /// Devuelve el valor con -1 porque empieza a contar desde 0
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
                    foreach (Animacion piece in this.animations)
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
                    // Busca blanco ara golpear segun los criterios dados
                    Jugadores target = GetTarget(this.TargetCond);

                    if (currentAction != Globales.Actions.HIT1 &&
                        currentAction != Globales.Actions.HIT2 &&
                        currentAction != Globales.Actions.HIT3)
                    {

                        #region MOVIMIENTO

                        // Si no esta en movimiento por default queda parado
                        currentAction = Globales.Actions.STAND;

                        // Dirigirse al blanco, dependiendo de donde esta ele eje del blanco vamos a sumarle la velocidad hacia el.
                        // Tambien se toma el lugar donde la IA va a detenerse y el punto que va a buscar para atacar a cierto personaja.
                        // Para obtener el lugar antes mencionado usamos la variable de HitRange asi se posiciona optimamente para su ataque.
                        // El HitRangeX tiene que ser mayor para que no hostigue tanto al blanco, sino se pega mucho a el
                        if (target.GetPosition().X <= this.Position.X - Globales.HitRangeX * 4)
                        {
                            // Izquierda
                            Position.X -= PlayerSpeed;
                            direction = Globales.Mirada.LEFT;
                            currentAction = Globales.Actions.WALK;
                        }
                        else if (target.GetPosition().X >= this.Position.X + Globales.HitRangeX * 4)
                        {
                            // Derecha
                            Position.X += PlayerSpeed;
                            direction = Globales.Mirada.RIGHT;
                            currentAction = Globales.Actions.WALK;
                        }

                        if (target.GetPosition().Y <= this.Position.Y - Globales.HitRangeY)
                        {
                            // Arriba
                            Position.Y -= PlayerSpeed;
                            currentAction = Globales.Actions.WALK;
                        }
                        else if (target.GetPosition().Y >= this.Position.Y + Globales.HitRangeY)
                        {
                            // Abajo
                            Position.Y += PlayerSpeed;
                            currentAction = Globales.Actions.WALK;
                        }

                        #endregion

                        #region GOLPEAR

                        // Obtengo las posiciones del blanco y nuestra
                        Rectangle temp = this.Pieces_Anim[0].GetPosition();
                        Rectangle temp2 = target.animations[0].GetPosition();

                        // Si el blanco esta dentro del rango de golpe se lo ataca
                        if (CollisionVerifier(ref temp, ref temp2))
                        {
                            // El rango depende de como estan almacenados en las variables globales, la primer variable es incluyente y la segunda excluyente.
                            currentAction = (Globales.Actions)Globales.randomly.Next(2, 5);
                        }

                        #endregion
                    }
                    else
                    {
                        // Si esta pegando tiene que terminar su animacion y despues desbloquear otra vez la gama de movimientos
                        // Para esto comparamos el frame actual de la animacion con su frame
                        if (this.GetCurrentFrame() == this.GetTotalFrames())
                        {
                            currentAction = Globales.Actions.STAND;

                            // Cuando termine la animacion de pegar puede generar daño de vuelta a alguien que ya haya atacado
                            ResetInjured();
                        }
                    }
                }

                /// <summary>
                /// Logica de las colisiones de los golpes:
                /// 
                ///     Implementamos un chequeo jugador por jugador a la hora de golpear, que cumpla con las siguientes reglas:
                ///     - Si el frame de la animacion no es justo cuando golpea con la espada se saltea.
                ///     - Si fue golpeado anteriormente se saltea
                ///     - Si es fantasma se saltea
                ///     - Si es IA se saltea
                /// </summary>
                private void CollisionLogic()
                {
                    if ((   this.currentAction == Globales.Actions.HIT1 || 
                            this.currentAction == Globales.Actions.HIT2 || 
                            this.currentAction == Globales.Actions.HIT3) &&
                            !this.ghost_mode)
                    {

                        for (int i = 0; i < Globales.totalQuant; i++)
                        {
                            // Ver sumamry
                            if (!Globales.players[i].machine &&
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
                /// Lógica de los efectos de las colisiones y movimientos realizados.
                /// </summary>
                private void EffectLogic()
                {

                    if (!this.ghost_mode)
                    {
                        // Reestablezco su color natural si no va a recibir daño, de esta manera no permito que vuelva a su color 
                        // demasiado rapido como para que no se vea que fue dañado
                        if (this.injured_value == 0)
                            this.ColorAnimationChange(Color.White);
                        
                        // Hago la resta necesaria a la health
                        this.health -= this.injured_value;

                        // Vuelvo el contador de daño a 0 y quito que este dañado
                        this.injured_value = 0;
                                                
                        // Si pierde toda su HP se vuelve fantasma
                        if (this.health <= 0)
                        {
                            this.ghost_mode = true;
                        }
                    }
                    else
                    {
                        // Lo manejo con el ghost a la IA tb asi no tengo que cambiar todo lo que esta hecho con los jugadores.
                        // De esta manera es mas facil porque las corroboraciones del ghost_mode siguen corriendo, 
                        // nada mas que no se dibujan las animaciones de la IA porque estan desactivadas
                        this.ActivatePlayer(false);
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
                            this.direction == Globales.Mirada.RIGHT)
                            ||
                           (temp.X <= temp2.Center.X + Globales.HitRangeX &&
                            temp.X + temp.Width >= temp2.X + temp2.Width &&
                            temp.Y >= temp2.Y - Globales.HitRangeY &&
                            temp.Y <= temp2.Y + Globales.HitRangeY &&
                            this.direction == Globales.Mirada.LEFT);
                }

                /// <summary>
                /// Obtiene condiciones al azar, se hace en inicializar para que se haga una sola vez en la creacion del personaje
                /// </summary>
                private void GetCondition()
                {
                    TargetCond = (Globales.TargetCondition)Globales.randomly.Next(0, 4);
                }

                /// <summary>
                /// Setea un objetivo segun los criterios de busqueda que se obtuvieron de GetCondition() en Initialize.
                /// Se hace en cada vuelta logica ya que recalcula los parametros por si hay que cambiar de blanco bajo los mismos criterios.
                /// </summary>
                /// <returns></returns>
                private Jugadores GetTarget(Globales.TargetCondition Condition)
                {
                    switch (Condition)
                    {

                        case Globales.TargetCondition.MAXHEALTH:
                            {
                                int vida = 0;
                                // GAB - Tiene que buscar un target nuevo o quedarse quieto cuando mata al que era su target
                                // Si le pongo -1 hace el efecto de querer cambiar de target pero si dejo -1 tira out of index
                                int playerMaxHealth = 0;

                                for (int i = 0; i < Globales.playersQuant; i++)
                                {
                                    if (Globales.players[i].health > vida && Globales.players[i].health > 0)
                                    {
                                        vida = Globales.players[i].health;
                                        playerMaxHealth = i;
                                    }
                                }

                                return Globales.players[playerMaxHealth];
                            }

                        case Globales.TargetCondition.MINHEALTH:
                            {
                                int vida = 1000;
                                int playerMinHealth = 0;

                                for (int i = 0; i < Globales.playersQuant; i++)
                                {
                                    if (Globales.players[i].health < vida && Globales.players[i].health > 0)
                                    {
                                        vida = Globales.players[i].health;
                                        playerMinHealth = i;
                                    }
                                }

                                return Globales.players[playerMinHealth];
                            }

                        case Globales.TargetCondition.MAXMONEY:
                            {
                                return Globales.players[2];
                            }

                        case Globales.TargetCondition.MINMONEY:
                            {
                                return Globales.players[3];
                            }

                        default: break;

                    }


                    return Globales.players[0];
                }

            #endregion

        #endregion

    }
}
