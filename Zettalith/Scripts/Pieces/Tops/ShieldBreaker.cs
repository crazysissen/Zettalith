﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class ShieldBreaker : Top
    {
        public ShieldBreaker()
        {
            Name = "Shield Breaker";
            Health = 3;
            AttackDamage = 2;
            AbilityRange = 3;
            ManaCost = new Mana(0, 3, 3);
            AbilityCost = new Mana(0, 2, 3);
            Modifier = new Direct(new Stats(0, true), true);
            Texture = Load.Get<Texture2D>("ShieldBreaker");

            Description = "Removes all armor from a Zettalith";
        }

        public override object[] UpdateAbility(TilePiece piece, Point mousePos, bool mouseDown, out bool cancel)
        {
            List<Point> points = Abilities.TargetAll(piece.Position, AbilityRange);
            List<SPoint> sPoints = new List<SPoint>(points.ToArray().ToSPointArray());

            ClientSideController.AddHighlight(points.ToArray());

            if (mouseDown)
            {
                for (int i = 0; i < sPoints.Count; ++i)
                {
                    if (mousePos == sPoints[i])
                    {
                        object[] temp = { sPoints[i], Modifier, piece.GridIndex };
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
            TileObject piece = InGameController.Grid.GetObject(((SPoint)data[0]).X, ((SPoint)data[0]).Y);
            (piece as TilePiece).Piece.ModThis(data[1] as Modifier);

            ClientSideController.Particles.Beam(piece.SupposedPosition - new Vector2(0, 1), InGameController.Grid[(int)data[2]].SupposedPosition - new Vector2(0, 1), Color.White, new Color(Color.CadetBlue, 0.0f), 200);
        }
    }
}
