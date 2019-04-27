using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class CyclopsAbomination : Top
    {
        public override string Description { get => "Deals " + (Modifier as Addition).StatChanges.Health * -1 + " damage to all Zettaliths in a 5 units wide beam."; protected set => throw new Exception("Cannot set overwritten Description property."); }

        public CyclopsAbomination()
        {
            Name = "Cycloptic Abomination";
            Health = 10;
            AttackDamage = 10;
            AbilityRange = 0;
            ManaCost = new Mana(4, 4, 4);
            AbilityCost = new Mana(1, 1, 1);
            Modifier = new Addition(new Stats(-5), true);
            Texture = Load.Get<Texture2D>("jos2");
        }

        public override object[] UpdateAbility(TilePiece piece, Point mousePos, bool mouseDown, out bool cancel)
        {
            List<Point> points = Abilities.Beam(piece.Position, mousePos, 5);
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
