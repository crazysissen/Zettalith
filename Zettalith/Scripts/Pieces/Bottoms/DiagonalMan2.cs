using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class DiagonalMan2 : Bottom
    {
        public DiagonalMan2()
        {
            Name = "Diagonal Man 2";
            Health = 10;
            AttackDamage = 7;
            ManaCost = new Mana(2, 0, 0);
            MoveRange = 2;
            Texture = Load.Get<Texture2D>("TestSubpiece");

            Description = "Moves " + MoveRange + " tiles diagonally";
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
