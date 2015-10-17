namespace Hunting_Lord_Garzul.Objetos
{
    public class PieceSet
    {
        // Pieza del cuerpo
        public string Piece { get; private set; }

        // Set de armadura de la pieza del cuerpo
        public string Set { get; set; }

        public void Initialize(string piece, string set)
        {
            Piece = piece;
            Set = set;
        }
    }
}
