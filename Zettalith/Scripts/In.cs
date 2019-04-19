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
    public static class Input
    {
        //static KeyboardState previous;
        //static bool previousLeftMouse, previousRightMouse;

        //public static void UpdateMethods()
        //{
        //    previous = KeyboardState;

        //    MouseState state = MouseState;
        //    previousLeftMouse = state.LeftButton == ButtonState.Pressed;
        //    previousRightMouse = state.RightButton == ButtonState.Pressed;
        //}

        //public static MouseState MouseState => Mouse.GetState();

        //public static KeyboardState KeyboardState => Keyboard.GetState();

        //public static Point MousePosition => MouseState.Position;

        //public static bool LeftMouse => MouseState.LeftButton == ButtonState.Pressed;

        //public static bool RightMouse => MouseState.RightButton == ButtonState.Pressed;

        //public static bool LeftMouseDown => LeftMouse && !previousLeftMouse;

        //public static bool RightMouseDown => RightMouse && !previousRightMouse;

        //public static bool Key(Keys key) => KeyboardState.IsKeyDown(key);

        //public static bool KeyDown(Keys key) => (!previous.IsKeyDown(key) && KeyboardState.IsKeyDown(key));

        public static KeyboardState KeyboardState => myKState;
        public static MouseState MouseState => myMState;

        public static Point MousePosition => myMState.Position;
        public static bool LeftMouse => myMState.LeftButton == ButtonState.Pressed;
        public static bool RightMouse => myMState.RightButton == ButtonState.Pressed;
        public static bool LeftMouseDown => myMState.LeftButton == ButtonState.Pressed && myLastMState.LeftButton == ButtonState.Released;
        public static bool RightMouseDown => myMState.RightButton == ButtonState.Pressed && myLastMState.RightButton == ButtonState.Released;
        public static int ScrollWheelChange => myScrollWheelState == myLastScrollWheelState ? 0 : (myScrollWheelState > myLastScrollWheelState ? 1 : -1);

        private static KeyboardState myKState, myLastKState;
        private static MouseState myMState, myLastMState;
        private static GamePadState myGState, myLastGState;

        private static int myScrollWheelState, myLastScrollWheelState;

        public static void Init()
        {
            myKState = new KeyboardState();
            myMState = new MouseState();

            myLastKState = myKState;
            myLastMState = myMState;

            myScrollWheelState = myMState.ScrollWheelValue;
        }

        /// <summary>
        /// Called at the start of every frame
        /// </summary>
        public static void Update()
        {
            myLastKState = myKState;
            myLastMState = myMState;
            myLastGState = myGState;

            myLastScrollWheelState = myScrollWheelState;

            myKState = Keyboard.GetState();
            myMState = Mouse.GetState();
            myGState = GamePad.GetState(1);

            myScrollWheelState = myMState.ScrollWheelValue;
        }

        public static bool Key(Keys aKey)
            => myKState.IsKeyDown(aKey);

        public static bool KeyDown(Keys aKey)
            => myKState.IsKeyDown(aKey) && myLastKState.IsKeyUp(aKey);
    }
}
