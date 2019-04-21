using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class Queen : Bottom
    {
        public Queen()
        {
            Name = "Queen";
            Health = 5;
            AttackDamage = 0;
            ManaCost = new Mana(0, 0, 4);
            MoveCost = new Mana(0, 0, 3);
            MoveRange = 3;
            MovementTime = 1;
            Texture = Load.Get<Texture2D>("HorseBottom");

            Description = "Moves " + MoveRange + " tiles in any given direction.";
        }

        public override List<Point> RequestMove(Point origin)
        {
            return Movement.Straight(origin, MoveRange).Concat(Movement.Diagonal(origin, MoveRange)).ToList();
        }

        public override void ActivateMove(TilePiece piece, Point target)
        {
            InGameController.Grid.ChangePosition(piece, target.X, target.Y);
        }
    }
}
