using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class Cyclops : Top
    {
        public Cyclops()
        {
            Name = "Cycloptic Horror";
            Health = 4;
            AttackDamage = 0;
            AbilityRange = 0;
            ManaCost = new Mana(0, 3, 0);
            AbilityCost = new Mana(0, 3, 0);
            Modifier = new Addition(new Stats(-3), true);
            Texture = Load.Get<Texture2D>("Cleo_Cyclops_head");

            Description = "Deals " + (Modifier as Addition).StatChanges.Health * -1 + " damage to Zettaliths in a straight line";
        }

        public override object[] UpdateAbility(TilePiece piece, Point mousePos, bool mouseDown, out bool cancel)
        {
            List<Point> points = Abilities.Beam(piece.Position, mousePos, 1);
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