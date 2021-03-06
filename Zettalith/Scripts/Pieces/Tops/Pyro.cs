﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class Pyro : Top
    {
        public override string Description { get => "Deals " + Modifier.StatChanges.Health * -1 + " damage to all Zettaliths"; protected set => throw new Exception("Cannot set overwritten Description property."); }

        public Pyro()
        {
            Name = "Pyromaniac";
            Health = 2;
            AttackDamage = 0;
            AbilityRange = 0;
            ManaCost = new Mana(3, 0, 0);
            AbilityCost = new Mana(2, 0, 0);
            Modifier = new Addition(new Stats(-1), true);
            Texture = Load.Get<Texture2D>("PyroTop");
        }

        public override object[] UpdateAbility(TilePiece piece, Point mousePos, bool mouseDown, out bool cancel)
        {
            List<Point> points = Abilities.TargetAll();
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

                ClientSideController.Particles.Beam(InGameController.Grid[(int)data[2]].SupposedPosition - new Vector2(0, 1), piece.SupposedPosition - new Vector2(0, 1), new Color(255, 68, 68), new Color(255, 171, 0)); 
            }
        }
    }
}
