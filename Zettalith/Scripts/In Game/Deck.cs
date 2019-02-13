using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    class Deck
    {
        public Stack<InGamePiece> Pieces { get; private set; }

        public Deck(Piece[] pieces)
        {
            Pieces = new Stack<InGamePiece>();

            foreach (Piece piece in pieces)
            {
                InGamePiece newPiece = new InGamePiece(piece);
                newPiece.Texture = PlayerRendering.GetTexture(piece.TopIndex, piece.MiddleIndex, piece.BottomIndex);
            }
        }

        public void Shuffle()
        {

        }
    }
}
