using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class Bouncer : Top
    {
        public Bouncer()
        {
            Name = "Bouncer";
            Health = 6;
            AttackDamage = 1;
            AbilityRange = 2;
            ManaCost = new Mana(2, 4, 0);
            AbilityCost = new Mana(0, 5, 0);
            Modifier = new Addition(new Stats(-3), true);
            Texture = Load.Get<Texture2D>("Top");

            Description = "Push another Zettalith, dealing " + Modifier.StatChanges.Health * -1 + " damage";
        }

        public override object[] UpdateAbility(TilePiece piece, Point mousePos, bool mouseDown, out bool cancel)
        {
            List<Point> points = Abilities.Beam(piece.Position, mousePos, AbilityRange, 1);
            List<SPoint> sPoints = new List<SPoint>(points.ToArray().ToSPointArray());
            SPoint target = new Point();


            for (int i = 0; i < points.Count; ++i)
            {
                if (InGameController.Grid.GetObject(points[i].X, points[i].Y) != null)
                {
                    target = points[i];
                    points.RemoveAt(i);
                }
            }

            ClientSideController.AddHighlight(points.ToArray());
            ClientSideController.AddHighlight(ClientSideController.defaultEnemyHighlightColor, target);

            if (mouseDown)
            {
                for (int i = 0; i < sPoints.Count; ++i)
                {
                    if (mousePos == sPoints[i])
                    {
                        object[] temp = { sPoints[i], piece.GridIndex, target, Modifier };
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
            Point pos = InGameController.Grid.PositionOf((int)data[1]);
            TilePiece caster = (TilePiece)InGameController.Grid.GetObject(pos.X, pos.Y);
            TilePiece target = (TilePiece)InGameController.Grid.GetObject(((SPoint)data[2]).X, ((SPoint)data[2]).Y);

        }
    }
}
