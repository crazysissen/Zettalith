using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Zettalith
{
    static class MapGen
    {
        public static Map SquareMap(int width, int height)
        {
            Grid grid = new Grid(width, height);

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    grid[x, y] = new Tile();
                }
            }

            Point[] kingPositions = new Point[2];

            for (int i = 0; i < 2; ++i)
            {

            }

            return new Map() { grid = grid };
        }
    }

    struct Map
    {
        public Grid grid;
        public Point[] kingPositions;
    }
}