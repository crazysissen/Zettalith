using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class StrongBlaster : Top
    {
        public override string Description { get => "Deals " + (Modifier as Addition).StatChanges.Health * -1 + " damage"; protected set => throw new Exception("Cannot set overwritten Description property."); }

        public StrongBlaster()
        {
            Name = "Vaporizer";
            Health = 4;
            AttackDamage = 0;
            AbilityRange = 3;
            ManaCost = new Mana(5, 0, 0);
            AbilityCost = new Mana(4, 0, 0);
            Modifier = new Addition(new Stats(-7), true);
            Texture = Load.Get<Texture2D>("LargeBlasterTop");
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
                        object[] temp = { sPoints[i], Modifier, piece.GridIndex };
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

            ClientSideController.Particles.Beam(piece.SupposedPosition - new Vector2(0, 1), InGameController.Grid[(int)data[2]].SupposedPosition - new Vector2(0, 1), Color.White, new Color(Color.CadetBlue, 0.0f), 400);
        }
    }
}
