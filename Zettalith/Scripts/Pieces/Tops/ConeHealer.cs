using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class ConeHealer : Top
    {
        public ConeHealer()
        {
            Name = "Cone Healer";
            Health = 3;
            AttackDamage = 0;
            AbilityRange = 4;
            ManaCost = new Mana(0, 3, 2);
            AbilityCost = new Mana(0, 0, 4);
            Modifier = new Addition(new Stats(2), true);
            Texture = Load.Get<Texture2D>("HealerTop2");

            Description = "Heals all Zettaliths in a " + AbilityRange + " units long cone by " + (Modifier as Addition).StatChanges.Health;
        }

        public override object[] UpdateAbility(TilePiece piece, Point mousePos, bool mouseDown, out bool cancel)
        {
            List<Point> points = Abilities.Cone(piece.Position, mousePos, AbilityRange);
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
