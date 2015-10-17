using System;
using System.Collections.Generic;
using Hunting_Lord_Garzul.Abstractos.Estados;
using Hunting_Lord_Garzul.Abstractos.Heroes;
using Hunting_Lord_Garzul.Objetos;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Hunting_Lord_Garzul.Generales
{
    public class Globales
    {
        // Fuente de los mensajes de chequeo
        public static SpriteFont CheckStatusVar;
        public static SpriteFont CheckStatusVar2;
        public static float Mensaje1;
        public static float Mensaje2;
        public static float Mensaje3;
        public static float Mensaje4;
        public static float Mensaje5;
        
        // Colores
        public static Color ColorGhost = new Color(255, 255, 255, 30);
        public static Color ColorEnemy = Color.White;

        // La clase de los estados del juego
        public static Estados CurrentState;

        // Determina lo que se presiono en el teclado y gamepad
        public static KeyboardState CurrentKeyboardState = new KeyboardState();
        public static KeyboardState PreviousKeyboardState = new KeyboardState();
        //public static GamePadState[] currentGamePadState = new GamePadState[4];
        //public static GamePadState[] previousGamePadState = new GamePadState[4];

        // Dimensiones de la pantalla
        public static int ViewportHeight;
        public static int ViewportWidth;
        
        // Dimensiones del frame de los personajes y escala de los mismos
        public static int FrameHeight = 240;
        public static int FrameWidth = 320;
        // Mas grande es el numero mas chico es el personaje
        public static int Scalar = 6;

        // Crea lista de jugadores y enemigos
        public static List<Jugadores> Players = new List<Jugadores>();
        public static int PlayersQuant = 4;
        public static int EnemiesQuant = 8;

        // Para donde mira el personaje
        public enum Mirada { Right, Left };
        
        // Las distintas acciones que puede hacer con sus respectivos frames y los controles
        public enum Actions { Walk, Stand, Hit1, Hit2, Hit3, Dead }
        public enum Controls { Up, Down, Left, Right, Button1, Button2 }
        
        // Los distintos estados del juego
        public enum EstadosJuego { Intro, Titulo, Seleccion, Mapa, Vs, Avance, Pausa, Gameover, Final }

        // Los distintos parametros de busqueda de objetivo de la IA
        public enum TargetCondition { Maxhealth, Minhealth, Maxmoney, Minmoney }

        // Los distintos heroes
        public static string[] Heroes = new string[7] { "Paladin", "Paladina", "Barbaro", "Barbara", "Arquero", "Arquera", "IA_1" };
        
        // El orden de los items influye directamente en el orden en el que se dibujan las piezas.
        // Cada clase tiene su set de items (paladin, barbaro, etc)
        public static string[] PiecesPaladin = { "shield", "gauntletback", "greaveback", "breastplate", "helm", "tasset", "greavetop", "sword", "gauntlettop" };
        public static string[] PiecesBarbaro = { "gauntletback", "greaveback", "breastplate", "helm", "tasset", "greavetop", "sword", "gauntlettop" };
        public static string[] PiecesIa1 = { "gauntletback", "greaveback", "breastplate", "helm", "tasset", "greavetop", "gauntlettop" };
        public static List<string> Armors = new List<string>();

        // Los distintos estilos de escenarios
        public static string[] Scenes = { "Avance", "Versus", "Titulo" };

        // Las distintas armaduras o skins que puede llevar y las piezas de animacion
        public static List<Texturas> PaladinTextures = new List<Texturas>();
        public static List<Texturas> Ia1Textures = new List<Texturas>();
        
        // Los distintos niveles de avance
        public static List<Texturas> AvanceTextures = new List<Texturas>();

        // Los distintos niveles de versus
        public static List<Texturas> VersusTextures = new List<Texturas>();

        // Las distintas capas de parallax
        public static List<Parallax> Layers = new List<Parallax>();

        // Texturas de titulos
        public static Texture2D PantallaTitulo;
        // Textura de Seleccion
        public static Texture2D PantallaSeleccion;
        public static Texture2D Selector;
        
        // Rectangulos de colisiones para chequear y su textura
        public static Rectangle RectangleCollision;
        public static Rectangle RectangleCollision2;
        public static Texture2D PuntoBlanco;
        public static bool EnableRectangles = false;

        // Para llevar la cuenta de los frames por segundo
        public static TimeSpan ElapsedTime = TimeSpan.Zero;
        public static int FrameRate = 0;
        public static int FrameCounter = 0;

        /// <summary>
        /// Rango que admite el golpe sobre el eje X e Y
        /// </summary>
        public static int HitRangeY = 7;
        public static int HitRangeX = 15;

        /// <summary>
        /// Variable random
        /// </summary>
        public static Random Randomly = new Random();
    }
}
