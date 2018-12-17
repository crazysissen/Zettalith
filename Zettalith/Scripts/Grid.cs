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

        Tile[,,] _tileArray;
        TileObject[] _objects;

        public Grid(int x, int y, int z)
        {
            _tileArray = new Tile[x, y, z];
            _objects = new TileObject[MAXSIZE];
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

    struct Tile
    {
        public int TileObject { get; set; }
    }

    //    struct TileActivator
    //    {
    //        public event Action<TileObject> EnterTile, 
    //    }
}
