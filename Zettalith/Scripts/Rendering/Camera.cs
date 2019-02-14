using Microsoft.Xna.Framework;

namespace Zettalith
{
    public class Camera
    {
        // A square based on the average distances to the screen edges, divided into pieces
        const float
            UNIVERSALMODIFIER = 1.0f;

        public const int
            WORLDUNITPIXELS = 64;

        public Vector2 Position { get; set; }
        public float Scale { get; set; }

        public Vector2 CenterCoordinate { get; private set; }

        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }

        private float _standardWUScaling, _standardSquareDiameter;

        public float WorldUnitDiameter => _standardWUScaling * _standardSquareDiameter;

        public Camera(GraphicsDeviceManager graphics)
        {
            ScreenWidth = graphics.PreferredBackBufferWidth;
            ScreenHeight = graphics.PreferredBackBufferHeight;

            _standardSquareDiameter = 0.5f * (ScreenWidth + ScreenHeight);

            _standardWUScaling = _standardSquareDiameter / WORLDUNITPIXELS;

            CenterCoordinate = new Vector2(ScreenWidth * 0.5f, ScreenHeight * 0.5f);
        }

        public Vector2 WorldToScreenPosition(Vector2 worldPosition)
            => CenterCoordinate + (worldPosition - Position) * _standardSquareDiameter * 0.5f * Scale * UNIVERSALMODIFIER;

        public Vector2 ScreenToWorldPosition(Vector2 screenPosition)
            => (screenPosition - CenterCoordinate) / (_standardSquareDiameter * 0.5f * Scale * UNIVERSALMODIFIER) + Position;

        public Vector2 WorldToScreenSize(Vector2 size)
            => size * UNIVERSALMODIFIER * _standardWUScaling * Scale;

        public Vector2 ScreenToWorldSize(Vector2 size)
            => size / (UNIVERSALMODIFIER * Scale * _standardWUScaling);
    }
}