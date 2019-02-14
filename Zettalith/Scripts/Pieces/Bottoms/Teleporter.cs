using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class Teleporter : Bottom
    {
        public Teleporter()
        {
            Name = "Teleporter";
            Health = 3;
            ManaCost = new Mana(0, 0, 4);
            MoveCost = new Mana(0, 0, 3);
            MoveRange = 5;
            Description = "Teleports to an empty tile within 5 tiles.";
            Texture = Load.Get<Texture2D>("TestSubpiece");
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
