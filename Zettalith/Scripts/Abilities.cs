using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Zettalith
{
    public static class Abilities
    {
        public static List<Point> Beam(Point origin, Point mousePos)
        {
            List<Point> temp = new List<Point>();

            if (mousePos == origin)
                return temp;
            if (mousePos.X == origin.X)
            {
                if (mousePos.Y > origin.Y)
                {
                    for (int i = origin.Y + 1; i <= mousePos.Y; ++i)
                    {
                        temp.Add(new Point(origin.X, i));
                    }
                }
                else if (mousePos.Y < origin.Y)
                {
                    for (int i = origin.Y - 1; i >= mousePos.Y; --i)
                    {
                        temp.Add(new Point(origin.X, i));
                    }
                }
            }
            else if (mousePos.Y == origin.Y)
            {
                if (mousePos.X > origin.X)
                {
                    for (int i = origin.X + 1; i <= mousePos.X; ++i)
                    {
                        temp.Add(new Point(i, origin.Y));
                    }
                }
                else if (mousePos.X < origin.X)
                {
                    for (int i = origin.X - 1; i >= mousePos.X; --i)
                    {
                        temp.Add(new Point(i, origin.Y));
                    }
                }
            }

            return temp;
        }

        public static List<Point> Beam(Point origin, Point mousePos, int range)
        {
            List<Point> temp = new List<Point>();

            if (mousePos == origin)
                return temp;
            if (mousePos.X == origin.X)
            {
                if (mousePos.Y > origin.Y)
                {
                    for (int i = origin.Y + 1; i <= mousePos.Y && i <= range; ++i)
                    {
                        temp.Add(new Point(origin.X, i));
                    }
                }
                else if (mousePos.Y < origin.Y)
                {
                    for (int i = origin.Y - 1; i >= mousePos.Y && i >= range; --i)
                    {
                        temp.Add(new Point(origin.X, i));
                    }
                }
            }
            else if (mousePos.Y == origin.Y)
            {
                if (mousePos.X > origin.X)
                {
                    for (int i = origin.X + 1; i <= mousePos.X && i <= range; ++i)
                    {
                        temp.Add(new Point(i, origin.Y));
                    }
                }
                else if (mousePos.X < origin.X)
                {
                    for (int i = origin.X - 1; i >= mousePos.X && i >= range; --i)
                    {
                        temp.Add(new Point(i, origin.Y));
                    }
                }
            }

            return temp;
        }

        public static List<Point> Target;
    }
}
