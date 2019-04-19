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
        public static List<Point> Beam(Point origin, Point mousePos, int width)
        {
            List<Point> points = new List<Point>();

            if (mousePos == origin)
                return points;
            if (mousePos.X == origin.X)
            {
                if (mousePos.Y > origin.Y)
                {
                    for (int i = origin.Y + 1; i <= mousePos.Y; ++i)
                    {
                        for (int j = -(width / 2); j <= width / 2; ++j)
                        {
                            points.Add(new Point(origin.X + j, i));
                        }
                    }
                }
                else if (mousePos.Y < origin.Y)
                {
                    for (int i = origin.Y - 1; i >= mousePos.Y; --i)
                    {
                        for (int j = -(width / 2); j <= width / 2; ++j)
                        {
                            points.Add(new Point(origin.X + j, i));
                        }
                    }
                }
            }
            else if (mousePos.Y == origin.Y)
            {
                if (mousePos.X > origin.X)
                {
                    for (int i = origin.X + 1; i <= mousePos.X; ++i)
                    {
                        for (int j = -(width / 2); j <= width / 2; ++j)
                        {
                            points.Add(new Point(i, origin.Y + j));
                        }
                    }
                }
                else if (mousePos.X < origin.X)
                {
                    for (int i = origin.X - 1; i >= mousePos.X; --i)
                    {
                        for(int j = -(width / 2); j <= width / 2; ++j)
                        {
                        points.Add(new Point(i, origin.Y + j));
                    }
                }
                }
            }

            return points;
        }

        public static List<Point> Beam(Point origin, Point mousePos, int range, int width)
        {
            List<Point> points = new List<Point>();

            if (mousePos == origin)
                return points;
            if (mousePos.X == origin.X)
            {
                if (mousePos.Y > origin.Y)
                {
                    for (int i = origin.Y + 1; i <= mousePos.Y && i <= range; ++i)
                    {
                        for (int j = -(width / 2); j <= width / 2; ++j)
                        {
                            points.Add(new Point(origin.X + j, i));
                        }
                    }
                }
                else if (mousePos.Y < origin.Y)
                {
                    for (int i = origin.Y - 1; i >= mousePos.Y && i >= range; --i)
                    {
                        for (int j = -(width / 2); j <= width / 2; ++j)
                        {
                            points.Add(new Point(origin.X + j, i));
                        }
                    }
                }
            }
            else if (mousePos.Y == origin.Y)
            {
                if (mousePos.X > origin.X)
                {
                    for (int i = origin.X + 1; i <= mousePos.X && i <= range; ++i)
                    {
                        for (int j = -(width / 2); j <= width / 2; ++j)
                        {
                            points.Add(new Point(i, origin.Y + j));
                        }
                    }
                }
                else if (mousePos.X < origin.X)
                {
                    for (int i = origin.X - 1; i >= mousePos.X && i >= range; --i)
                    {
                        for (int j = -(width / 2); j <= width / 2; ++j)
                        {
                            points.Add(new Point(i, origin.Y + j));
                        }
                    }
                }
            }

            return points;
        }

        public static List<Point> Target(bool enemy)
        {
            List<Point> points = new List<Point>();

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
                                points.Add(new Point(i, j));
                            }
                        }
                        if (!enemy)
                        {
                            if (tempPiece.Player == InGameController.PlayerIndex)
                            {
                                points.Add(new Point(i, j));
                            }
                        }
                    }
                }
            }

            return points;
        }

        public static List<Point> Target(bool enemy, Point origin, int range)
        {
            List<Point> points = new List<Point>();

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
                            points.Add(new Point(i, j));
                        }
                    }
                    if (!enemy)
                    {
                        if (tempPiece.Player == InGameController.PlayerIndex)
                        {
                            points.Add(new Point(i, j));
                        }
                    }
                }
            }

            return points;
        }

        public static List<Point> TargetAll()
        {
            List<Point> points = new List<Point>();

            for (int i = 0; i < InGameController.Grid.xLength; ++i)
            {
                for (int j = 0; j < InGameController.Grid.yLength; ++j)
                {
                    if (InGameController.Grid.GetObject(i, j) is TilePiece)
                    {
                        points.Add(new Point(i, j));
                    }
                }
            }

            return points;
        }

        public static List<Point> TargetAll(Point origin, int range)
        {
            List<Point> points = new List<Point>();

            int xBound = (origin.X - range) < 0 ? 0 : origin.X - range;
            int yBound = (origin.Y + range) > InGameController.Grid.yLength ? InGameController.Grid.yLength : origin.Y + range;

            for (int i = xBound; i <= origin.X + range && i <= InGameController.Grid.xLength; ++i)
            {
                for (int j = yBound; j >= origin.Y - range && j >= 0; --j)
                {
                    if (InGameController.Grid.GetObject(i, j) == null || !(InGameController.Grid.GetObject(i, j) is TilePiece))
                        continue;

                    points.Add(new Point(i, j));
                }
            }

            return points;
        }

        public static List<Point> SquareAoE(Point center, int radius, bool excludeObjects)
        {
            List<Point> points = new List<Point>();

            int xBound = (center.X - radius) < 0 ? 0 : center.X - radius;
            int yBound = (center.Y + radius) > InGameController.Grid.yLength ? InGameController.Grid.yLength : center.Y + radius;

            for (int i = xBound; i <= center.X + radius && i <= InGameController.Grid.xLength; ++i)
            {
                for (int j = yBound; j >= center.Y - radius && j >= 0; --j)
                {
                    if (excludeObjects)
                    {
                        if (InGameController.Grid.Vacant(i, j))
                            points.Add(new Point(i, j));
                    }
                    else
                        points.Add(new Point(i, j));
                }
            }

            return points;
        }

        public static List<Point> CircleAoE(Point center, Point origin, int radius, int minRange, bool includeCenter)
        {
            List<Point> points = new List<Point>();

            if ((center - origin).ToVector2().Length() < minRange)
            {
                return points;
            }

            int xBound = (center.X - radius) < 0 ? 0 : center.X - radius;
            int yBound = (center.Y + radius) > InGameController.Grid.yLength ? InGameController.Grid.yLength : center.Y + radius;

            for (int i = xBound; i <= center.X + radius && i <= InGameController.Grid.xLength; ++i)
            {
                for (int j = yBound; j >= center.Y - radius && j >= 0; --j)
                {
                    if ((new Point(i, j) - center).ToVector2().Length() < (radius + 0.5f))
                    {
                        points.Add(new Point(i, j));
                    }
                }
            }
            
            if (!includeCenter)
            {
                points.Remove(center);
            }

            return points;
        }

        public static List<Point> Cone(Point origin, Point mousePos, int range)
        {
            List<Point> upCone = new List<Point>();
            List<Point> rightCone = new List<Point>();
            List<Point> downCone = new List<Point>();
            List<Point> leftCone = new List<Point>();

            for (int i = 1; i <= range; ++i)
            {
                int offset = i - 1;

                for (int j = -offset; j <= offset; ++j)
                {
                    upCone.Add(new Point(origin.X + j, origin.Y - i));
                    rightCone.Add(new Point(origin.X + i, origin.Y + j));
                    downCone.Add(new Point(origin.X + j, origin.Y + i));
                    leftCone.Add(new Point(origin.X - i, origin.Y + j));
                }
            }

            return upCone.Contains(mousePos) ? upCone : rightCone.Contains(mousePos) ? rightCone : downCone.Contains(mousePos) ? downCone : leftCone.Contains(mousePos) ? leftCone : new List<Point>();
        }
    }
}
