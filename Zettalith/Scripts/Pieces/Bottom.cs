using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith.Pieces
{
    abstract class Bottom : SubPiece
    {
        public Mana MoveCost { get; set; }

        public int ToIndex()
        {
            return Bottoms.bottoms.IndexOf(GetType());
        }

        public Bottom FromIndex(int index)
        {
            Bottom tempBottom = (Bottom)Activator.CreateInstance(Bottoms.bottoms[index]);
            return tempBottom;
        }
    }
}