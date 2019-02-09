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

        readonly int _xL, _yL;

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

            _xL = x;
            _yL = y;

            _tileArray = new Tile[x, y];
            _objects = new TileObject[MAXSIZE];
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

        public bool InBounds(int x, int y)
        {
            return x > -1 && y > -1 && x < _xL && y < _yL;
        }
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
