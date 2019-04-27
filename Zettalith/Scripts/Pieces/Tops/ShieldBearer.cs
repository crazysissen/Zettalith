using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class ShieldBearer : Top
    {
        public override string Description { get => "Gives a Zettalith " + Modifier.StatChanges.Armor + " armor"; protected set => throw new Exception("Cannot set overwritten Description property."); }

        public ShieldBearer()
        {
            Name = "Shield Bearer";
            Health = 5;
            AttackDamage = 0;
            AbilityRange = 3;
            ManaCost = new Mana(0, 4, 2);
            AbilityCost = new Mana(0, 4, 0);
            Modifier = new Addition(new Stats(3, true), true);
            Texture = Load.Get<Texture2D>("ShieldBearer");
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
