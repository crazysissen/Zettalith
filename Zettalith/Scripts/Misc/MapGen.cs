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
            SquareMap, NoiseMap, NoiseMirrorMap
        }

        static Func<Random, int, int, Map>[] functions = { NoHolesMap, NoiseMap };

        public static Map Generate(Random r, int width, int height, Type type)
        {
            if (width % 2 == 0 && height % 2 == 0)
            {
                return AddBuffer(functions[(int)type].Invoke(r, width, height));
            }

            if (width % 2 != 0)
            {
                if (width > 1)
                {
                    width--;
                }
                else
                {
                    width++;
                }
            }
            if (height % 2 != 0)
            {
                if (height > 1)
                {
                    height--;
                }
                else
                {
                    height++;
                }
            }

            return functions[(int)type].Invoke(r, width, height);
        }

        public static Map AddBuffer(Map map)
        {
            int buffer = 4;

            Grid old = map.grid, newGrid = new Grid(old.xLength + buffer * 2, old.yLength + buffer * 2);

            for (int x = 0; x < old.xLength; x++)
            {
                for (int y = 0; y < old.yLength; y++)
                {
                    newGrid[x + buffer, y + buffer] = old[x, y];
                }
            }

            Point[] newSpawns = { map.spawnPositions[0] + new Point(buffer, buffer), map.spawnPositions[1] + new Point(buffer, buffer) };

            return new Map() { grid = newGrid, spawnPositions = newSpawns };
        }

        public static Map NoiseMap(Random r, int width, int height)
        {
            //try
            //{
                int borderRange = 2;

                float scale = 0.1f, threshhold = -0.45f;

                Noise noise;

                Grid grid = new Grid(width, height);
                Point[] spawns = null;

                bool looping = true;
                while (looping)
                {
                    noise = new Noise(r.Next());

                    grid = new Grid(width, height);

                    int[][] array = new int[width][];

                    for (int x = 0; x < width; x++)
                    {
                        array[x] = new int[height];

                        for (int y = 0; y < height; y++)
                        {
                            float currentThreshhold = threshhold;

                            if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                            {
                                currentThreshhold += 0.6f;
                            }

                            if (noise.Generate(x * scale, y * scale) > currentThreshhold)
                            {
                                grid[x, y] = new Tile();
                                array[x][y] = 0;
                                continue;
                            }

                            array[x][y] = 1;
                        }
                    }

                    // Create spawns 

                    List<Point> spawns1 = new List<Point>(), spawns2 = new List<Point>();

                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < borderRange; y++)
                        {
                            if (grid[x, y] != null)
                            {
                                spawns1.Add(new Point(x, y));
                            }

                            if (grid[x, height - 1 - y] != null)
                            {
                                spawns2.Add(new Point(x, height - 1 - y));
                            }
                        }
                    }

                    if (spawns1.Count == 0 || spawns2.Count == 0)
                    {
                        continue;
                    }

                    int count1 = spawns1.Count, count2 = spawns2.Count;
                    bool breakItDown = false;

                    for (int i = 0; i < count1 - 1 && !breakItDown; i++)
                    {
                        int rIndex = r.Next(spawns1.Count);
                        Point randomSpawn = spawns1[rIndex];
                        spawns1.Remove(randomSpawn);

                        List<Point> targets = new List<Point>(spawns2);

                        for (int j = 0; j < count2 - 1 && !breakItDown; j++)
                        {
                            rIndex = r.Next(targets.Count);

                            Point currentSpawn = targets[rIndex];
                            targets.Remove(currentSpawn);

                            List<Vector2> result = new Astar(array, new int[] { randomSpawn.X, randomSpawn.Y }, new int[] { currentSpawn.X, currentSpawn.Y }, "").result;

                            if (result != null && result.Count > 0)
                            {
                                breakItDown = true;

                                spawns = new Point[2]
                                {
                                randomSpawn,
                                currentSpawn
                                };
                            }
                        }
                    }

                    if (spawns == null)
                    {
                        continue;
                    }

                    looping = false;
                }

                return new Map() { grid = grid, spawnPositions = spawns };
            //}
        }

        public static Map NoHolesMap(Random r, int width, int height)
        {
            Grid grid = new Grid(width, height);

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    grid[x, y] = new Tile();
                }
            }

            return new Map() { grid = grid, spawnPositions = CreateSpawns(grid, ref r) };
        }

        public static Map RectangleMap(Random r, int width, int height)
        {
            int halfHeight = (int)(height * 0.5f), smallestForHoles = 4, chanceForBigStoneOneIn = 3;

            Grid halfGrid = new Grid(width, halfHeight);

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < halfHeight; ++y)
                {
                    halfGrid[x, y] = new Tile();
                }
            }

            // If not tiny add Holes
            if (width >= smallestForHoles && halfHeight >= smallestForHoles)
            {
                int xOffset = 0, yOffset = 0;

                if (width % smallestForHoles != 0) { xOffset = 1; }
                if (halfHeight % smallestForHoles != 0) { yOffset = 1; }

                int widht4s = width / smallestForHoles;
                int height4s = halfHeight / smallestForHoles;

                for (int i = 0; i < height4s; ++i)
                {
                    for (int j = 0; j < widht4s; ++j)
                    {
                        if( r.Next(1, chanceForBigStoneOneIn + 1) == 1)
                        {
                            int tempHoleX = r.Next(1, smallestForHoles), tempHoleY = r.Next(1, smallestForHoles);

                            halfGrid[smallestForHoles * j + tempHoleX + xOffset, smallestForHoles * i + tempHoleY + xOffset] = null;
                            halfGrid[smallestForHoles * j + tempHoleX + xOffset + 1, smallestForHoles * i + tempHoleY + xOffset] = null;
                            halfGrid[smallestForHoles * j + tempHoleX + xOffset, smallestForHoles * i + tempHoleY + xOffset + 1] = null;
                            halfGrid[smallestForHoles * j + tempHoleX + xOffset + 1, smallestForHoles * i + tempHoleY + xOffset + 1] = null;
                        }
                        else
                        {
                            halfGrid[smallestForHoles * j + r.Next(1, smallestForHoles + 1) + xOffset, smallestForHoles * i + r.Next(1, smallestForHoles + 1) + yOffset] = null;
                        }
                    }
                }
            }

            Grid grid = DoubleFlipGrid(halfGrid);

            return new Map() { grid = grid, spawnPositions = CreateSpawns(grid, ref r) };
        }

        private static Grid DoubleFlipGrid(Grid halfGrid)
        {
            Grid grid = new Grid(halfGrid.xLength, halfGrid.yLength * 2);

            for (int i = 0; i < halfGrid.yLength; i++)
            {
                for (int j = 0; j < grid.xLength; j++)
                {
                    grid[j, (int)(grid.yLength * 0.5f + i)] = halfGrid[j, halfGrid.yLength - 1 - i];
                    grid[j, i] = halfGrid[j, i];
                }
            }

            return grid;
        }

        private static Point[] CreateSpawns(Grid grid, ref Random r)
        {
            int maxSpawnTiles = (int)Math.Round(grid.yLength * MAXSPAWNDISTANCE);
            List<Point> possibleSpawnPoints = new List<Point>();

            for (int x = 0; x < grid.xLength; ++x)
            {
                for (int y = 0; y < maxSpawnTiles; ++y)
                {
                    if (grid[x, y] != null && grid[x + 1, y] != null && grid[x, y + 1] != null && grid[x + 1, x + 1] != null)
                    {
                        possibleSpawnPoints.Add(new Point(x, y));
                    }
                }
            }

            try
            {
                Point spawnPoint = possibleSpawnPoints[r.Next(possibleSpawnPoints.Count)];
                return new Point[] { spawnPoint, new Point(spawnPoint.X, grid.yLength - 1 - spawnPoint.Y) };
            }
            catch
            {
                return null;
            }
        }
    }

    struct Map
    {
        public Grid grid;
        public Point[] spawnPositions;
    }
}