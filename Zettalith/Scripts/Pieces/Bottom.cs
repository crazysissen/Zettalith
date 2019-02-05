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

        protected int ToIndex()
        {
            return Bottoms.bottoms.IndexOf(GetType());
        }

        protected Bottom FromIndex()
        {
            Bottom tempBottom = (Bottom)Activator.CreateInstance(Bottoms.bottoms[Index]);
            return tempBottom;
        }
    }
}