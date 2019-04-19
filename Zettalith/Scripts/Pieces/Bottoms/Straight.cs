﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class Straight : Bottom
    {
        public Straight()
        {
            Name = "Straight";
            Health = 3;
            AttackDamage = 0;
            ManaCost = new Mana(2, 0, 0);
            MoveCost = new Mana(2, 0, 0);
            MoveRange = 3;
            Texture = Load.Get<Texture2D>("1TileBottom");

            Description = "Moves " + MoveRange + " tiles in a straight line.";
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
