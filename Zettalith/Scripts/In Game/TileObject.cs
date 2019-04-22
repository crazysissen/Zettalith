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
        public Point RenderPosition
        {
            get
            {
                int multiplier = InGameController.IsHost ? 1 : -1;
                return new Point(Position.X * multiplier, Position.Y * multiplier);
            }
        }

        public Renderer.Sprite Renderer { get; set; }

        public TileObject()
        {
            GridIndex = InGameController.Grid.NewIndex();
            InGameController.Grid.Objects[GridIndex] = this;
        }

        public void Destroy()
        {
            ClientSideController.Particles.Destroy(this, 1);

            Renderer.Destroy();
            Renderer = null;
            InGameController.Grid.Remove(this);
        }

        public void UpdateRenderer()
        {
            Renderer.Position = SupposedPosition;
            Renderer.Layer = DefaultLayer(Position.Y);
        }

        public Vector2 SupposedPosition => new Vector2(Position.X, Position.Y * ClientSideController.HEIGHTDISTANCE) * (InGameController.IsHost ? 1 : -1);

        public static Layer DefaultLayer(int y) => InGameController.IsHost ?
            new Layer(MainLayer.Main, (y - InGameController.Grid.yLength) * 2 - 1) :
            new Layer(MainLayer.Main, (-y) * 2 - 1);
    }
}
