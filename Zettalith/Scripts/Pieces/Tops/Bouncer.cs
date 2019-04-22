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
            AbilityRange = 1;
            ManaCost = new Mana(2, 4, 0);
            AbilityCost = new Mana(0, 5, 0);
            Modifier = new Addition(new Stats(-3), true);
            Texture = Load.Get<Texture2D>("Bouncer");

            Description = "Pushes away another Zettalith, dealing " + Modifier.StatChanges.Health * -1 + " damage";
        }

        public override object[] UpdateAbility(TilePiece piece, Point mousePos, bool mouseDown, out bool cancel)
        {
            List<Point> points = Abilities.TargetAll(piece.Position, AbilityRange);

            if (points.Contains(piece.Position))
            {
                points.Remove(piece.Position);
            }

            List<SPoint> sPoints = new List<SPoint>(points.ToArray().ToSPointArray());

            for (int i = 0; i < points.Count; ++i)
            {
                if (points[i].X != piece.Position.X && points[i].Y != piece.Position.Y)
                {
                    points.RemoveAt(i);
                    i--;
                }
            }

            Point target = new Point(-1, -1);

            if (InGameController.Grid.GetObject(mousePos.X, mousePos.Y) != null && points.Contains(mousePos))
            {
                points.Remove(mousePos);
                target = mousePos;
            }

            Point dir = target - piece.Position;
            Point last = target;

            for (int i = 1; i < InGameController.Grid.xLength; ++i)
            {
                if (dir.X > 0)
                {
                    if (InGameController.Grid.Vacant(target.X + i, target.Y))
                    {
                        last = new Point(target.X + i, target.Y);
                    }
                    else
                    {
                        break;
                    }
                }
                else if (dir.X < 0)
                {
                    if (InGameController.Grid.Vacant(target.X - i, target.Y))
                    {
                        last = new Point(target.X - i, target.Y);
                    }
                    else
                    {
                        break;
                    }
                }
                else if (dir.Y > 0)
                {
                    if (InGameController.Grid.Vacant(target.X, target.Y + i))
                    {
                        last = new Point(target.X, target.Y + i);
                    }
                    else
                    {
                        break;
                    }
                }
                else if (dir.Y < 0)
                {
                    if (InGameController.Grid.Vacant(target.X, target.Y - i))
                    {
                        last = new Point(target.X, target.Y - i);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            ClientSideController.AddHighlight(points.ToArray());

            if (target != new Point(-1, -1))
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
                        object[] temp = { (SPoint)target, (SPoint)last, Modifier };
                        cancel = false;
                        return temp;
                    }
                }

                cancel = true;
                return null;
            }

            ClientSideController.AddHighlight(ClientSideController.defaultEnemyHighlightColor, target);
            ClientSideController.AddHighlight(ClientSideController.meleeHighlightColor, last);

            
            cancel = false;
            return null;
        }

        public override void ActivateAbility(object[] data)
        {
            TilePiece target = (TilePiece)InGameController.Grid.GetObject(((SPoint)data[0]).X, ((SPoint)data[0]).Y);
            InGameController.Grid.ChangePosition(target, ((SPoint)data[1]).X, ((SPoint)data[1]).Y);
            target.Piece.ModThis((Modifier)data[2]);
        }
    }
}
