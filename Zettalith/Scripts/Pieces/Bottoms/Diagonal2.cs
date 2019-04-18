using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class Diagonal2 : Bottom
    {
        public Diagonal2()
        {
            Name = "Diagonal 2";
            Health = 4;
            AttackDamage = 1;
            ManaCost = new Mana(0, 4, 0);
            MoveCost = new Mana(0, 2, 0);
            MoveRange = 5;
            Texture = Load.Get<Texture2D>("DiagonalBottom");

            Description = "Moves " + MoveRange + " tiles diagonally.";
        }

        public override List<Point> RequestMove(Point origin)
        {
            return Movement.Diagonal(origin, MoveRange);
        }

        public override void ActivateMove(TilePiece piece, Point target)
        {
            InGameController.Grid.ChangePosition(piece, target.X, target.Y);
        }
    }
}
