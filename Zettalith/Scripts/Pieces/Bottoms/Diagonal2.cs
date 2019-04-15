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
            Health = 8;
            AttackDamage = 2;
            ManaCost = new Mana(0, 0, 2);
            MoveRange = 3;
            Texture = Load.Get<Texture2D>("DiagonalBottom");

            Description = "Moves only " + MoveRange + " tiles diagonally, but is very strong.";
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
