using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith.Pieces
{
    abstract class Middle : SubPiece
    {
        public int ToIndex()
        {
            return Middles.middles.IndexOf(GetType());
        }

        public Middle FromIndex(int index)
        {
            Middle tempMiddle = (Middle)Activator.CreateInstance(Middles.middles[index]);
            return tempMiddle;
        }
    }
}