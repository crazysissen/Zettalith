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
    partial class GUI : GUIContainer
    {
        public class Collection : GUIContainer, IGUIMember
        {
            void IGUIMember.Draw(SpriteBatch spriteBatch, MouseState mouse, KeyboardState keyboard, float scaledDeltaTime, float unscaledDeltaTime)
            {

            }
        }

        public class Panel : GUIContainer, IGUIMember
        {
            void IGUIMember.Draw(SpriteBatch spriteBatch, MouseState mouse, KeyboardState keyboard, float scaledDeltaTime, float unscaledDeltaTime)
            {

            }
        }

        public class ScrollView : GUIContainer, IGUIMember
        {
            void IGUIMember.Draw(SpriteBatch spriteBatch, MouseState mouse, KeyboardState keyboard, float scaledDeltaTime, float unscaledDeltaTime)
            {

            }
        }

        /// <summary>Standard image on-screen</summary>
        public class Image : IGUIMember
        {
            Renderer.SpriteScreen renderer;

            public Image(Texture2D texture, Rectangle transform)
            {
                renderer = new Renderer.SpriteScreen(texture, transform);
                renderer.Automatic = false;
            }

            public Image(Texture2D texture, Rectangle transform, Color color)
            {
                renderer = new Renderer.SpriteScreen(texture, transform, color);
                renderer.Automatic = false;
            }

            public Image(Texture2D texture, Rectangle transform, Color color, float rotation, Vector2 origin, SpriteEffects effects)
            {
                renderer = new Renderer.SpriteScreen(texture, transform, color, rotation, origin, effects);
                renderer.Automatic = false;
            }

            void IGUIMember.Draw(SpriteBatch spriteBatch, MouseState mouse, KeyboardState keyboard, float scaledDeltaTime, float unscaledDeltaTime)
            {
                renderer.Draw(spriteBatch, null, scaledDeltaTime);
            }
        }

        public class ImageAnimation : IGUIMember
        {


            public ImageAnimation()
            {

            }

            void IGUIMember.Draw(SpriteBatch spriteBatch, MouseState mouse, KeyboardState keyboard, float scaledDeltaTime, float unscaledDeltaTime)
            {

            }
        }

        public class Button : IGUIMember
        {
            void IGUIMember.Draw(SpriteBatch spriteBatch, MouseState mouse, KeyboardState keyboard, float scaledDeltaTime, float unscaledDeltaTime)
            {

            }
        }

        public class Text : IGUIMember
        {
            void IGUIMember.Draw(SpriteBatch spriteBatch, MouseState mouse, KeyboardState keyboard, float scaledDeltaTime, float unscaledDeltaTime)
            {

            }
        }
    }
}
