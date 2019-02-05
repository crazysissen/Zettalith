using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith.Pieces
{
    abstract class Middle : SubPiece
    {
        protected int ToIndex()
        {
            return Middles.middles.IndexOf(GetType());
        }

        protected Middle FromIndex()
        {
            Middle tempMiddle = (Middle)Activator.CreateInstance(Middles.middles[Index]);
            return tempMiddle;
        }
    }
}