﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    sealed class Grid
    {
        const int
            MAXSIZE = 1024;

        public readonly int xLength, yLength;

        public Tile[,] TileArray { get; private set; }
        public TileObject[] Objects => objects;

        private TileObject[] objects;

        public TileObject this[int id]
        {
            get => id >= 0 && id < objects.Length ? objects[id] : null;

            set
            {
                if (id >= 0 && id < objects.Length)
                {
                    objects[id] = value;
                }
            }
        }

        public Tile this[int x, int y]
        {
            get
            {
                if (!InBounds(x, y))
                {
                    return null;
                }

                if (TileArray[x, y] == null)
                {
                    return null;
                }

                return TileArray[x, y];
            }

            set
            {
                if (!InBounds(x, y))
                {
                    return;
                }

                TileArray[x, y] = value;
            }
        }

        public Tile this[Point point]
        {
            get => this[point.X, point.Y];

            set => this[point.Y, point.Y] = value;
        }

        public Grid(int x, int y)
        {
            if (x < 0 || y < 0)
                throw new OverflowException("Tried to initialize array with negative dimension(s).");

            xLength = x;
            yLength = y;

            TileArray = new Tile[x, y];
            objects = new TileObject[MAXSIZE];
        }

        public List<TileObject> GetList()
        {
            try
            {
                List<TileObject> tempObjects = new List<TileObject>();

                for (int i = 0; i < MAXSIZE; i++)
                {
                    if (objects[i]!= null)
                    {
                        tempObjects.Add(objects[i]);
                    }
                }

                return tempObjects;
            }
            catch
            {
                return null;
            }
        }

        public Point PositionOf(int id)
        {
            for (int x = 0; x < xLength; x++)
            {
                for (int y = 0; y < yLength; y++)
                {
                    if (TileArray[x, y] != null && TileArray[x, y].TileObject.HasValue && TileArray[x, y].TileObject.Value == id)
                    {
                        return new Point(x, y);
                    }
                }
            }

            return new Point();
        }

        public void Remove(int id)
        {
            if (Objects[id] == null)
            {
                return;
            }

            TileArray[Objects[id].Position.X, objects[id].Position.Y].TileObject = null;
            objects[id] = null;
        }

        public void Remove(TileObject tObject)
        {
            if (tObject != null)
            {
                Remove(tObject.GridIndex);
            }
        }

        public void Remove(int x, int y)
        {
            if (InBounds(x, y))
            {
                objects[TileArray[x, y].TileObject.Value] = null;
                TileArray[x, y] = null;
            }
        }

        public TileObject Place(int x, int y, TileObject tObject)
        {
            if (!Vacant(x, y))
            {
                return null;
            }

            TileArray[x, y].TileObject = tObject.GridIndex;
            tObject.Position = new Point(x, y);

            return tObject;
        }

        public TileObject GetObject(int x, int y)
        {
            if (!InBounds(x, y) || TileArray[x, y] == null)
            {
                return null;
            }

            int? tileObject = TileArray[x, y].TileObject;

            return tileObject.HasValue ? objects[tileObject.Value] : null;
        }

        public void ChangePosition(TileObject tObject, int x, int y)
        {
            TileArray[tObject.Position.X, tObject.Position.Y].TileObject = null;
            TileArray[x, y].TileObject = tObject.GridIndex;
            tObject.Position = new Point(x, y);
            tObject.UpdateRenderer();
        }

        public void SwapPosition(TileObject tObject, Point target)
        {
            TileArray[tObject.Position.X, tObject.Position.Y].TileObject = GetObject(target.X, target.Y).GridIndex;
            GetObject(target.X, target.Y).Position = tObject.Position;
            GetObject(target.X, target.Y).UpdateRenderer();

            TileArray[target.X, target.Y].TileObject = tObject.GridIndex;
            tObject.Position = target;
            tObject.UpdateRenderer();
        }

        public int NewIndex()
        {
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i] == null)
                {
                    return i;
                }
            }

            throw new Exception("You surpassed the limit of on-board objects, dumbass.");
        }

        public bool Vacant(int x, int y)
            => InBounds(x, y) && TileArray[x, y] != null && !TileArray[x, y].TileObject.HasValue;

        public bool InBounds(int x, int y) 
            => x > -1 && y > -1 && x < xLength && y < yLength;
    }

    class Tile
    {
        public int? TileObject { get; set; }

        public Tile()
        {
            TileObject = null;
        }

        public Tile(int tileObject)
        {
            TileObject = tileObject;
        }
    }

    //    struct TileActivator
    //    {
    //        public event Action<TileObject> EnterTile, 
    //    }
}
