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
            ManaCost = new Mana(4, 0, 0);
            MoveCost = new Mana(0, 4, 0);
            Texture = Load.Get<Texture2D>("BombTop1");

            Description = "Swaps place with another Zettalith";
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
