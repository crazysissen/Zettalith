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

        public TilePiece(InGamePiece inGamePiece)
        {
            Piece = inGamePiece;
        }
    }
}
