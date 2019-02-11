using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class Diagonal : Bottom
    {
        public Diagonal()
        {
            ManaCost = new Mana(1, 2, 3);
            Texture = Load.Get<Texture2D>("TestSubpiece");
        }

        public override List<Point> RequestMove(Point origin)
        {
            return Movement.Diagonal(origin, MoveRange);
        }
    }
}
