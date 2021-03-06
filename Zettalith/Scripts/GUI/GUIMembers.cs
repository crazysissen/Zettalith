﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Zettalith
{
    partial class GUI : GUIContainer
    {
        public class Collection : GUIContainer, IGUIMember
        {
            Layer IGUIMember.Layer => Layer.Default;

            Point IGUIMember.Origin { get; set; }

            void IGUIMember.Draw(SpriteBatch spriteBatch, MouseState mouse, KeyboardState keyboard, float unscaledDeltaTime)
            {

            }
        }
        
        public class MaskedCollection : GUIContainerMasked, IGUIMember
        {
            Layer IGUIMember.Layer => Layer.Default;

            void IGUIMember.Draw(SpriteBatch spriteBatch, MouseState mouse, KeyboardState keyboard, float unscaledDeltaTime)
            {

            }
        }

        public class Panel : GUIContainer, IGUIMember
        {
            Layer IGUIMember.Layer => Layer.Default;

            void IGUIMember.Draw(SpriteBatch spriteBatch, MouseState mouse, KeyboardState keyboard, float unscaledDeltaTime)
            {

            }
        }

        public class ScrollView : GUIContainer, IGUIMember
        {
            Layer IGUIMember.Layer => Layer.Default;

            void IGUIMember.Draw(SpriteBatch spriteBatch, MouseState mouse, KeyboardState keyboard, float unscaledDeltaTime)
            {

            }
        }

        /// <summary>
        /// CURRENTLY ONLY ACCEPTS NUMBERS, PERIOD AND COLON
        /// </summary>
        public class TextField : GUIContainer, IGUIMember
        {
            const float
                FLASHTIME = 0.85f,
                HEIGHTPROPORTION = 0.05f;

            public enum TextType : byte { Letters = 0b1, Numbers = 0b10, Periods = 0b100, Space = 0b1000 }

            Point IGUIMember.Origin { get => _origin; set => _origin = value; }
            Layer IGUIMember.Layer => Layer;
            Point _origin = new Point();

            public string Content { get; set; }
            public int? MaxLetters { get; set; }
            public int CursorPosition { get; set; }
            public bool Highlighted { get; private set; }
            public TextType AllowedText { get; set; } = TextType.Letters | TextType.Numbers | TextType.Periods | TextType.Space;

            public Layer Layer { get; set; }
            public Rectangle Rectangle { get; set; }
            public MaskedCollection Mask { get; set; }
            public Renderer.SpriteScreen Renderer { get; set; }
            public Renderer.SpriteScreenFloating IBeam { get; set; }
            public Renderer.Text TextRenderer { get; set; }

            private float iBeamFlash;

            public TextField(Layer backgroundLayer, Layer textLayer, SpriteFont font, float fontSize, Rectangle transform, Vector2 position, Vector2 origin,  string text = null, Color? textColor = null, Color? textureColor = null, Texture2D backgroundTexture = null)
            {
                Layer = backgroundLayer;
                Rectangle = transform;
                Content = text;
                CursorPosition = 0;

                TextRenderer = new Renderer.Text(textLayer, font, text ?? "", fontSize, 0, position, origin, textColor ?? Color.Black);

                float height = font.MeasureString("I").Y * 0.65f;
                IBeam = new Renderer.SpriteScreenFloating(textLayer, Load.Get<Texture2D>("Square"), position + new Vector2(0, height / 5), new Vector2(height * HEIGHTPROPORTION, height) / 16, textColor ?? Color.Black, 0, new Vector2(height * HEIGHTPROPORTION * 18f, 0), /*Vector2.One * 0.5f, */SpriteEffects.None);
                IBeam.Active = false;

                Add(TextRenderer, IBeam);

                if (backgroundTexture != null)
                {
                    Renderer = new Renderer.SpriteScreen(backgroundLayer, backgroundTexture, transform);
                    Add(Renderer);
                }
            }

            void IGUIMember.Draw(SpriteBatch spriteBatch, MouseState mouse, KeyboardState keyboard, float unscaledDeltaTime)
            {
                bool onArea = RendererFocus.OnArea(new Rectangle(_origin + Rectangle.Location, Rectangle.Size), Layer);

                TextRenderer.String = new StringBuilder(Content);

                Mouse.SetCursor(onArea ? MouseCursor.IBeam : MouseCursor.Arrow);

                if (Input.LeftMouseDown)
                {
                    Click(onArea);
                }

                if (Highlighted)
                {
                    iBeamFlash += unscaledDeltaTime;

                    bool flashOn = iBeamFlash % FLASHTIME < FLASHTIME * 0.5f;
                    IBeam.Color = flashOn ? TextRenderer.Color : Color.Transparent;

                    float height = TextRenderer.Font.MeasureString("I").Y * 0.60f;
                    IBeam.Position = TextRenderer.Position + new Vector2(TextRenderer.Font.MeasureString(Content.Substring(0, CursorPosition)).X, 0) + new Vector2(0, height / 5);

                    KeyboardActions();
                }
            }

            public void ChangeState(bool active)
            {
                Highlighted = active;
                IBeam.Active = active;

                iBeamFlash = 0;
            }

            void Click(bool onArea)
            {
                if (onArea)
                {
                    ChangeState(true);

                    float tempPosition = Input.MousePosition.ToVector2().X - _origin.X - TextRenderer.Position.X, lastLength = float.MinValue;

                    List<float> lengths = new List<float>();

                    int? position = null;

                    for (int i = 0; i <= Content.Length && position == null; i++)
                    {
                        float currentLength = TextRenderer.Font.MeasureString(Content.Substring(0, i)).X * TextRenderer.Scale.X;
                        lengths.Add(currentLength);

                        if (currentLength > tempPosition)
                        {
                            position = (currentLength - tempPosition > tempPosition - lastLength) ? i - 1 : i;
                        }

                        lastLength = currentLength;
                    }

                    if (position == null)
                    {
                        position = Content.Length;
                    }

                    Test.Log("Position in text field: " + position);

                    CursorPosition = position.Value;
                }
                else
                {
                    ChangeState(false);
                }
            }

            void Write(object value)
            {
                if (MaxLetters == null || Content.Length <= MaxLetters)
                {
                    Content = Content.Insert(CursorPosition, value.ToString());
                    ++CursorPosition;
                }
            }

            void Back()
            {
                Content = Content.Remove(CursorPosition - 1, 1);
                --CursorPosition;
            }

            void KeyboardActions()
            {
                KeyboardState kState = Keyboard.GetState();
                Keys[] numbers = { Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9 };
                Keys[] numPad = { Keys.NumPad0, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4, Keys.NumPad5, Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9 };

                bool shift = kState.IsKeyDown(Keys.LeftShift) || kState.IsKeyDown(Keys.RightShift);

                if (AllowedText.HasFlag(TextType.Numbers))
                {
                    for (int i = 0; i < numbers.Length; ++i)
                    {
                        Keys key = numbers[i];

                        if (Input.KeyDown(key))
                        {
                            Write(i);
                        }
                    }

                    if (kState.NumLock)
                    {
                        for (int i = 0; i < numPad.Length; ++i)
                        {
                            Keys key = numPad[i];

                            if (Input.KeyDown(key))
                            {
                                Write(i);
                            }
                        }
                    }
                }

                if (AllowedText.HasFlag(TextType.Letters))
                {
                    for (int i = 65; i < 91; i++)
                    {
                        if (Input.KeyDown((Keys)i))
                        {
                            Write((char)(shift ? i : (i + 32)));
                        }
                    }
                }

                if (AllowedText.HasFlag(TextType.Periods))
                {
                    if (Input.KeyDown(Keys.OemPeriod))
                    {
                        if (shift)
                        {
                            Write(':');
                        }
                        else
                        {
                            Write('.');
                        }
                    }
                }

                if (AllowedText.HasFlag(TextType.Space))
                {
                    if (Input.KeyDown(Keys.Space))
                    {
                        Write(' ');
                    }
                }

                if (Input.KeyDown(Keys.Back) && Content.Length > 0 && CursorPosition > 0)
                {
                    Back();
                }

                if (Input.KeyDown(Keys.Delete) && CursorPosition < Content.Length)
                {
                    ++CursorPosition;
                    Back();
                }

                if (Input.KeyDown(Keys.Enter))
                {
                    ChangeState(false);
                }

                if (Input.KeyDown(Keys.Left) && CursorPosition > 0)
                {
                    --CursorPosition;
                }

                if (Input.KeyDown(Keys.Right) && CursorPosition < Content.Length)
                {
                    ++CursorPosition;
                }
            }
        }

        public class Button : IGUIMember
        {
            Point IGUIMember.Origin { get => _origin; set => _origin = value; }
            Point _origin = new Point();

            Layer IGUIMember.Layer => Layer;

            const float
                DEFAULTTRANSITIONTIME = 0.05f;

            public enum State { Idle, Hovered, Pressed }
            public enum Type { ColorSwitch, TextureSwitch, AnimatedSwitch }
            public enum Transition { Custom, Switch, LinearFade, DecelleratingFade, AcceleratingFade, EaseOutBack, EaseOutElastic, EaseOutCubic }

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
            public SpriteEffects SpriteEffects { get; set; }

            public bool ScaleEffect { get; set; } = false;
            public float ScaleEffectAmplitude { get; set; } = 1.0f;

            public Layer Layer { get; set; }

            private Func<float, float> _transition;
            private Color _textBaseColor;
            private float _currentTime, _targetTime, _timeMultiplier, _startScale, _targetScale, _currentScale = 1;
            private bool _inTransition, _beginHoldOnButton, _pressedLastFrame;
            private Color _startColor, _targetColor;
            private Texture2D _startTexture, _targetTexture;
            private State _startState;
            private SoundEffect _effect;

            private float[] _scaleSwitch = { 1.0f, 1.04f, 0.97f };

            /// <summary>Testing button</summary>
            public Button(Layer layer, Rectangle transform)
                : this(layer, transform, Load.Get<Texture2D>("Square"), new Color(0.9f, 0.9f, 0.9f))
            { }

            /// <summary>Testing button</summary>
            public Button(Layer layer, Rectangle transform, Color color)
                : this(layer, transform, Load.Get<Texture2D>("Square"), color)
            { }

            /// <summary>Simple button with preset color multipliers, for testing primarily</summary>
            public Button(Layer layer, Rectangle transform, Texture2D texture)
                : this(layer, transform, texture, new Color(0.9f, 0.9f, 0.9f))
            { }

            /// <summary>Simple button, for testing primarily</summary>
            public Button(Layer layer, Rectangle transform, Texture2D texture, Color color)
                : this(layer, transform, texture, PseudoDefaultColors(color), Transition.LinearFade, DEFAULTTRANSITIONTIME)
            { }

            /// <summary>Simple button that switches color (color multiplier) when hovered/clicked</summary>
            public Button(Layer layer, Rectangle transform, Texture2D texture, Color idle, Color hover, Color click)
                : this(layer, transform, texture, idle, hover, click, Transition.LinearFade, DEFAULTTRANSITIONTIME)
            { }

            /// <summary>Simple button that changes color (color multiplier) when hovered/clicked according to a set transition type and time</summary>
            public Button(Layer layer, Rectangle transform, Texture2D texture, Color idle, Color hover, Color click, Transition transitionType, float transitionTime)
                : this(layer, transform, texture, new Color[] { idle, hover, click }, transitionType, transitionTime)
            { }

            /// <summary>Simple button that changes color (color multiplier) when hovered/clicked according to a set transition type and time</summary>
            /// <param name="colorSwitch">Color array in order [idle, hover, click]</param>
            public Button(Layer layer, Rectangle transform, Texture2D texture, Color[] colorSwitch, Transition transitionType, float transitionTime)
            {
                DisplayType = Type.ColorSwitch;

                Layer = layer;

                Transform = transform;
                Texture = texture;
                ColorSwitch = colorSwitch;
                TransitionType = transitionType;
                TransitionTime = transitionTime;

                SetTransitionType(transitionType);
            }

            /// <summary>Button that switches texture when hovered/clicked</summary>
            public Button(Layer layer, Rectangle transform, Texture2D idle, Texture2D hover, Texture2D click)
                : this(layer, transform, idle, hover, click, Transition.LinearFade, DEFAULTTRANSITIONTIME)
            { }

            /// <summary>Button that changes texture when hovered/clicked according to a set transition type and time</summary>
            public Button(Layer layer, Rectangle transform, Texture2D idle, Texture2D hover, Texture2D click, Transition transitionType, float transitionTime)
            {
                DisplayType = Type.TextureSwitch;

                Layer = layer;

                Transform = transform;
                TextureSwitch = new Texture2D[] { idle, hover, click };
                TransitionType = transitionType;
                TransitionTime = transitionTime;

                SetTransitionType(transitionType);
            }

            void IGUIMember.Draw(SpriteBatch spriteBatch, MouseState mouse, KeyboardState keyboard, float unscaledDeltaTime)
            {
                bool onButton = RendererFocus.OnArea(new Rectangle(Transform.Location + _origin, Transform.Size), Layer);
                bool pressed = mouse.LeftButton == ButtonState.Pressed;

                Transfer(pressed, onButton);

                List<TAS> textures = new List<TAS>();
                Color color = Color.White;
                float scaledValue = _transition.Invoke(_currentTime);

                if (_currentTime >= 1)
                {
                    _inTransition = false;
                    _currentTime = 0;
                    _startState = CurrentState;
                }

                if (_inTransition)
                {
                    _currentTime += unscaledDeltaTime / TransitionTime;

                    if (ScaleEffect)
                    {
                        _currentScale = scaledValue.Lerp(_startScale, _targetScale);
                    }
                    else
                    {
                        _currentScale = 1;
                    }

                    switch (DisplayType)
                    {
                        case Type.ColorSwitch:
                            textures.Add(new TAS(Texture, 1));
                            color = Color.Lerp(_startColor, _targetColor, scaledValue);
                            break;

                        case Type.TextureSwitch:
                            textures.Add(new TAS(_startTexture, 1));
                            textures.Add(new TAS(_targetTexture, 1 - scaledValue));
                            color = Color.White;
                            break;
                    }
                }

                if (!_inTransition)
                {
                    if (ScaleEffect)
                    {
                        _currentScale = _scaleSwitch[(int)CurrentState];
                    }

                    switch (DisplayType)
                    {
                        case Type.ColorSwitch:
                            textures.Add(new TAS(Texture, 255));
                            color = ColorSwitch[(int)CurrentState];
                            break;

                        case Type.TextureSwitch:
                            textures.Add(new TAS(TextureSwitch[(int)CurrentState], 255));
                            color = Color.White;
                            break;
                    }
                }

                Vector2 
                    halfSize = new Vector2(Transform.Size.X * 0.5f, Transform.Size.Y * 0.5f),
                    middlePosition = Transform.Location.ToVector2() + halfSize;

                Rectangle targetRectangle = new Rectangle(_origin + (middlePosition - halfSize * _currentScale).RoundToPoint(), (Transform.Size.ToVector2() * _currentScale).RoundToPoint());

                for (int i = 0; i < textures.Count; ++i)
                {
                    spriteBatch.Draw(textures[i].texture, targetRectangle, null, new Color(color, textures[i].alpha), 0, new Vector2(0.5f, 0.5f), SpriteEffects, Layer.LayerDepth);
                }

                if (!pressed)
                {
                    _beginHoldOnButton = false;
                }

                _pressedLastFrame = pressed;
            }

            private void Transfer(bool pressed, bool onButton)
            {
                if (pressed && !_pressedLastFrame && onButton)
                {
                    _beginHoldOnButton = true;
                }

                if (CurrentState != State.Idle && !onButton)
                {
                    OnExit?.Invoke();
                    ChangeState(State.Idle);
                }
                else
                    switch (CurrentState)
                    {
                        case State.Idle:
                            if (onButton)
                            {
                                OnEnter?.Invoke();
                                if (pressed)
                                {
                                    ChangeState(State.Pressed);
                                    if (!_pressedLastFrame)
                                        OnMouseDown?.Invoke();
                                }
                                if (!pressed)
                                    ChangeState(State.Hovered);
                            }
                            break;

                        case State.Hovered:
                            if (onButton && pressed && _beginHoldOnButton)
                            {
                                OnMouseDown?.Invoke();
                                ChangeState(State.Pressed);
                            }
                            break;

                        case State.Pressed:
                            if (!pressed && onButton)
                            {
                                ChangeState(State.Hovered);
                                if (_beginHoldOnButton)
                                {
                                    OnClick?.Invoke();

                                    if (_effect != null)
                                    {
                                        _effect.Play();
                                    }
                                }
                            }
                            break;
                    }
            }

            private void ChangeState(State state)
            {
                State previousStartState = _startState;

                _startState = CurrentState;
                CurrentState = state;

                _inTransition = true;
                _targetTime = (_inTransition && state == previousStartState) ? _targetTime - _currentTime : 1;
                _currentTime = 0;

                _startScale = (_scaleSwitch[(int)_startState] - 1) * ScaleEffectAmplitude + 1;
                _targetScale = (_scaleSwitch[(int)state] - 1) * ScaleEffectAmplitude + 1;

                switch (DisplayType)
                {
                    case Type.ColorSwitch:
                        _startColor = ColorSwitch[(int)_startState];
                        _targetColor = ColorSwitch[(int)state];
                        break;

                    case Type.TextureSwitch:
                        _startTexture = TextureSwitch[(int)_startState];
                        _targetTexture = TextureSwitch[(int)state];
                        break;

                    case Type.AnimatedSwitch:
                        break;
                }
            }

            public void SetTransitionType(Transition type)
            {
                if (type != Transition.Custom)
                {
                    TransitionType = type;
                }

                switch (type)
                {
                    case Transition.Switch:
                        _transition = o => (float)Math.Ceiling(o);
                        return;

                    case Transition.LinearFade:
                        _transition = o => o;
                        return;

                    case Transition.DecelleratingFade:
                        _transition = MathZ.SineD;
                        return;

                    case Transition.AcceleratingFade:
                        _transition = MathZ.SineA;
                        return;

                    case Transition.EaseOutBack:
                        _transition = Easing.EaseOutBack;
                        return;

                    case Transition.EaseOutElastic:
                        _transition = Easing.EaseOutElastic;
                        return;

                    case Transition.EaseOutCubic:
                        _transition = Easing.EaseOutCubic;
                        return;
                }
            }

            public void AddText(string text, float fontSize, bool centered, Color baseColor, SpriteFont font)
            {
                Vector2 measure = font.MeasureString(text);

                Text = new Renderer.Text(
                    new Layer(Layer.main, Layer.layer + 1), font, text, fontSize, 0,
                    centered ? new Vector2((Transform.Left + Transform.Right) * 0.5f, (Transform.Top + Transform.Bottom) * 0.5f) : new Vector2(Transform.Left + 8, (Transform.Top + Transform.Bottom) * 0.5f),
                    centered ? new Vector2(0.5f, 0.5f) * measure : new Vector2(0, 0.5f) * measure,
                    baseColor);

                _textBaseColor = baseColor;
            }

            public void ChangeText(string text, float? fontSize = null, Color? color = null, SpriteFont font = null)
            {
                Vector2 measure = (font ?? Text.Font).MeasureString(text);

                Text.String = new StringBuilder(text);
                Text.Font = font ?? Text.Font;
                Text.Color = color ?? Text.Color;
                Text.Scale = fontSize == null ? Text.Scale : Vector2.One * fontSize.Value;
                Text.Origin = Vector2.One * 0.5f * measure;
            }

            public void AddEffect(SoundEffect effect) => this._effect = effect;

            public static Color[] DefaultColors()
                => new Color[] { Color.White, new Color(1.15f, 1.15f, 1.15f), new Color(0.85f, 0.85f, 0.85f) };

            public static Color[] PseudoDefaultColors(Color origin)
                => new Color[] { origin, origin * 1.15f, origin * 0.85f };

            public void SetPseudoDefaultColors(Color origin)
                => ColorSwitch = PseudoDefaultColors(origin);

            public void SetDefaultColors()
                => ColorSwitch = DefaultColors();

            public void SetTransitionExplicit(Func<float, float> function)
                => _transition = function;

            public void SetTextureSwitch(Texture2D idle, Texture2D hover, Texture2D click)
                => TextureSwitch = new Texture2D[] { idle, hover, click };

            public void SetColorSwitch(Color idle, Color hover, Color click)
                => ColorSwitch = new Color[] { idle, hover, click };

            public void SetColorSwitch(Color[] colors)
                => ColorSwitch = colors.Length == 3 ? colors : ColorSwitch;

            struct TAS
            {
                public Texture2D texture;
                public float alpha;

                public TAS(Texture2D texture, float alpha)
                {
                    this.texture = texture;
                    this.alpha = alpha;
                }
            }
        }
    }
}
