using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith.Pieces
{
    abstract class Bottom : SubPiece
    {
        public abstract Mana MoveCost { get; set; }

    }
}
