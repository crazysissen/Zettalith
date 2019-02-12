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
            List<Point> temp = new List<Point>();
            bool[] check = new bool[4]
            {
                true, true, true, true
            };

            for (int i = 1; i <= moveRange; ++i)
            {
                if (check[0] && InGameController.Grid.Vacant(origin.X, origin.Y + i))
                    temp.Add(new Point(origin.X, origin.Y + i));
                else
                    check[0] = false;

                if (check[1] && InGameController.Grid.Vacant(origin.X + i, origin.Y))
                    temp.Add(new Point(origin.X + i, origin.Y));
                else
                    check[1] = false;

                if (check[2] && InGameController.Grid.Vacant(origin.X, origin.Y - i))
                    temp.Add(new Point(origin.X, origin.Y - i));
                else
                    check[2] = false;

                if (check[3] && InGameController.Grid.Vacant(origin.X - i, origin.Y))
                    temp.Add(new Point(origin.X - i, origin.Y));
                else
                    check[3] = false;
            }

            //for (int i = 1; i <= MoveRange; ++i)
            //{
            //    if (InGameController.Grid.Vacant(origin.X + i, origin.Y))
            //        temp.Add(new Point(origin.X + i, origin.Y));
            //    else
            //        break;
            //}

            //for (int i = 1; i <= MoveRange; ++i)
            //{
            //    if (InGameController.Grid.Vacant(origin.X - i, origin.Y))
            //        temp.Add(new Point(origin.X - i, origin.Y));
            //    else
            //        break;
            //}

            //for (int i = 1; i <= MoveRange; ++i)
            //{
            //    if (InGameController.Grid.Vacant(origin.X, origin.Y + i))
            //        temp.Add(new Point(origin.X, origin.Y + i));
            //    else
            //        break;
            //}

            //for (int i = 1; i <= MoveRange; ++i)
            //{
            //    if (InGameController.Grid.Vacant(origin.X, origin.Y - i))
            //        temp.Add(new Point(origin.X, origin.Y - i));
            //    else
            //        break;
            //}

            return temp;
        }

        /// <summary>
        /// For the int array: Down, Right, Up, Left
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="moveRanges"></param>
        /// <returns></returns>
        public static List<Point> Straight(Point origin, int[] moveRanges)
        {
            List<Point> temp = new List<Point>();

            for (int i = 1; i <= moveRanges[0]; ++i)
            {
                if (InGameController.Grid.Vacant(origin.X, origin.Y + i))
                    temp.Add(new Point(origin.X, origin.Y + i));
                else
                    break;
            }

            for (int i = 1; i <= moveRanges[1]; ++i)
            {
                if (InGameController.Grid.Vacant(origin.X + i, origin.Y))
                    temp.Add(new Point(origin.X + i, origin.Y));
                else
                    break;
            }

            for (int i = 1; i <= moveRanges[2]; ++i)
            {
                if (InGameController.Grid.Vacant(origin.X, origin.Y - i))
                    temp.Add(new Point(origin.X, origin.Y - i));
                else
                    break;
            }

            for (int i = 1; i <= moveRanges[3]; ++i)
            {
                if (InGameController.Grid.Vacant(origin.X - i, origin.Y))
                    temp.Add(new Point(origin.X - i, origin.Y));
                else
                    break;
            }

            return temp;
        }

        public static List<Point> Diagonal(Point origin, int moveRange)
        {
            List<Point> temp = new List<Point>();
            bool[] check = new bool[4]
            {
                true, true, true, true
            };

            for (int i = 1; i < moveRange; ++i)
            {
                if (check[0] && InGameController.Grid.Vacant(origin.X + i, origin.Y + i))
                    temp.Add(new Point(origin.X + i, origin.Y + i));
                else
                    check[0] = false;

                if (check[1] && InGameController.Grid.Vacant(origin.X + i, origin.Y - i))
                    temp.Add(new Point(origin.X + i, origin.Y - i));
                else
                    check[1] = false;

                if (check[2] && InGameController.Grid.Vacant(origin.X - i, origin.Y - i))
                    temp.Add(new Point(origin.X - i, origin.Y - i));
                else
                    check[2] = false;

                if (check[3] && InGameController.Grid.Vacant(origin.X - i, origin.Y + i))
                    temp.Add(new Point(origin.X - i, origin.Y + i));
                else
                    check[3] = false;
            }

            return temp;
        }

        /// <summary>
        /// For the int array: Bottom-right, Top-right, Top-left, Bottom-left
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="moveRanges"></param>
        /// <returns></returns>
        public static List<Point> Diagonal(Point origin, int[] moveRanges)
        {
            List<Point> temp = new List<Point>();

            for (int i = 1; i < moveRanges[0]; ++i)
            {
                if (InGameController.Grid.Vacant(origin.X + i, origin.Y + i))
                    temp.Add(new Point(origin.X + i, origin.Y + i));
                else
                    break;
            }

            for (int i = 1; i < moveRanges[1]; ++i)
            {
                if (InGameController.Grid.Vacant(origin.X + i, origin.Y - i))
                    temp.Add(new Point(origin.X + i, origin.Y - i));
                else
                    break;
            }

            for (int i = 1; i < moveRanges[2]; ++i)
            {
                if (InGameController.Grid.Vacant(origin.X - i, origin.Y - i))
                    temp.Add(new Point(origin.X - i, origin.Y - i));
                else
                    break;
            }

            for (int i = 1; i < moveRanges[3]; ++i)
            {
                if (InGameController.Grid.Vacant(origin.X - i, origin.Y + i))
                    temp.Add(new Point(origin.X - i, origin.Y + i));
                else
                    break;
            }

            return temp;
        }

        public static List<Point> Teleport(Point origin)
        {
            List<Point> temp = new List<Point>();

            for (int i = 0; i < InGameController.Grid.xLength; ++i)
            {
                for (int j = 0; j < InGameController.Grid.yLength; ++j)
                {
                    if (InGameController.Grid.Vacant(i, j))
                    {
                        temp.Add(new Point(i, j));
                    }
                }
            }

            temp.Remove(origin);
            return temp;
        }

        public static List<Point> Teleport(Point origin, Point moveRange)
        {
            List<Point> temp = new List<Point>();

            for (int i = -moveRange.X; i <= moveRange.X; ++i)
            {
                for (int j = -moveRange.Y; j <= moveRange.Y; ++j)
                {
                    if (InGameController.Grid.Vacant(origin.X + i, origin.Y + j))
                    {
                        temp.Add(new Point(origin.X + i, origin.Y + j));
                    }
                }
            }

            return temp;
        }

        //public static List<Point> Swap(Point origin)
        //{

        //}
    }
}
