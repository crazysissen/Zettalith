﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith.Pieces
{
    class CyclopsNightmare : Top
    {
        public CyclopsNightmare()
        {
            Name = "Cycloptic Nightmare";
            Health = 5;
            AttackDamage = 3;
            ManaCost = new Mana(4, 3, 2);
            Texture = Load.Get<Texture2D>("Cleo_Cyclops_head");
            Modifier = new Addition(new Stats(-3), true);
            Description = "Deals " + (Modifier as Addition).StatChanges.Health * -1 + " damage to all Zettaliths in a 3 units wide beam.";
        }

        public override object[] UpdateAbility(TilePiece piece, Point mousePos, bool mouseDown, out bool cancel)
        {
            List<Point> points = Abilities.Beam(piece.Position, mousePos, 3);
            List<SPoint> sPoints = new List<SPoint>(points.ToArray().ToSPointArray());

            if (sPoints.Count == 0)
            {
                if (mouseDown)
                {
                    cancel = true;
                    return null;
                }

                cancel = false;
                return null;
            }

            ClientSideController.AddHighlight(points.ToArray());

            if (mouseDown)
            {
                for (int i = 0; i < sPoints.Count; ++i)
                {
                    if (mousePos == sPoints[i])
                    {
                        object[] temp = { sPoints, Modifier };
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
                //(InGameController.Grid.GetObject((data[0] as List<SPoint>)[i].X, (data[0] as List<SPoint>)[i].Y) as TilePiece).Piece.ModThis(data[1] as Modifier);
            }
        }
    }
}
