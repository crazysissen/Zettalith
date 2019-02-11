using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class Straight : Bottom
    {
        public Straight()
        {
            Name = "Tower Man";
            Health = 8;
            AttackDamage = 4;
            ManaCost = new Mana(3, 0, 1);
            Description = "One straight boy.";
            Texture = Load.Get<Texture2D>("TestSubpiece");
            MoveRange = 5;
        }

        public override List<Point> RequestMove(Point origin)
        {
            return Movement.Straight(origin, MoveRange);
        }
    }
}