﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class TestTop1 : Top
    {
        public TestTop1()
        {
            Name = "Bomb";
            Health = 5;
            AttackDamage = 1;
            ManaCost = new Mana(3, 0, 0);
            AbilityCost = new Mana(2, 0, 0);
            Description = "Explodes and deals 7 damage to all Zettaliths within 2 tiles";
            Modifier = new Addition(new Stats(-7), true);
            Texture = Load.Get<Texture2D>("TestSubpiece");
            AbilityRange = 2;
        }

        public override object[] UpdateAbility(TilePiece piece, Point mousePos, bool mouseDown, out bool cancel)
        {
            List<Point> points = Abilities.CircleAoE(piece.Position, AbilityRange);
            List<SPoint> sPoints = new List<SPoint>(points.ToArray().ToSPointArray());

            ClientSideController.AddHighlight(points.ToArray());

            if (mouseDown)
            {
                object[] temp = { sPoints, Modifier, piece.GridIndex };
                cancel = false;
                return temp;
            }

            cancel = false;
            return null;
        }

        public override void ActivateAbility(object[] data)
        {
            List<SPoint> temp = data[0] as List<SPoint>;

            for (int i = 0; i < temp.Count; ++i)
            {
                TileObject piece = InGameController.Grid.GetObject(temp[i].X, temp[i].Y);

                if (piece == null || !(piece is TilePiece))
                    continue;

                (piece as TilePiece).Piece.ModThis(data[1] as Modifier);

                InGameController.Grid[(int)data[2]].Destroy();
            }
        }
    }
}
