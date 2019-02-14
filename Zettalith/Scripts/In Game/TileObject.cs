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

        public Renderer.Sprite Renderer { get; set; }

        public TileObject()
        {
            
        }

        public void Destroy()
        {
            InGameController.Grid.Remove(this);
        }

        public void UpdateRenderer()
        {
            Renderer.Position = new Vector2(Position.X, Position.Y * GameRendering.HEIGHTDISTANCE) * (InGameController.IsHost ? 1 : -1);
        }
    }
}
