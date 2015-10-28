using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hunting_Lord_Garzul.Objetos
{
    public class Piece_Set
    {
        // Pieza del cuerpo
        private string Piece;
        public string piece
        {
            get { return Piece; }
        }
        
        // Set de armadura de la pieza del cuerpo
        private string Set;
        public string set
        {
            get { return Set; }
            set { Set = value; }
        }
        
        public void Initialize(string piece, string set)
        {
            this.Piece = piece;
            this.Set = set;
        }
    }
}
