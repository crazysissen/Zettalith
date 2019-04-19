using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith.Pieces
{
    class Swap : Bottom
    {
        public Swap()
        {
            Name = "Swap";
            Health = 2;
            AttackDamage = 0;
            ManaCost = new Mana(0, 3, 5);
            MoveCost = new Mana(0, 5, 5);
            Texture = Load.Get<Texture2D>("SwapBottom");

            Description = "Swaps place with another Zettalith.";
        }

        public override List<Point> RequestMove(Point origin)
        {
            return Movement.Target(origin);
        }

        public override void ActivateMove(TilePiece piece, Point target)
        {
            InGameController.Grid.SwapPosition(piece, target);
        }
    }
}
