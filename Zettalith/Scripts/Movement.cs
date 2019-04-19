using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Zettalith
{
    public static class Movement
    {
        public static List<Point> Straight(Point origin, int moveRange)
        {
            List<Point> points = new List<Point>();
            bool[] check = new bool[4]
            {
                true, true, true, true
            };

            for (int i = 1; i <= moveRange; ++i)
            {
                if (check[0] && InGameController.Grid.Vacant(origin.X, origin.Y + i))
                    points.Add(new Point(origin.X, origin.Y + i));
                else
                    check[0] = false;

                if (check[1] && InGameController.Grid.Vacant(origin.X + i, origin.Y))
                    points.Add(new Point(origin.X + i, origin.Y));
                else
                    check[1] = false;

                if (check[2] && InGameController.Grid.Vacant(origin.X, origin.Y - i))
                    points.Add(new Point(origin.X, origin.Y - i));
                else
                    check[2] = false;

                if (check[3] && InGameController.Grid.Vacant(origin.X - i, origin.Y))
                    points.Add(new Point(origin.X - i, origin.Y));
                else
                    check[3] = false;
            }

            return points;
        }

        /// <summary>
        /// For the int array: Down, Right, Up, Left
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="moveRanges"></param>
        /// <returns></returns>
        public static List<Point> Straight(Point origin, int[] moveRanges)
        {
            List<Point> points = new List<Point>();

            for (int i = 1; i <= moveRanges[0]; ++i)
            {
                if (InGameController.Grid.Vacant(origin.X, origin.Y + i))
                    points.Add(new Point(origin.X, origin.Y + i));
                else
                    break;
            }

            for (int i = 1; i <= moveRanges[1]; ++i)
            {
                if (InGameController.Grid.Vacant(origin.X + i, origin.Y))
                    points.Add(new Point(origin.X + i, origin.Y));
                else
                    break;
            }

            for (int i = 1; i <= moveRanges[2]; ++i)
            {
                if (InGameController.Grid.Vacant(origin.X, origin.Y - i))
                    points.Add(new Point(origin.X, origin.Y - i));
                else
                    break;
            }

            for (int i = 1; i <= moveRanges[3]; ++i)
            {
                if (InGameController.Grid.Vacant(origin.X - i, origin.Y))
                    points.Add(new Point(origin.X - i, origin.Y));
                else
                    break;
            }

            return points;
        }

        public static List<Point> Diagonal(Point origin, int moveRange)
        {
            List<Point> points = new List<Point>();
            bool[] check = new bool[4]
            {
                true, true, true, true
            };

            for (int i = 1; i <= moveRange; ++i)
            {
                if (check[0] && InGameController.Grid.Vacant(origin.X + i, origin.Y + i))
                    points.Add(new Point(origin.X + i, origin.Y + i));
                else
                    check[0] = false;

                if (check[1] && InGameController.Grid.Vacant(origin.X + i, origin.Y - i))
                    points.Add(new Point(origin.X + i, origin.Y - i));
                else
                    check[1] = false;

                if (check[2] && InGameController.Grid.Vacant(origin.X - i, origin.Y - i))
                    points.Add(new Point(origin.X - i, origin.Y - i));
                else
                    check[2] = false;

                if (check[3] && InGameController.Grid.Vacant(origin.X - i, origin.Y + i))
                    points.Add(new Point(origin.X - i, origin.Y + i));
                else
                    check[3] = false;
            }

            return points;
        }

        /// <summary>
        /// For the int array: Bottom-right, Top-right, Top-left, Bottom-left
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="moveRanges"></param>
        /// <returns></returns>
        public static List<Point> Diagonal(Point origin, int[] moveRanges)
        {
            List<Point> points = new List<Point>();

            for (int i = 1; i <= moveRanges[0]; ++i)
            {
                if (InGameController.Grid.Vacant(origin.X + i, origin.Y + i))
                    points.Add(new Point(origin.X + i, origin.Y + i));
                else
                    break;
            }

            for (int i = 1; i <= moveRanges[1]; ++i)
            {
                if (InGameController.Grid.Vacant(origin.X + i, origin.Y - i))
                    points.Add(new Point(origin.X + i, origin.Y - i));
                else
                    break;
            }

            for (int i = 1; i <= moveRanges[2]; ++i)
            {
                if (InGameController.Grid.Vacant(origin.X - i, origin.Y - i))
                    points.Add(new Point(origin.X - i, origin.Y - i));
                else
                    break;
            }

            for (int i = 1; i <= moveRanges[3]; ++i)
            {
                if (InGameController.Grid.Vacant(origin.X - i, origin.Y + i))
                    points.Add(new Point(origin.X - i, origin.Y + i));
                else
                    break;
            }

            return points;
        }

        public static List<Point> Teleport(Point origin)
        {
            List<Point> points = new List<Point>();

            for (int i = 0; i < InGameController.Grid.xLength; ++i)
            {
                for (int j = 0; j < InGameController.Grid.yLength; ++j)
                {
                    if (InGameController.Grid.Vacant(i, j))
                    {
                        points.Add(new Point(i, j));
                    }
                }
            }

            points.Remove(origin);
            return points;
        }

        public static List<Point> Teleport(Point origin, Point moveRange)
        {
            List<Point> points = new List<Point>();

            for (int i = -moveRange.X; i <= moveRange.X; ++i)
            {
                for (int j = -moveRange.Y; j <= moveRange.Y; ++j)
                {
                    if (InGameController.Grid.Vacant(origin.X + i, origin.Y + j))
                    {
                        points.Add(new Point(origin.X + i, origin.Y + j));
                    }
                }
            }

            return points;
        }

        public static List<Point> Target(Point origin)
        {
            List<Point> points = new List<Point>();

            for (int i = 0; i < InGameController.Grid.xLength; ++i)
            {
                for (int j = 0; j < InGameController.Grid.yLength; ++j)
                {
                    if (!InGameController.Grid.Vacant(i, j))
                    {
                        points.Add(new Point(i, j));
                    }
                }
            }

            points.Remove(origin);
            return points;
        }

        public static List<Point> Custom(Point origin, Point[] custom)
        {
            List<Point> points = new List<Point>();

            if (!InGameController.IsHost)
            {
                for (int i = 0; i < custom.Length; ++i)
                {
                    custom[i] = new Point(custom[i].X, custom[i].Y * -1);
                }
            }

            for (int i = 0; i < custom.Length; ++i)
            {
                points.Add(origin + custom[i]);
            }

            return points;
        }
    }
}
