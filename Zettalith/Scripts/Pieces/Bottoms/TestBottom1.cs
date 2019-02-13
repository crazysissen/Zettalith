﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Zettalith.Pieces
{
    class TestBottom1 : Bottom
    {
        public TestBottom1()
        {
            Name = "TestBottom1";
            Health = 10;
            AttackDamage = 7;
            ManaCost = new Mana(2, 0, 0);
            MoveRange = 2;
            Description = "Frail but powerful legs.";
            Texture = Load.Get<Texture2D>("TestSubpiece");
        }

        public override List<Point> RequestMove(Point origin)
        {
            return Movement.Diagonal(origin, MoveRange);
        }

        public override void ActivateMove(TilePiece piece, Point target)
        {
            InGameController.Grid.ChangePosition(piece, target.X, target.Y);
        }
    }
}
