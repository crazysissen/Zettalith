using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith.Pieces
{
    abstract class Top : SubPiece
    {
        public abstract Mana AbilityCost { get; set; }

    }
}
