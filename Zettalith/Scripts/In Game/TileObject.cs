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
        public int GridIndex { get; set; }
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
            Renderer.Position = new Vector2(Position.X, Position.Y * ClientSideController.HEIGHTDISTANCE) * (InGameController.IsHost ? 1 : -1);
            Renderer.Layer = DefaultLayer(Position.Y);
        }

        public static Layer DefaultLayer(int y) => InGameController.IsHost ?
            new Layer(MainLayer.Main, (y - InGameController.Grid.yLength) * 2 - 1) :
            new Layer(MainLayer.Main, (-y) * 2 - 1);
    }
}
