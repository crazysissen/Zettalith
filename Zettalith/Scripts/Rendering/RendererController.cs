using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    static class RendererController
    {
        public static Camera Camera { get; private set; }

        public static TestGUI TestGUI { get; set; }

        static List<Renderer> renderers = new List<Renderer>();

        public static void Initialize(GraphicsDeviceManager graphics, Vector2 cameraPosition, float cameraScale)
        {
            Camera = new Camera(graphics)
            {
                Position = cameraPosition,
                Scale = cameraScale
            };
        }

        public static void Render(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            foreach (Renderer renderer in renderers)
            {
                if (renderer.Automatic)
                {
                    renderer.Draw(spriteBatch, Camera, (float)gameTime.ElapsedGameTime.TotalSeconds);
                }
            }

            TestGUI.Draw(spriteBatch);

            spriteBatch.End();
        }

        public static void AddRenderer(Renderer renderer)
            => renderers.Add(renderer);

        public static void RemoveRenderer(Renderer renderer)
            => renderers.Remove(renderer);
    }
}
