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
        public abstract List<Point> RequestAttack();
        public abstract void ActivateAttack();

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