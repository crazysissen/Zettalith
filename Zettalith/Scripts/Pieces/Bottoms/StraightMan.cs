using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class StraightMan : Bottom
    {
        public StraightMan()
        {
            Name = "Jhin";
            Health = 20;
            AttackDamage = 3;
            ManaCost = new Mana(0, 1, 0);
            MoveRange = 4;
            Description = "Moves " + MoveRange + " tiles in a straight line.";
            Texture = Load.Get<Texture2D>("TestSubpiece2");
        }

        public override List<Point> RequestMove(Point origin)
        {
            return Movement.Straight(origin, MoveRange);
        }

        public override void ActivateMove(TilePiece piece, Point target)
        {
            InGameController.Grid.ChangePosition(piece, target.X, target.Y);
        }
    }
}
