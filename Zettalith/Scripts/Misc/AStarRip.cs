using System;
using System.Collections.Generic;
using System.Collections;
using Microsoft.Xna.Framework;

namespace Zettalith
{
    public class Astar
    {
        public List<Vector2> result = new List<Vector2>();
        private string find;

        private class ASObject
        {
            public int X
            {
                get;
                set;
            }

            public int Y
            {
                get;
                set;
            }

            public double F
            {
                get;
                set;
            }

            public double G
            {
                get;
                set;
            }

            public int V
            {
                get;
                set;
            }

            public ASObject P
            {
                get;
                set;
            }

            public ASObject(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        private ASObject[] DiagonalSuccessors(bool xN, bool xS, bool xE, bool xW, int N, int S, int E, int W, int[][] grid, int rows, int cols, ASObject[] result, int i)
        {
            if (xN)
            {
                if (xE && grid[N][E] == 0)
                {
                    result[i++] = new ASObject(E, N);
                }
                if (xW && grid[N][W] == 0)
                {
                    result[i++] = new ASObject(W, N);
                }
            }
            if (xS)
            {
                if (xE && grid[S][E] == 0)
                {
                    result[i++] = new ASObject(E, S);
                }
                if (xW && grid[S][W] == 0)
                {
                    result[i++] = new ASObject(W, S);
                }
            }
            return result;
        }

        private ASObject[] DiagonalSuccessorsFree(bool xN, bool xS, bool xE, bool xW, int N, int S, int E, int W, int[][] grid, int rows, int cols, ASObject[] result, int i)
        {
            xN = N > -1;
            xS = S < rows;
            xE = E < cols;
            xW = W > -1;

            if (xE)
            {
                if (xN && grid[N][E] == 0)
                {
                    result[i++] = new ASObject(E, N);
                }
                if (xS && grid[S][E] == 0)
                {
                    result[i++] = new ASObject(E, S);
                }
            }
            if (xW)
            {
                if (xN && grid[N][W] == 0)
                {
                    result[i++] = new ASObject(W, N);
                }
                if (xS && grid[S][W] == 0)
                {
                    result[i++] = new ASObject(W, S);
                }
            }
            return result;
        }

        private ASObject[] Successors(int x, int y, int[][] grid, int rows, int cols)
        {
            int N = y - 1;
            int S = y + 1;
            int E = x + 1;
            int W = x - 1;

            bool xN = N > -1 && grid[N][x] == 0;
            bool xS = S < rows && grid[S][x] == 0;
            bool xE = E < cols && grid[y][E] == 0;
            bool xW = W > -1 && grid[y][W] == 0;

            int i = 0;

            ASObject[] result = new ASObject[8];

            if (xN)
            {
                result[i++] = new ASObject(x, N);
            }
            if (xE)
            {
                result[i++] = new ASObject(E, y);
            }
            if (xS)
            {
                result[i++] = new ASObject(x, S);
            }
            if (xW)
            {
                result[i++] = new ASObject(W, y);
            }

            ASObject[] obj =
                (this.find == "Diagonal" || this.find == "Euclidean") ? DiagonalSuccessors(xN, xS, xE, xW, N, S, E, W, grid, rows, cols, result, i) :
                (this.find == "DiagonalFree" || this.find == "EuclideanFree") ? DiagonalSuccessorsFree(xN, xS, xE, xW, N, S, E, W, grid, rows, cols, result, i) :
                                                                                         result;

            return obj;
        }

        private double Diagonal(ASObject start, ASObject end)
        {
            return Math.Max(Math.Abs(start.X - end.X), Math.Abs(start.Y - end.Y));
        }

        private double Euclidean(ASObject start, ASObject end)
        {
            var x = start.X - end.X;
            var y = start.Y - end.Y;

            return Math.Sqrt(x * x + y * y);
        }

        private double Manhattan(ASObject start, ASObject end)
        {
            return Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y);
        }

        public Astar(int[][] grid, int[] s, int[] e, string f)
        {
            this.find = (f == null) ? "Diagonal" : f;

            int cols = grid[0].Length;
            int rows = grid.Length;
            int limit = cols * rows;
            int length = 1;

            List<ASObject> open = new List<ASObject>();
            open.Add(new ASObject(s[0], s[1]));
            open[0].F = 0;
            open[0].G = 0;
            open[0].V = s[0] + s[1] * cols;

            ASObject current;

            List<int> list = new List<int>();

            double distanceS;
            double distanceE;

            int i;
            int j;

            double max;
            int min;

            ASObject[] next;
            ASObject adj;

            ASObject end = new ASObject(e[0], e[1]);
            end.V = e[0] + e[1] * cols;

            bool inList;

            do
            {
                max = limit;
                min = 0;

                for (i = 0; i < length; i++)
                {
                    if (open[i].F < max)
                    {
                        max = open[i].F;
                        min = i;
                    }
                }

                current = open[min];
                open.RemoveAt(min);

                if (current.V != end.V)
                {
                    --length;
                    next = Successors(current.X, current.Y, grid, rows, cols);

                    for (i = 0, j = next.Length; i < j; ++i)
                    {
                        if (next[i] == null)
                        {
                            continue;
                        }

                        (adj = next[i]).P = current;
                        adj.F = adj.G = 0;
                        adj.V = adj.X + adj.Y * cols;
                        inList = false;

                        foreach (int key in list)
                        {
                            if (adj.V == key)
                            {
                                inList = true;
                            }
                        }

                        if (!inList)
                        {
                            if (this.find == "DiagonalFree" || this.find == "Diagonal")
                            {
                                distanceS = Diagonal(adj, current);
                                distanceE = Diagonal(adj, end);
                            }
                            else if (this.find == "Euclidean" || this.find == "EuclideanFree")
                            {
                                distanceS = Euclidean(adj, current);
                                distanceE = Euclidean(adj, end);
                            }
                            else
                            {
                                distanceS = Manhattan(adj, current);
                                distanceE = Manhattan(adj, end);
                            }

                            adj.F = (adj.G = current.G + distanceS) + distanceE;
                            open.Add(adj);
                            list.Add(adj.V);
                            length++;
                        }
                    }
                }
                else
                {
                    i = length = 0;
                    do
                    {
                        this.result.Add(new Vector2(current.X, current.Y));
                    }
                    while ((current = current.P) != null);
                    result.Reverse();
                }
            }
            while (length != 0);
        }
    }
}
