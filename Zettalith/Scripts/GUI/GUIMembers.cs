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
            void IGUIMember.Draw(SpriteBatch spriteBatch, MouseState mouse, KeyboardState keyboard, float unscaledDeltaTime) { }
        }
        
        public class MaskedCollection : GUIContainerMasked, IGUIMember
        {
            void IGUIMember.Draw(SpriteBatch spriteBatch, MouseState mouse, KeyboardState keyboard, float unscaledDeltaTime) { }
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
            public enum State { Idle, Hovered, Clicked }
            public enum Type { NoEffect, ColorSwitch, TextureSwitch, AnimatedSwitch }
            public enum Transition { Custom, Switch, LinearFade, DecelleratingFade, AcceleratingFade }

            public event Action OnEnter;
            public event Action OnExit;
            public event Action OnMouseDown;
            public event Action OnClick;

            public State CurrentState { get; private set; }
            public Type DisplayType { get; private set; }
            public Transition TransitionType { get; set; }
            public float TransitionTime { get; set; }
            public Rectangle Transform { get; set; }

            public Renderer.Text Text { get; set; }
            public Texture2D Texture { get; set; }
            public Texture2D[] TextureSwitch { get; private set; }
            public Color[] ColorSwitch { get; private set; }

            public Layer layer;

            private Func<float, float> _transition;
            private Color _textBaseColor;
            private float _currentTime;
            private bool _beginHoldOnButton, _pressedLastFrame;
            private Color _startColor, _targetColor;
            private State _actualState;

            /// <summary>Testing button</summary>
            public Button(Rectangle transform, Color color)
            {

            }

            /// <summary>Simple button with preset color multipliers, for testing primarily</summary>
            public Button(Rectangle transform, Texture2D texture)
            {

            }

            /// <summary>Simple button that switches color (color multiplier) when hovered/clicked</summary>
            public Button(Rectangle transform, Texture2D texture, Color idle, Color hover, Color click)
            {

            }

            /// <summary>Simple button that changes color (color multiplier) when hovered/clicked according to a set transition type and time</summary>
            public Button(Rectangle transform, Texture2D texture, Color idle, Color hover, Color click, Transition transitionType, float transitionTime)
            {

            }

            /// <summary>Button that switches texture when hovered/clicked</summary>
            public Button(Rectangle transform, Texture2D idle, Texture2D hover, Texture2D click)
            {

            }

            /// <summary>Button that changes texture when hovered/clicked according to a set transition type and time</summary>
            public Button(Rectangle transform, Texture2D idle, Texture2D hover, Texture2D click, Transition transitionType, float transitionTime)
            {

            }

            void IGUIMember.Draw(SpriteBatch spriteBatch, MouseState mouse, KeyboardState keyboard, float unscaledDeltaTime)
            {
                bool onButton = Transform.Contains(mouse.Position);
                bool pressed = mouse.LeftButton == ButtonState.Pressed;
                
                if (!pressed)
                {
                    _beginHoldOnButton = false;
                }

                if (pressed && !_pressedLastFrame && onButton)
                {
                    _beginHoldOnButton = true;
                }

                switch (CurrentState)
                {
                    case State.Idle:
                        
                        if (onButton)
                        {
                            OnEnter.Invoke();

                            if (pressed)
                            {
                                CurrentState = State.Clicked;

                                if (!_pressedLastFrame)
                                {
                                    OnMouseDown.Invoke();
                                }
                            }

                            if (!pressed)
                            {
                                CurrentState = State.Hovered;
                            }
                        }

                        break;

                    case State.Hovered:

                        if (onButton && pressed && _beginHoldOnButton)
                        {
                            OnMouseDown.Invoke();

                            CurrentState = State.Clicked;
                        }

                        if (!onButton)
                        {
                            OnExit.Invoke();

                            CurrentState = State.Idle;
                        }

                        break;

                    case State.Clicked:

                        if (!onButton)
                        {
                            OnExit.Invoke();

                            CurrentState = State.Idle;
                        }

                        if (!pressed && onButton)
                        {
                            if (_beginHoldOnButton)
                            {
                                OnClick.Invoke();
                            }

                            CurrentState = State.Hovered;
                        }

                        break;
                }

                Texture2D texture;
                Color color;

                //if ()
                //{

                //}

                _pressedLastFrame = pressed;

                //spriteBatch.Draw()
            }

            public void SetTransitionType(Transition type)
            {
                if (type != Transition.Custom)
                {
                    TransitionType = type;
                }

                switch (type)
                {
                    case Transition.Custom:
                        Test.Log("Can't set transition type to 'Custom' explicitly.");
                        return;

                    case Transition.Switch:
                        _transition = o => (float)Math.Ceiling(o);
                        return;

                    case Transition.LinearFade:
                        _transition = o => o;
                        return;

                    case Transition.DecelleratingFade:
                        _transition = o => Mathz.SineD(o);
                        return;

                    case Transition.AcceleratingFade:
                        _transition = o => Mathz.SineA(o);
                        return;
                }
            }

            public void AddText(string text, int fontSize, bool centered, Color baseColor, SpriteFont font)
            {
                Text = new Renderer.Text(
                    layer, font, text, fontSize, 0, 
                    centered ? new Vector2((Transform.Left + Transform.Right) * 0.5f, (Transform.Top + Transform.Bottom) * 0.5f) : new Vector2(Transform.Left + 2, (Transform.Top + Transform.Bottom) * 0.5f), 
                    centered ? new Vector2(0.5f, 0.5f) : new Vector2(0, 0.5f), 
                    baseColor);

                _textBaseColor = baseColor;
            }

            public void SetTransitionExplicit(Func<float, float> function) 
                => _transition = function;

            public void SetTextureSwitch(Texture2D idle, Texture2D hover, Texture2D click)
                => TextureSwitch = new Texture2D[] { idle, hover, click };

            public void SetColorSwitch(Color idle, Color hover, Color click)
                => ColorSwitch = new Color[] { idle, hover, click };
        }
    }
}
