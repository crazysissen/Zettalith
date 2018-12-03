using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    static class RendererController
    {
        public static Camera Camera { get; private set; }
        public static TestGUI TestGUI { get; set; }
        public static GUI GUI { get; set; }

        private static List<Renderer> renderers = new List<Renderer>();
        private static List<(GUIContainerMasked, Point)> renderMasks = new List<(GUIContainerMasked, Point)>();

        public static void Initialize(GraphicsDeviceManager graphics, Vector2 cameraPosition, float cameraScale)
        {
            GUI = new GUI();

            Camera = new Camera(graphics)
            {
                Position = cameraPosition,
                Scale = cameraScale
            };
        }

        public static void Render(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
        {
            XNAController.Graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.Stencil, Color.DarkBlue, 0, 0);

            MouseState mouseState = In.MouseState;
            KeyboardState keyboardState = In.KeyboardState;

            renderMasks.Clear();

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            foreach (Renderer renderer in renderers)
            {
                if (renderer.Automatic)
                {
                    renderer.Draw(spriteBatch, Camera, (float)gameTime.ElapsedGameTime.TotalSeconds);
                }
            }

            GUI.Draw(spriteBatch, mouseState, keyboardState, (float)gameTime.ElapsedGameTime.TotalSeconds);

            TestGUI.Draw(spriteBatch);

            spriteBatch.End();

            var m = Matrix.CreateOrthographicOffCenter(0,
                graphics.GraphicsDevice.PresentationParameters.BackBufferWidth,
                graphics.GraphicsDevice.PresentationParameters.BackBufferHeight,
                0, 0, 1);

            var a = new AlphaTestEffect(graphics.GraphicsDevice)
            {
                Projection = m
            };

            int iterations = 1;
            foreach ((GUIContainerMasked mask, Point position) item in renderMasks)
            {
                var s1 = new DepthStencilState
                {
                    StencilEnable = true,
                    StencilFunction = CompareFunction.Always,
                    StencilPass = StencilOperation.Replace,
                    ReferenceStencil = iterations,
                    DepthBufferEnable = false,
                };

                var s2 = new DepthStencilState
                {
                    StencilEnable = true,
                    StencilFunction = CompareFunction.LessEqual,
                    StencilPass = StencilOperation.Keep,
                    ReferenceStencil = iterations,
                    DepthBufferEnable = true,
                };

                Texture2D transparent = new Texture2D(graphics.GraphicsDevice, 1, 1);
                transparent.SetData(new Color[] { Color.Transparent });

                spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, s1, null, a);
                spriteBatch.Draw(transparent, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                spriteBatch.Draw(item.mask.Mask.MaskTexture, new Rectangle(item.mask.Mask.Rectangle.Location + item.mask.Origin, item.mask.Mask.Rectangle.Size), Color.White);
                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, s2, null, null);
                GUI.DrawContainer(item.mask, spriteBatch, mouseState, keyboardState, (float)gameTime.ElapsedGameTime.TotalSeconds, item.position);
                spriteBatch.End();

                ++iterations;
            }
        }

        public static void TemporaryAddMask(GUIContainerMasked mask, Point origin)
            => renderMasks.Add((mask, origin));

        public static void AddRenderer(Renderer renderer)
            => renderers.Add(renderer);

        public static void RemoveRenderer(Renderer renderer)
            => renderers.Remove(renderer);
    }
}
