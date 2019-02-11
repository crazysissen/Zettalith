using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Zettalith.Pieces
{
    abstract class Bottom : SubPiece
    {
        public int MoveRange { get; set; }

        public virtual List<Point> RequestMove(Point origin)
        {
            return null;
        }

        public virtual void ActivateMove(Point tile)
        {

        }

        //public int ToIndex()
        //{
        //    return Bottoms.bottoms.IndexOf(GetType());
        //}

        //public Bottom FromIndex(int index)
        //{
        //    return (Bottom)Activator.CreateInstance(Bottoms.bottoms[index]);
        //}
    }
}