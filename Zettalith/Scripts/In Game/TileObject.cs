using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Zettalith
{
    abstract class TileObject
    {
        public int Index { get; set; }
        public Point Position { get; set; }

        public TileObject()
        {
            
        }

        public void Destroy()
        {
            InGameController.Grid.Remove(this);
        }
    }
}
