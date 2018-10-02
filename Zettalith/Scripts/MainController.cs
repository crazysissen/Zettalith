using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Zettalith
{
    public class MainController
    {
        Renderer.Sprite renderer;

        public MainController()
        {
            
        }

        public void Initialize(XNAController game)
        {
            RendererController.Initialize(new Vector2(0, 0), 1);
        }

        public void LateInitialize(XNAController game)
        {
            renderer = new Renderer.Sprite(ContentController.Get<Texture2D>("Square"), new Vector2(0, 0), new Vector2(5, 5), Color.White, 0, SpriteEffects.None);

        }

        public void Update(XNAController game, GameTime gameTime)
        {

            DirectInput.UpdateMethods();
        }


        public void Draw(XNAController game, GameTime gameTime, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            RendererController.Render(graphics, spriteBatch, gameTime);
        }
    }
}
