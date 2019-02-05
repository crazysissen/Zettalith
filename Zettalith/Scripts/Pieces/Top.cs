using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith.Pieces
{
    abstract class Top : SubPiece
    {
        protected int ToIndex()
        {
            return Tops.tops.IndexOf(GetType());
        }

        protected Top FromIndex()
        {
            Top tempTop = (Top)Activator.CreateInstance(Tops.tops[Index]);
            return tempTop;
        }
    }
}