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

        public class Image : IGUIMember
        {
            Renderer.SpriteScreen renderer;

            /// <summary>Standard image on-screen</summary>
            public Image(Texture2D texture, Rectangle transform)
            {
                renderer = new Renderer.SpriteScreen(texture, transform);
                renderer.Automatic = false;
            }

            /// <summary></summary>
            //public Image(Texture2D texture, Rectangle transform, Color)
            //{
            //    renderer = new Renderer.SpriteScreen();
            //    renderer.Automatic = false;
            //}

            void IGUIMember.Draw(SpriteBatch spriteBatch, MouseState mouse, KeyboardState keyboard, float scaledDeltaTime, float unscaledDeltaTime)
            {
                renderer.Draw(spriteBatch, null, scaledDeltaTime);
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
