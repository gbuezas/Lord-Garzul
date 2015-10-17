using Microsoft.Xna.Framework.Graphics;

namespace Hunting_Lord_Garzul.Objetos
{
    public class Texturas
    {
        #region VARIABLES

        // La textura a utilizar
        public Texture2D Textura { get; set; }

        // Nombre completo de la textura
        public string NombreTextura { get; set; }

        // Set de armadura a la que pertenece esta pieza
        private readonly string _set;
        public string Set
        {
            get { return _set; }
        }
        
        // Pieza que representa la textura
        private readonly string _piece;
        public string Piece
        {
            get { return _piece; }
        }
        
        // Accion que realiza esta pieza
        private readonly string _action;
        public string Action
        {
            get { return _action; }
        }
        
        // Frames de la animacion
        public string Frame { get; set; }

        #endregion

        #region METODOS

        /// <summary>
        /// Cargamos los sprites a utilizar y los datos necesarios para poder utilizarlos.
        /// Mas tarde se van a contrastar con los datos del jugador para utilizarlos correctamente.
        /// Ejemplo: set1_gauntlettop_walk_10.png
        /// </summary>
        /// <param name="textura">Los sprites que se van a utilizar</param>
        /// <param name="nombre">El nombre del archivo con sus datos pertinentes</param>
        public Texturas(Texture2D textura, string nombre)
        {
            Textura = textura;
            NombreTextura = nombre;
            
            var separador = nombre.Split('_');
            
            _set = separador[0];
            _piece = separador[1];
            _action = separador[2];
            Frame = separador[3];
        }

        #endregion
    }
}
