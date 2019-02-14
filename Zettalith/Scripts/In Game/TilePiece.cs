using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    class TilePiece : TileObject
    {
        public InGamePiece Piece { get; private set; }

        public int Player { get; set; }

        public TilePiece(InGamePiece inGamePiece, int player)
        {
            Piece = inGamePiece;
            Player = player;
        }
    }
}
