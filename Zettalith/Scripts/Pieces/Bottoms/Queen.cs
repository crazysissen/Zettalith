﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class Queen : Bottom
    {
        public Queen()
        {
            Name = "Queen";
            Health = 5;
            AttackDamage = 2;
            ManaCost = new Mana(0, 3, 0);
            MoveRange = 3;
            Texture = Load.Get<Texture2D>("TestSubpiece");

            Description = "Moves " + MoveRange + " tiles in any given direction.";
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
