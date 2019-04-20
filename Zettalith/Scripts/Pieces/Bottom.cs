using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Zettalith
{
    abstract class Bottom : SubPiece
    {
        public int MoveRange { get; set; }

        public float MovementTime { get; set; }

        public virtual List<Point> RequestMove(Point origin)
        {
            return null;
        }

        public virtual void ActivateMove(TilePiece piece, Point target)
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