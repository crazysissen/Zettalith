using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class OneSideTwoUp : Bottom
    {
        //Point[] customMove = new Point[]
        //{
        //    new Point(-1, 0), new Point(1, 0), new Point(0, -1), new Point(0, -2),
        //};

        int[] customMove = new int[]
        {
            0, 1, 2, 1,
        };

        public OneSideTwoUp()
        {
            Name = "Viking Legs";
            Health = 2;
            AttackDamage = 0;
            ManaCost = new Mana(2, 0, 0);
            MoveCost = new Mana(1, 0, 0);
            MoveRange = 0;
            Texture = Load.Get<Texture2D>("TPBottom");

            Description = "Capable of moving one unit to the sides or two units forward";
        }

        public override List<Point> RequestMove(Point origin)
        {
            return Movement.Straight(origin, customMove);
        }

        public override void ActivateMove(TilePiece piece, Point target)
        {
            InGameController.Grid.ChangePosition(piece, target.X, target.Y);
        }
    }
}
