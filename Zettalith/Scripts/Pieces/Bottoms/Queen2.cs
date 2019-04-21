using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class Queen2 : Bottom
    {
        public Queen2()
        {
            Name = "Queen 2";
            Health = 8;
            AttackDamage = 0;
            ManaCost = new Mana(0, 0, 6);
            MoveCost = new Mana(0, 0, 5);
            MoveRange = 6;
            MovementTime = 1.5f;
            Texture = Load.Get<Texture2D>("QueenBottom");

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