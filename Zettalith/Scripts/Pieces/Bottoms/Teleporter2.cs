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
            Name = "Warper";
            Health = 5;
            AttackDamage = 0;
            ManaCost = new Mana(5, 0, 7);
            MoveCost = new Mana(5, 0, 5);
            MoveRange = 6;
            MovementTime = 4;
            Texture = Load.Get<Texture2D>("TPBottom");

            Description = "Teleports to an empty tile";
        }

        public override List<Point> RequestMove(Point origin)
        {
            return Movement.Teleport(origin, new Point(MoveRange, MoveRange));
        }

        public override void ActivateMove(TilePiece piece, Point target)
        {
            Vector2 supposed = new Vector2(target.X, target.Y * ClientSideController.HEIGHTDISTANCE) * (InGameController.IsHost ? 1 : -1);
            ClientSideController.Particles.Beam(piece.SupposedPosition, supposed, Color.LightPink, new Color(Color.Purple, 0.0f));
            InGameController.Grid.ChangePosition(piece, target.X, target.Y);
        }
    }
}
