using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith.Pieces
{
    class OneTile : Bottom
    {
        public OneTile()
        {
            Name = "One Tile";
            Health = 2;
            AttackDamage = 0;
            ManaCost = new Mana(2, 0, 0);
            MoveCost = new Mana(2, 0, 0);
            MoveRange = 1;
            Texture = Load.Get<Texture2D>("1TileBottom");

            Description = "Moves " + MoveRange + " tiles in any direction";
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
