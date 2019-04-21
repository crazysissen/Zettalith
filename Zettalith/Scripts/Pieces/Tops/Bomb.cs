﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class Bomb : Top
    {
        public Bomb()
        {
            Name = "Bomb";
            Health = 4;
            AttackDamage = 0;
            AbilityRange = 2;
            ManaCost = new Mana(0, 4, 0);
            AbilityCost = new Mana(0, 4, 0);
            Modifier = new Addition(new Stats(-7), true);
            Texture = Load.Get<Texture2D>("AltBomb");

            Description = "Explodes and deals " + (Modifier as Addition).StatChanges.Health * -1 + " damage in a circle";
        }

        public override object[] UpdateAbility(TilePiece piece, Point mousePos, bool mouseDown, out bool cancel)
        {
            List<Point> points = Abilities.CircleAoE(piece.Position, new Point(), AbilityRange, 0, false);
            List<SPoint> sPoints = new List<SPoint>(points.ToArray().ToSPointArray());

            ClientSideController.AddHighlight(points.ToArray());

            if (mouseDown)
            {
                for (int i = 0; i < sPoints.Count; ++i)
                {
                    if (mousePos == sPoints[i])
                    {
                        object[] temp = { sPoints, Modifier, piece.GridIndex };
                        cancel = false;
                        return temp;
                    }
                }

                cancel = true;
                return null;
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
            }

            InGameController.Grid[(int)data[2]].Destroy();
        }
    }
}
