using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zettalith.Pieces;

namespace Zettalith
{
    class Deck
    {
        public List<InGamePiece> Pieces { get; private set; }

        public Deck(Set set)
        {
            Pieces = new List<InGamePiece>();

            if (set.Pieces != null)
            {
                foreach (Piece piece in set.Pieces)
                {
                    InGamePiece newPiece = new InGamePiece(piece);
                    newPiece.Texture = GameRendering.GetTexture(piece.TopIndex, piece.MiddleIndex, piece.BottomIndex);
                    Pieces.Add(newPiece);
                }
            }
        }

        public InGamePiece Draw()
        {
            InGamePiece piece = Pieces[0];
            Pieces.RemoveAt(0);
            return piece;
        }

        public InGamePiece DrawSpecific(Func<InGamePiece, bool> criteria, out bool successful)
        {
            InGamePiece[] foundPieces = Pieces.Where(criteria).ToArray();

            if (foundPieces.Length == 0)
            {
                successful = false;
                return null;
            }

            Pieces.Remove(foundPieces[0]);
            successful = true;
            return foundPieces[0];
        }

        public void Shuffle(int shuffleSeed)
        {
            Random random = new Random(shuffleSeed);

            List<InGamePiece> newList = new List<InGamePiece>();

            for (int i = Pieces.Count - 1; i >= 0; --i)
            {
                int index = random.Next(i);
                InGamePiece piece = Pieces[index];
                Pieces.RemoveAt(index);
                newList.Add(piece);
            }

            Pieces = newList;
        }
    }
}
