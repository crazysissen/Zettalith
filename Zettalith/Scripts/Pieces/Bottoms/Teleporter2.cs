using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class Teleporter2 : Bottom
    {
        public Teleporter2()
        {
            Name = "Teleporter 2";
            Health = 5;
            ManaCost = new Mana(0, 0, 4);
            MoveCost = new Mana(2, 2, 2);
            MoveRange = 8;
            Texture = Load.Get<Texture2D>("TPBottom");

            Description = "Teleports to an empty tile within " + MoveRange + " tiles.";
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
