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
            Health = 3;
            ManaCost = new Mana(0, 0, 6);
            MoveCost = new Mana(0, 0, 4);
            Texture = Load.Get<Texture2D>("TestSubpiece");

            Description = "Teleports to anywhere on the map.";
        }

        public override List<Point> RequestMove(Point origin)
        {
            return Movement.Teleport(origin);
        }

        public override void ActivateMove(TilePiece piece, Point target)
        {
            InGameController.Grid.ChangePosition(piece, target.X, target.Y);
        }
    }
}
