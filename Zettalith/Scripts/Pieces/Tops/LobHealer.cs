using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith.Pieces
{
    class LobHealer : Top
    {
        public LobHealer()
        {
            Name = "Splash Healer";
            Health = 2;
            AttackDamage = 0;
            AbilityRange = 2;
            ManaCost = new Mana(0, 2, 2);
            Modifier = new Addition(new Stats(3), true);
            Texture = Load.Get<Microsoft.Xna.Framework.Graphics.Texture2D>("HealerTop");

            Description = "Throws a health potion and heals all targets within " + AbilityRange + " tiles by " + (Modifier as Addition).StatChanges.Health;
        }

        public override object[] UpdateAbility(TilePiece piece, Point mousePos, bool mouseDown, out bool cancel)
        {
            List<Point> points = Abilities.CircleAoE(mousePos, piece.Position, AbilityRange, 0, true);
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
