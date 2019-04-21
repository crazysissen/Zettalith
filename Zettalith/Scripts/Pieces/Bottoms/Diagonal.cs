using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class Diagonal : Bottom
    {
        public Diagonal()
        {
            Name = "Diagonal";
            Health = 4;
            AttackDamage = 0;
            ManaCost = new Mana(0, 2, 0);
            MoveCost = new Mana(0, 2, 0);
            MoveRange = 3;
            MovementTime = 1;
            Texture = Load.Get<Texture2D>("SixtenBottom2");

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
