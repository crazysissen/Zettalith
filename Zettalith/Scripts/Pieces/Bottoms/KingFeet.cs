using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class KingFeet : Bottom
    {
        public KingFeet()
        {
            Name = "King Feet";
            Health = 10;
            AttackDamage = 2;
            MoveRange = 1;
            MovementTime = 2;
            Texture = Load.Get<Texture2D>("SixtenBottom3");

            Description = "The finest boots in the entire kingdom";
        }

        public override List<Point> RequestMove(Point origin)
        {
            return Movement.Teleport(origin, new Point(MoveRange, MoveRange));
        }

        public override void ActivateMove(TilePiece piece, Point target)
        {
            InGameController.Grid.ChangePosition(piece, target.X, target.Y);
        }
    }
}
