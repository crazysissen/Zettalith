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
            void IGUIMember.Draw(SpriteBatch spriteBatch, MouseState mouse, KeyboardState keyboard, float unscaledDeltaTime)
            {

            }
        }
        
        public class Mask : GUIContainerMasked, IGUIMember
        {
            void IGUIMember.Draw(SpriteBatch spriteBatch, MouseState mouse, KeyboardState keyboard, float unscaledDeltaTime)
            {

            }
        }

        public class Panel : GUIContainer, IGUIMember
        {
            void IGUIMember.Draw(SpriteBatch spriteBatch, MouseState mouse, KeyboardState keyboard, float unscaledDeltaTime)
            {

            }
        }

        public class ScrollView : GUIContainer, IGUIMember
        {
            void IGUIMember.Draw(SpriteBatch spriteBatch, MouseState mouse, KeyboardState keyboard, float unscaledDeltaTime)
            {

            }
        }

        public class Button : IGUIMember
        {
            public Action OnMouseDown { get; set; }
            public Action OnMosueUp { get; set; }
            public Action OnClick { get; set; }

            public bool Hovering { get; private set; }

            public Renderer.SpriteScreen Renderer { get; set; }

            void IGUIMember.Draw(SpriteBatch spriteBatch, MouseState mouse, KeyboardState keyboard, float unscaledDeltaTime)
            {
                //Hovering = 
            }
        }
    }
}
