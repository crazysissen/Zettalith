using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Design;

namespace Zettalith
{
    // OBS STULEN RAKT FRÅN HOMICIDAL

    public class TestGUI
    {
        List<IGUIMember> members;

        /// <summary>
        /// Starts a new queue for this GUI instance, do this every frame or else it will stack
        /// </summary>
        public void New() => members = new List<IGUIMember>();

        /// <summary>
        /// Adds new IGUIElements to the GUI
        /// </summary>
        public void Add(params IGUIMember[] member) => members.AddRange(member);

        public void Draw(SpriteBatch spriteBatch)
        {
            int order = 0;
            foreach (IGUIMember member in members)
                member.Draw(spriteBatch, ++order);
        }

        // Nested types, etc.

        public interface IGUIMember
        {
            void Draw(SpriteBatch spriteBatch, int order);
        }

        public struct Label : IGUIMember
        {
            const uint fontSizeComparator = 100;

            string text;
            uint fontSize;
            Vector2 position;
            SpriteFont font;
            Color color;

            public Label(string text, uint fontSize, Vector2 position, SpriteFont font, Color color)
            {
                this.text = text;
                this.fontSize = fontSize;
                this.position = position;
                this.font = font;
                this.color = color;
            }

            void IGUIMember.Draw(SpriteBatch spriteBatch, int order) => spriteBatch.DrawString(font, text, position, color, 0, Vector2.Zero, (float)fontSize / fontSizeComparator, SpriteEffects.None, 0.2f - 0.0001f * order);
        }

        public struct Texture : IGUIMember
        {
            Rectangle rectangle;
            Texture2D texture;
            Color color;

            public Texture(Rectangle rectangle, Texture2D texture, Color color)
            {
                this.rectangle = rectangle;
                this.texture = texture;
                this.color = color;
            }

            /// <summary>
            /// Empty square with the desired color
            /// </summary>
            public Texture(Rectangle rectangle, Color color)
            {
                this.rectangle = rectangle;
                this.color = color;

                texture = Load.Get<Texture2D>("Square");
            }

            void IGUIMember.Draw(SpriteBatch spriteBatch, int order) => spriteBatch.Draw(texture, rectangle, null, color, 0, Vector2.Zero, SpriteEffects.None, 0.2f - 0.0001f * order);
        }

        public struct Button : IGUIMember
        {
            Rectangle rectangle;
            Color[] colors;
            Texture2D texture;
            bool textured;

            public delegate void Call();
            enum GUIButtonState { Neutral, Hover, Click }
            GUIButtonState currentState;

            /// <summary>
            /// Textured button
            /// </summary>
            /// <param name="callbackMethod">Method called on click</param>
            public Button(Rectangle rectangle, Texture2D texture, Color normalColor, Color hoverColor, Color clickedColor, Call callbackMethod)
            {
                this.rectangle = rectangle;
                colors = new Color[3] { normalColor, hoverColor, clickedColor };
                this.texture = texture;
                textured = true;

                // All fields must be asigned before local methods can be called
                currentState = GUIButtonState.Neutral;
                currentState = GetState();
                if (currentState == GUIButtonState.Click)
                    callbackMethod.Invoke();
            }

            /// <summary>
            /// Empty area with button functionality
            /// </summary>
            /// <param name="callbackMethod">Method called on click</param>
            public Button(Rectangle rectangle, Call callbackMethod)
            {
                this.rectangle = rectangle;
                colors = new Color[0];
                texture = null;
                textured = false;

                // All fields must be asigned before local methods can be called
                currentState = GUIButtonState.Neutral;
                currentState = GetState();

                if (currentState == GUIButtonState.Click)
                    callbackMethod.Invoke();
            }

            GUIButtonState GetState()
            {
                MouseState state = Mouse.GetState();

                if (state.Position.X >= rectangle.Left && state.Position.X <= rectangle.Right && state.Position.Y >= rectangle.Top && state.Position.Y <= rectangle.Bottom)
                    return Input.LeftMouseDown ? GUIButtonState.Click : GUIButtonState.Hover;

                return GUIButtonState.Neutral;
            }

            void IGUIMember.Draw(SpriteBatch spriteBatch, int order)
            {
                if (textured)
                    spriteBatch.Draw(texture, rectangle, null, colors[(int)currentState], 0, Vector2.Zero, SpriteEffects.None, 0.2f - 0.0001f * order);
            }
        }
    }
}
