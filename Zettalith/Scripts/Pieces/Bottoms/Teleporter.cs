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
            Health = 5;
            AttackDamage = 0;
            ManaCost = new Mana(3, 0, 5);
            MoveCost = new Mana(3, 0, 3);
            MoveRange = 3;
            MovementTime = 3;
            Texture = Load.Get<Texture2D>("SmallTPBottom");

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
