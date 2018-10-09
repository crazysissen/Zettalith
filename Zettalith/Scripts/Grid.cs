using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    class Grid<T>
    {
        Tile[,,]
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

    }

    struct TileActivator
    {
        public event Action<TileObject> 
    }
}
