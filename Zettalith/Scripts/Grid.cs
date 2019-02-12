using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    class Grid
    {
        const int
            MAXSIZE = 4096;

        public readonly int xLength, yLength;

        Tile[,] _tileArray;
        TileObject[] _objects;

        public TileObject this[int id]
        {
            get => id >= 0 && id < _objects.Length ? _objects[id] : null;

            set
            {
                if (id >= 0 && id < _objects.Length)
                {
                    _objects[id] = value;
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

                if (_tileArray[x, y] == null)
                {
                    return null;
                }

                return _tileArray[x, y];
            }

            set
            {
                if (!InBounds(x, y))
                {
                    return;
                }

                _tileArray[x, y] = value;
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

            _tileArray = new Tile[x, y];
            _objects = new TileObject[MAXSIZE];
        }

        public Point PositionOf(int id)
        {
            for (int x = 0; x < xLength; x++)
            {
                for (int y = 0; y < yLength; y++)
                {
                    if (_tileArray[x, y] != null && _tileArray[x, y].TileObject.HasValue && _tileArray[x, y].TileObject.Value == id)
                    {
                        return new Point(x, y);
                    }
                }
            }

            return new Point();
        }

        public void Remove(int id)
        {
            if (_objects[id] == null)
            {
                return;
            }

            _tileArray[_objects[id].Position.X, _objects[id].Position.Y] = null;
            _objects[id] = null;
        }

        public void Remove(TileObject tObject)
        {
            if (tObject != null)
            {
                Remove(tObject.Index);
            }
        }

        public void Remove(int x, int y)
        {
            if (InBounds(x, y))
            {
                _objects[_tileArray[x, y].TileObject.Value] = null;
                _tileArray[x, y] = null;
            }
        }

        public void Place(int x, int y, TileObject tObject)
        {
            if (!Vacant(x, y))
            {
                return;
            }

            _tileArray[x, y].TileObject = tObject.Index;
            tObject.Position = new Point(x, y);
        }

        public TileObject GetObject(int x, int y)
        {
            if (!InBounds(x, y))
            {
                return null;
            }

            int? tileObject = _tileArray[x, y].TileObject;

            return tileObject.HasValue ? _objects[tileObject.Value] : null;
        }

        public void ChangePosition(TileObject tObject, int x, int y)
        {
            if (Vacant(x, y))
            {
                _tileArray[tObject.Position.X, tObject.Position.Y].TileObject = null;
                _tileArray[x, y].TileObject = tObject.Index;
                tObject.Position = new Point(x, y);
            }
        }

        public int NewIndex()
        {
            int i = 0;
            while (_objects[i] != null)
            {
                ++i;
            }

            return i;
        }

        public bool Vacant(int x, int y)
            => InBounds(x, y) && _tileArray[x, y] != null && !_tileArray[x, y].TileObject.HasValue;

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
