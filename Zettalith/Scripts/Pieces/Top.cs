using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Zettalith.Pieces
{
    abstract class Top : SubPiece
    {
        // TODO: Make abstract
        public virtual List<Point> RequestAttack(Point origin)
        {
            return null;
        }

        public virtual void ActivateAttack(Point tile)
        {

        }

        //public int ToIndex()
        //{
        //    return Tops.tops.IndexOf(GetType());
        //}

        //public Top FromIndex(int index)
        //{
        //    return (Top)Activator.CreateInstance(Tops.tops[index]);
        //}
    }
}