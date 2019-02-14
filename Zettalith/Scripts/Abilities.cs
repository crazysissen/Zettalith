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

        public static List<Point> Target(bool enemy)
        {
            List<Point> temp = new List<Point>();

            for (int i = 0; i < InGameController.Grid.xLength; ++i)
            {
                for (int j = 0; j < InGameController.Grid.yLength; ++j)
                {
                    if (!(InGameController.Grid.GetObject(i, j) is TilePiece tempPiece))
                        continue;
                    else
                    {
                        if (enemy)
                        {
                            if (tempPiece.Player != InGameController.PlayerIndex)
                            {
                                temp.Add(new Point(i, j));
                            }
                        }
                        if (!enemy)
                        {
                            if (tempPiece.Player == InGameController.PlayerIndex)
                            {
                                temp.Add(new Point(i, j));
                            }
                        }
                    }
                }
            }

            return temp;
        }

        public static List<Point> Target(bool enemy, Point origin, int range)
        {
            List<Point> temp = new List<Point>();

            int xBound = (origin.X - range) < 0 ? 0 : origin.X - range;
            int yBound = (origin.Y + range) > InGameController.Grid.yLength ? InGameController.Grid.yLength : origin.Y + range;

            for (int i = xBound; i <= origin.X + range && i <= InGameController.Grid.xLength; ++i)
            {
                for (int j = yBound; j >= origin.Y - range && j >= 0; --j)
                {
                    if (!(InGameController.Grid.GetObject(i, j) is TilePiece tempPiece))
                        continue;

                    if (enemy)
                    {
                        if (tempPiece.Player != InGameController.PlayerIndex)
                        {
                            temp.Add(new Point(i, j));
                        }
                    }
                    if (!enemy)
                    {
                        if (tempPiece.Player == InGameController.PlayerIndex)
                        {
                            temp.Add(new Point(i, j));
                        }
                    }
                }
            }

            return temp;
        }

        public static List<Point> TargetAll()
        {
            List<Point> temp = new List<Point>();

            for (int i = 0; i < InGameController.Grid.xLength; ++i)
            {
                for (int j = 0; j < InGameController.Grid.yLength; ++j)
                {
                    if (InGameController.Grid.GetObject(i, j) is TilePiece)
                    {
                        temp.Add(new Point(i, j));
                    }
                }
            }

            return temp;
        }

        public static List<Point> TargetAll(Point origin, int range)
        {
            List<Point> temp = new List<Point>();

            int xBound = (origin.X - range) < 0 ? 0 : origin.X - range;
            int yBound = (origin.Y + range) > InGameController.Grid.yLength ? InGameController.Grid.yLength : origin.Y + range;

            for (int i = xBound; i <= origin.X + range && i <= InGameController.Grid.xLength; ++i)
            {
                for (int j = yBound; j >= origin.Y - range && j >= 0; --j)
                {
                    if (InGameController.Grid.GetObject(i, j) == null || !(InGameController.Grid.GetObject(i, j) is TilePiece))
                        continue;

                    temp.Add(new Point(i, j));
                }
            }

            return temp;
        }

        public static List<Point> SquareAoE(Point origin, int range)
        {
            List<Point> temp = new List<Point>();

            int xBound = (origin.X - range) < 0 ? 0 : origin.X - range;
            int yBound = (origin.Y + range) > InGameController.Grid.yLength ? InGameController.Grid.yLength : origin.Y + range;

            for (int i = xBound; i <= origin.X + range && i <= InGameController.Grid.xLength; ++i)
            {
                for (int j = yBound; j >= origin.Y - range && j >= 0; --j)
                {
                    temp.Add(new Point(i, j));
                }
            }

            return temp;
        }

        public static List<Point> CircleAoE(Point origin, int range)
        {
            List<Point> temp = new List<Point>();

            int xBound = (origin.X - range) < 0 ? 0 : origin.X - range;
            int yBound = (origin.Y + range) > InGameController.Grid.yLength ? InGameController.Grid.yLength : origin.Y + range;

            for (int i = xBound; i <= origin.X + range && i <= InGameController.Grid.xLength; ++i)
            {
                for (int j = yBound; j >= origin.Y - range && j >= 0; --j)
                {
                    if ((new Point(i, j) - origin).ToVector2().Length() < (range + 0.5f))
                    {
                        temp.Add(new Point(i, j));
                    }
                }
            }

            return temp;
        }
    }
}
