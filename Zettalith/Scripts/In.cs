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
    public static class In
    {
        static KeyboardState previous;
        static bool previousLeftMouse, previousRightMouse;

        public static void UpdateMethods()
        {
            previous = KeyboardState;

            MouseState state = MouseState;
            previousLeftMouse = state.LeftButton == ButtonState.Pressed;
            previousRightMouse = state.RightButton == ButtonState.Pressed;
        }

        public static MouseState MouseState => Mouse.GetState();

        public static KeyboardState KeyboardState => Keyboard.GetState();

        public static Point MousePosition => MouseState.Position;

        public static bool LeftMouse => MouseState.LeftButton == ButtonState.Pressed;

        public static bool RightMouse => MouseState.RightButton == ButtonState.Pressed;

        public static bool LeftMouseDown => LeftMouse && !previousLeftMouse;

        public static bool RightMouseDown => RightMouse && !previousRightMouse;

        public static bool Key(Keys key) => KeyboardState.IsKeyDown(key);

        public static bool KeyDown(Keys key) => (!previous.IsKeyDown(key) && KeyboardState.IsKeyDown(key));
    }
}
