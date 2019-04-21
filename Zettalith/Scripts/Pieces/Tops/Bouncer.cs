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
            List<Point> points = Abilities.Target(true, piece.Position, AbilityRange);
            
            for (int i = 0; i < points.Count; ++i)
            {
                if (points[i].X != piece.Position.X || points[i].Y != piece.Position.Y)
                {
                    points.RemoveAt(i);
                    --i;
                }
            }

            List<SPoint> sPoints = new List<SPoint>(points.ToArray().ToSPointArray());
            Point target = new Point();

            for (int i = 0; i < points.Count; ++i)
            {
                if (InGameController.Grid.GetObject(points[i].X, points[i].Y) != null)
                {
                    target = points[i];
                    break;
                }
            }

            points.Remove(target);

            if (target == new Point() && mouseDown)
            {
                cancel = true;
                return null;
            }

            Point last = target;
            Point endUp = piece.Position;
            Point dir = target - endUp;

            for (int i = 1; i < InGameController.Grid.xLength; ++i)
            {
                if (dir.X > 0)
                {
                    if (InGameController.Grid.Vacant(target.X + i, target.Y))
                    {
                        last = new Point(target.X + i, target.Y);
                        endUp = last - new Point(1, 0);
                    }
                    else
                        break;
                }
                else if (dir.X < 0)
                {
                    if (InGameController.Grid.Vacant(target.X - i, target.Y))
                    {
                        last = new Point(target.X - i, target.Y);
                        endUp = last + new Point(1, 0);
                    }
                    else
                        break;
                }
                else if (dir.Y > 0)
                {
                    if (InGameController.Grid.Vacant(target.X, target.Y + i))
                    {
                        last = new Point(target.X, target.Y + i);
                        endUp = last - new Point(0, 1);
                    }
                    else
                        break;
                }
                else if (dir.Y < 0)
                {
                    if (InGameController.Grid.Vacant(target.X, target.Y - i))
                    {
                        last = new Point(target.X, target.Y - i);
                        endUp = last + new Point(0, 1);
                    }
                    else
                        break;
                }
            }

            ClientSideController.AddHighlight(points.ToArray());

            if (target != new Point())
            {
                ClientSideController.AddHighlight(ClientSideController.defaultEnemyHighlightColor, target);
                ClientSideController.AddHighlight(ClientSideController.meleeHighlightColor, last);
            }

            if (mouseDown)
            {
                for (int i = 0; i < sPoints.Count; ++i)
                {
                    if (mousePos == sPoints[i])
                    {
                        object[] temp = { sPoints[i], piece.GridIndex, (SPoint)target, (SPoint)last, Modifier };
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
            Point casterPos = InGameController.Grid.PositionOf((int)data[1]);
            TilePiece caster = (TilePiece)InGameController.Grid.GetObject(casterPos.X, casterPos.Y);
            TilePiece target = (TilePiece)InGameController.Grid.GetObject(((SPoint)data[2]).X, ((SPoint)data[2]).Y);

            InGameController.Grid.ChangePosition(target, ((SPoint)data[3]).X, ((SPoint)data[3]).Y);
            InGameController.Grid.ChangePosition(caster, ((SPoint)data[0]).X, ((SPoint)data[0]).Y);
            target.Piece.ModThis((Modifier)data[4]);
        }
    }
}
