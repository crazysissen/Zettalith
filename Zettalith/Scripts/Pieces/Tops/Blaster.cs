﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class Blaster : Top
    {
        public Blaster()
        {
            Name = "Blaster";
            Health = 3;
            AttackDamage = 0;
            AbilityRange = 4;
            ManaCost = new Mana(3, 0, 0);
            AbilityCost = new Mana(3, 0, 0);
            Modifier = new Addition(new Stats(-5), true);
            Texture = Load.Get<Texture2D>("Top");

            Description = "Deals " + (Modifier as Addition).StatChanges.Health * -1 + " damage to target Zettalith within " + AbilityRange + " tiles";
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
                        object[] temp = { sPoints[i], Modifier };
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
        }
    }
}