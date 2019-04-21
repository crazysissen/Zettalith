using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class WeakBlaster : Top
    {
        public WeakBlaster()
        {
            Name = "Weak Blaster";
            Health = 2;
            AttackDamage = 0;
            AbilityRange = 5;
            ManaCost = new Mana(2, 0, 0);
            AbilityCost = new Mana(2, 0, 0);
            Modifier = new Addition(new Stats(-3), true);
            Texture = Load.Get<Texture2D>("SmallBlasterTop");

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
