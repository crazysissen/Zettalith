using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    class Grid
    {
        const int
            MAXSIZE = 4096;

        int _xL, _yL, _zL;

        Tile[,,] _tileArray;
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

        public TileObject this[int x, int y, int z]
        {
            get
            {
                if (!InBounds(x, y, z))
                {
                    return null;
                }

                if (_tileArray[x, y, z] == null)
                {
                    _tileArray[x, y, z] = new Tile();
                    return null;
                }

                return _objects[_tileArray[x, y, z].TileObject];
            }

            set
            {

            }
        }

        public TileObject this[Coordinate coordinate]
        {
            get => this[coordinate.x, coordinate.y, coordinate.z];

            set => this[coordinate.x, coordinate.y, coordinate.z] = value;
        }

        public Grid(int x, int y, int z)
        {
            if (x < 0 || y < 0 || z < 0)
                throw new OverflowException("Tried to initialize array with negative dimension(s).");

            _xL = x;
            _yL = y;
            _zL = z;

            _tileArray = new Tile[x, y, z];
            _objects = new TileObject[MAXSIZE];
        }

        public bool InBounds(int x, int y, int z)
        {
            return x > -1 && y > -1 && z > -1 && x < _xL && y < _yL && z < _zL;
        }
    }

    struct Coordinate
    {
        public int x, y, z;

        public Coordinate(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    class Tile
    {
        public int TileObject { get; set; }

        public Tile()
        {
            TileObject = 0;

            Grid g = new Grid(1, 1, 1);
            g[]
        }
    }

    //    struct TileActivator
    //    {
    //        public event Action<TileObject> EnterTile, 
    //    }
}
