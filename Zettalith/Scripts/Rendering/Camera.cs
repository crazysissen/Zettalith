using Microsoft.Xna.Framework;

namespace Zettalith
{
    public class Camera
    {
        public const int
            WORLDUNITPIXELS = 1500;

        // A square based on the average distances to the screen edges, divided into pieces
        private const float
            UNIVERSALMODIFIER = 0.1f;

        public Vector2 Position { get; set; }
        public float Scale { get; set; }

        public Vector2 CenterCoordinate { get; private set; }

        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }

        public float WorldUnitDiameter => _standardWUScaling * _standardSquareDiameter;

        private float _standardWUScaling, _standardSquareDiameter;

        public Camera(GraphicsDeviceManager graphics)
        {
            ScreenWidth = graphics.PreferredBackBufferWidth;
            ScreenHeight = graphics.PreferredBackBufferHeight;

            _standardSquareDiameter = 0.5f * (ScreenWidth + ScreenHeight);

            _standardWUScaling = _standardSquareDiameter / WORLDUNITPIXELS;

            CenterCoordinate = new Vector2(ScreenWidth * 0.5f, ScreenHeight * 0.5f);
        }

        public Vector2 WorldToScreenPosition(Vector2 worldPosition)
            => CenterCoordinate + (worldPosition - Position) * _standardSquareDiameter * _standardWUScaling * Scale * UNIVERSALMODIFIER;

        public Vector2 ScreenToWorldPosition(Vector2 screenPosition)
            => (screenPosition - CenterCoordinate) / (_standardSquareDiameter * _standardWUScaling * Scale * UNIVERSALMODIFIER) + Position;

        public Vector2 WorldToScreenSize(Vector2 size)
            => size * UNIVERSALMODIFIER * Scale;

        public Vector2 ScreenToWorldSize(Vector2 size)
            => size / (UNIVERSALMODIFIER * Scale);
    }
}