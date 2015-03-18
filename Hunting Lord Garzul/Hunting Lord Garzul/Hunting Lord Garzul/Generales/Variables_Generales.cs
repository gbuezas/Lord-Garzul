using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Hunting_Lord_Garzul.Objetos
{
    public class Variables_Generales
    {
        // Fuente de los mensajes de chequeo
        public static SpriteFont CheckStatusVar;
        public static SpriteFont CheckStatusVar_2;
        public static float mensaje1;
        public static float mensaje2;
        public static float mensaje3;
        public static float mensaje4;
        public static float mensaje5;
        public static string mensaje6;

        // La clase de los estados del juego
        public static Estados Estado_Actual;

        // Determina lo que se presiono en el teclado y gamepad
        public static KeyboardState currentKeyboardState = new KeyboardState();
        public static KeyboardState previousKeyboardState = new KeyboardState();
        //public static GamePadState[] currentGamePadState = new GamePadState[4];
        //public static GamePadState[] previousGamePadState = new GamePadState[4];

        // Dimensiones de la pantalla
        public static int AltoViewport;
        public static int AnchoViewport;
        
        // Crea lista de jugadores
        public static List<Jugadores> players = new List<Jugadores>();

        // Para donde mira el personaje
        public enum Mirada { DERECHA, IZQUIERDA };
        
        // Las distintas acciones que puede hacer con sus respectivos frames y los controles
        public enum Acciones { WALK = 12, STAND = 10, HIT_1 = 8, HIT_2 = 8, HIT_3 = 8, DEAD = 8 }
        public enum Controles { ARRIBA = 0, ABAJO, IZQUIERDA, DERECHA, BOTON_1, BOTON_2 }
        
        // Los distintos estados del juego
        public enum EstadosJuego { INTRO, TITULO, SELECCION, MAPA, VS, AVANCE, PAUSA, GAMEOVER, FINAL }

        // Los distintos heroes
        public static string[] Heroes = new string[6] { "Paladin", "Paladina", "Barbaro", "Barbara", "Arquero", "Arquera" };
        
        // Los distintos estilos de escenarios
        public static string[] Escenarios = new string[] { "Avance", "Versus", "Titulo" };

        // Las distintas armaduras o skins que puede llevar
        public static List<String> Tipo_Armadura = new List<String>();
        public static List<Texturas> TexturasPaladin = new List<Texturas>();
        public static List<Texturas> TexturasBarbaro = new List<Texturas>();

        // Los distintos niveles de avance
        public static List<Texturas> TexturasAvance = new List<Texturas>();

        // Los distintos niveles de versus
        public static List<Texturas> TexturasVersus = new List<Texturas>();

        // Las distintas capas de parallax
        public static List<Parallax> Capas = new List<Parallax>();

        // Texturas de titulos
        public static Texture2D Pantalla_Titulo;
        // Textura de Seleccion
        public static Texture2D Pantalla_Seleccion;
        public static Texture2D Selector;
        
    }
}
