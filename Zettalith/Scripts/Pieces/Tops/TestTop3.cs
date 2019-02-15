using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class TestTop3 : Top
    {
        public TestTop3()
        {
            Name = "Woop";
            Health = 10;
            AttackDamage = 2;
            ManaCost = new Mana(2, 1, 0);
            Texture = Load.Get<Texture2D>("TestSubpiece");
            Modifier = new Addition(new Stats(-3), true);
            Description = "Deals " + (Modifier as Addition).StatChanges.Health * -1 + " damage to all Zettaliths in a straight line.";
        }

        public override object[] UpdateAbility(TilePiece piece, Point mousePos, bool mouseDown, out bool cancel)
        {
            List<Point> points = Abilities.Beam(piece.Position, mousePos);
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

            // TODO: Highlight spoints list
            ClientSideController.AddHighlight(points.ToArray());

            if (mouseDown)
            {
                object[] temp = { sPoints, Modifier };
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
                //(InGameController.Grid.GetObject((data[0] as List<SPoint>)[i].X, (data[0] as List<SPoint>)[i].Y) as TilePiece).Piece.ModThis(data[1] as Modifier);
            }
        }
    }
}