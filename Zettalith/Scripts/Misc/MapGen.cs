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
        const float
            MAXSPAWNDISTANCE = 1.0f / 4;

        public enum Type
        {
            SquareMap
        }

        static Func<Random, int, int, Map>[] functions = { SquareMap };

        public static Map Generate(Random r, int width, int height, Type type) => functions[(int)type].Invoke(r, width, height);

        public static Map SquareMap(Random r, int width, int height)
        {
            Grid grid = new Grid(width, height);

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    grid[x, y] = new Tile();
                }
            }

            int maxSpawnTiles = (int)Math.Round(height * MAXSPAWNDISTANCE);
            List<Point> possibleSpawnPoints = new List<Point>();

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < maxSpawnTiles; ++y)
                {
                    if (grid[x, y] != null && grid[x + 1, y] != null && grid[x, y + 1] != null && grid[x + 1, x + 1] != null)
                    {
                        possibleSpawnPoints.Add(new Point(x, y));
                    }
                }
            }

            Point spawnPoint = possibleSpawnPoints[r.Next(possibleSpawnPoints.Count)];
            Point[] spawnPoints = { spawnPoint, new Point(spawnPoint.X, height - 1 - spawnPoint.Y) };

            return new Map() { grid = grid, spawnPositions = spawnPoints };
        }
    }

    struct Map
    {
        public Grid grid;
        public Point[] spawnPositions;
    }
}