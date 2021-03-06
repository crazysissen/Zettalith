﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class Straight2 : Bottom
    {
        public Straight2()
        {
            Name = "Castle";
            Health = 3;
            AttackDamage = 0;
            ManaCost = new Mana(3, 0, 0);
            MoveCost = new Mana(4, 0, 0);
            MoveRange = 6;
            MovementTime = 2;
            Texture = Load.Get<Texture2D>("StraightBottom");

            Description = "Moves in a straight line";
        }

        public override List<Point> RequestMove(Point origin)
        {
            return Movement.Straight(origin, MoveRange);
        }

        public override void ActivateMove(TilePiece piece, Point target)
        {
            InGameController.Grid.ChangePosition(piece, target.X, target.Y);
        }
    }
}
