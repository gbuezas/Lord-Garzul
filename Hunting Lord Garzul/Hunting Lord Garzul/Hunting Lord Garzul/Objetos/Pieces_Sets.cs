using System.Collections.Generic;
using System.Linq;
using Hunting_Lord_Garzul.Generales;

namespace Hunting_Lord_Garzul.Objetos
{
    class PiecesSets
    {
        public List<PieceSet> Pieces = new List<PieceSet>();

        public void Initialize()
        {
            foreach (var piece in Globales.PiecesPaladin)
            {
                var newPiece = new PieceSet();
                newPiece.Initialize(piece, "set1");
                Pieces.Add(newPiece);
            }
        }

        public void Set_Set(PieceSet pieceChanged)
        {
            foreach (var piece in Pieces)
            {
                if (piece.Piece == pieceChanged.Piece)
                {
                    piece.Set = pieceChanged.Set;
                }
            }
        }

        public string Get_Set(string pieceSearched)
        {
            foreach (var piece in Pieces.Where(piece => pieceSearched == piece.Piece))
            {
                return piece.Set;
            }
            return string.Empty;
        }
    }
}
