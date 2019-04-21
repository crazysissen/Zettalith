using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class Mortar : Top
    {
        public Mortar()
        {
            Name = "Mortar Tower";
            Health = 4;
            AttackDamage = 0;
            AbilityRange = 1;
            ManaCost = new Mana(0, 0, 4);
            AbilityCost = new Mana(2, 0, 3);
            Modifier = new Addition(new Stats(-3), true);
            Texture = Load.Get<Texture2D>("AOELOBtop");

            Description = "Deals " + Modifier.StatChanges.Health * -1 + " damage to Zettaliths in a circle. Cannot fire within 4 tiles";
        }

        public override object[] UpdateAbility(TilePiece piece, Point mousePos, bool mouseDown, out bool cancel)
        {
            List<Point> points = Abilities.CircleAoE(mousePos, piece.Position, AbilityRange, 4, true);
            List<SPoint> sPoints = new List<SPoint>(points.ToArray().ToSPointArray());

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
            }
        }
    }
}
