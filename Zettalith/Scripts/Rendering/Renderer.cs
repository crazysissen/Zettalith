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
    abstract class Renderer
    {
        const float
            DEGTORAD = (2 * (float)Math.PI) / 360,
            FONTSIZEMULTIPLIER = 1.0f / 4;

        /// <summary>Whether or not the object should be drawn automatically</summary>
        public virtual bool Active { get; set; } = true;
        public virtual bool AutomaticDraw { get; set; } = true;

        public abstract void Draw(SpriteBatch spriteBatch, Camera camera, float deltaTime);

        /// <summary>A Layer class to represent what depth it should be drawn at</summary>
        public virtual Layer Layer { get; set; }

        public Renderer()
        {
            RendererController.AddRenderer(this);
        }

        public void Destroy()
        {
            RendererController.RemoveRenderer(this);
        }

        public class Sprite : Renderer
        {
            /// <summary>The texture of the object</summary>
            public virtual Texture2D Texture { get; set; }

            /// <summary>The x & y coordinates of the object in world space</summary>
            public virtual Vector2 Position { get; set; }

            /// <summary>The width/height of the object</summary>
            public virtual Vector2 Size { get; set; }

            /// <summary>The rotation angle of the object measured in degrees (0-360)</summary>
            public virtual float Rotation { get; set; }

            /// <summary>A vector between (0,0) and (1,1) to represent the pivot around which the sprite is rotated
            /// and what point will line up to the Vector2 position</summary>
            public virtual Vector2 Origin { get; set; }

            /// <summary>The color multiplier of the object</summary>
            public virtual Color Color { get; set; }

            /// <summary>Wether or not the sprite is flipped somehow, stack using binary OR operator (|)</summary>
            public virtual SpriteEffects Effects { get; set; }

            public Sprite(Layer layer, Texture2D texture, Vector2 position, Vector2 size, Color color, float rotation, Vector2 rotationOrigin, SpriteEffects effects)
            {
                Layer = layer;
                Texture = texture;
                Position = position;
                Size = size;
                Rotation = rotation;
                Origin = rotationOrigin;
                Color = color;
                Effects = effects;
            }

            public override void Draw(SpriteBatch spriteBatch, Camera camera, float deltaTime)
            {
                spriteBatch.Draw(Texture, camera.WorldToScreenPosition(Position), null, Color, Rotation * DEGTORAD, Origin, camera.WorldToScreenSize(Size), Effects, Layer.LayerDepth);
            }
        }

        public class SpriteScreen : RendererIGUI
        {
            /// <summary>The texture of the object</summary>
            public virtual Texture2D Texture { get; set; }

            /// <summary>The x & y coordinates of the object in world spacesummary>
            public virtual Rectangle Transform { get; set; }

            /// <summary>The rotation angle of the object measured in degrees (0-360)</summary>
            public virtual float Rotation { get; set; }

            /// <summary>A vector between (0,0) and (1,1) to represent the pivot around which the object rotates 
            /// and what point will line up to the Vector2 position</summary>
            public virtual Vector2 Origin { get; set; }

            /// <summary>The color multiplier of the object</summary>
            public virtual Color Color { get; set; }

            /// <summary> Wether or not the sprite is flipped somehow, stack using binary OR operator (|)</summary>
            public virtual SpriteEffects Effects { get; set; }

            public SpriteScreen(Layer layer, Texture2D texture, Rectangle transform) : this(layer, texture, transform, Color.White, 0, Vector2.Zero, SpriteEffects.None) { }

            public SpriteScreen(Layer layer, Texture2D texture, Rectangle transform, Color color) : this(layer, texture, transform, color, 0, Vector2.Zero, SpriteEffects.None) { }

            public SpriteScreen(Layer layer, Texture2D texture, Rectangle transform, Color color, float rotation, Vector2 rotationOrigin, SpriteEffects effects)
            {
                Layer = layer;
                Texture = texture;
                Transform = transform;
                Rotation = rotation;
                Origin = rotationOrigin;
                Color = color;
                Effects = effects;
            }

            public override void Draw(SpriteBatch spriteBatch, Camera camera, float deltaTime)
            {
                spriteBatch.Draw(Texture, new Rectangle(Transform.Location + Offset, Transform.Size), null, Color, Rotation * DEGTORAD, Origin, Effects, Layer.LayerDepth);
            }
        }

        public class Animator : Renderer
        {
            /// <summary>The texture of the object</summary>
            public virtual Texture2D Texture { get; set; }

            /// <summary>The x & y coordinates of the object in world space</summary>
            public virtual Vector2 Position { get; set; }

            /// <summary>The x & y size multiplier of the object</summary>
            public virtual Vector2 Size { get; set; }

            /// <summary>The rotation angle of the object measured in degrees (0-360)</summary>
            public virtual float Rotation { get; set; }

            /// <summary>A vector between (0,0) and (1,1) to represent the pivot around which the object rotates 
            /// and what point will line up to the Vector2 position</summary>
            public virtual Vector2 Origin { get; set; }

            /// <summary>The color multiplier of the object</summary>
            public virtual Color Color { get; set; }

            /// <summary>Wether or not the sprite is flipped somehow, stack using binary OR operator (|)</summary>
            public virtual SpriteEffects Effects { get; set; }

            public Point FrameDimensions { get; private set; }
            public Point CountDimensions { get; private set; }
            public float Time { get; set; }
            public float TimeInterval { get; set; }
            public bool Repeat { get; set; }
            public int CurrentFrame => (int)(Time / TimeInterval);
            public int FrameCount { get; private set; }

            public Animator(Layer layer, Texture2D sheet, Point frameDimensions, Vector2 position, Vector2 size, Vector2 origin, float rotation, Color color, float interval, float startTime, bool repeat, SpriteEffects spriteEffects)
            {
                if (sheet.Width % frameDimensions.X != 0 || sheet.Height % frameDimensions.Y != 0)
                {
                    throw new Exception("Tried to create AnimatorScreen where the format image was not proportional to the frame size.");
                }

                Layer = layer;

                FrameDimensions = frameDimensions;
                Time = startTime;
                TimeInterval = interval;
                Repeat = repeat;
                CountDimensions = new Point(sheet.Width / frameDimensions.X, sheet.Height / frameDimensions.Y);
                FrameCount = CountDimensions.X * CountDimensions.Y;

                Texture = sheet;
                Position = position;
                Size = size;
                Rotation = rotation;
                Origin = origin;
                Color = color;
                Effects = spriteEffects;
            }

            public override void Draw(SpriteBatch spriteBatch, Camera camera, float deltaTime)
            {
                Time += deltaTime;
                if (Time > FrameCount * TimeInterval)
                {
                    Time %= (FrameCount * TimeInterval);
                }

                int currentFrame = CurrentFrame;
                Rectangle DestinationRectangle = new Rectangle()
                {
                    X = FrameDimensions.X * (currentFrame % CountDimensions.X),
                    Y = FrameDimensions.Y * (int)((float)currentFrame / CountDimensions.X),
                    Width = FrameDimensions.X,
                    Height = FrameDimensions.Y
                };

                spriteBatch.Draw(Texture, camera.WorldToScreenPosition(Position), DestinationRectangle, Color, Rotation, Origin, camera.WorldToScreenSize(Size), Effects, Layer.LayerDepth);
            }
        }

        public class AnimatorScreen : RendererIGUI
        {
            /// <summary>The texture of the object</summary>
            public virtual Texture2D Texture { get; set; }

            /// <summary>The x & y coordinates of the object in world space</summary>
            public virtual Rectangle Transform { get; set; }

            /// <summary>The rotation angle of the object measured in degrees (0-360)</summary>
            public virtual float Rotation { get; set; }

            /// <summary>A vector between (0,0) and (1,1) to represent the pivot around which the object rotates 
            /// and what point will line up to the Vector2 position</summary>
            public virtual Vector2 Origin { get; set; }

            /// <summary>The color multiplier of the object</summary>
            public virtual Color Color { get; set; }

            /// <summary>Wether or not the sprite is flipped somehow, stack using binary OR operator (|)</summary>
            public virtual SpriteEffects Effects { get; set; }

            public Point FrameDimensions { get; private set; }
            public Point CountDimensions { get; private set; }
            public float Time { get; set; }
            public float TimeInterval { get; set; }
            public bool Repeat { get; set; }
            public int CurrentFrame => (int)(Time / TimeInterval);
            public int FrameCount { get; private set; }

            public AnimatorScreen(Layer layer, Texture2D sheet, Point frameDimensions, Rectangle transform, Vector2 origin, float rotation, Color color, float interval, float startTime, bool repeat, SpriteEffects spriteEffects)
            {
                if (sheet.Width % frameDimensions.X != 0 || sheet.Height % frameDimensions.Y != 0)
                {
                    throw new Exception("Tried to create AnimatorScreen where the format image was not proportional to the frame size.");
                }

                Layer = layer;

                FrameDimensions = frameDimensions;
                Time = startTime;
                TimeInterval = interval;
                Repeat = repeat;
                CountDimensions = new Point(sheet.Width / frameDimensions.X, sheet.Height / frameDimensions.Y);
                FrameCount = CountDimensions.X * CountDimensions.Y;

                Texture = sheet;
                Transform = transform;
                Rotation = rotation;
                Origin = origin;
                Color = color;
                Effects = spriteEffects;
            }

            public override void Draw(SpriteBatch spriteBatch, Camera camera, float deltaTime)
            {
                Time += deltaTime;
                if (Time > FrameCount * TimeInterval)
                {
                    Time %= (FrameCount * TimeInterval);
                }

                int currentFrame = CurrentFrame;
                Rectangle DestinationRectangle = new Rectangle()
                {
                    X = FrameDimensions.X * (currentFrame % CountDimensions.X),
                    Y = FrameDimensions.Y * (int)((float)currentFrame / CountDimensions.X),
                    Width = FrameDimensions.X,
                    Height = FrameDimensions.Y
                };

                Rectangle transform = new Rectangle(Transform.Location + Offset, Transform.Size);

                spriteBatch.Draw(Texture, transform, DestinationRectangle, Color, Rotation, Origin, Effects, Layer.LayerDepth);
            }
        }

        public class Text : RendererIGUI
        {
            public SpriteFont Font { get; set; }
            /// <summary>A StringBuilder class to represent the text shown</summary>
            public StringBuilder String { get; set; }
            public Vector2 Position { get; set; }
            public Vector2 Scale { get; set; }
            /// <summary>A vector between (0,0) and (1,1) to represent the pivot around which the object rotates 
            /// and what point will line up to the Vector2 position</summary>
            public Vector2 Origin { get; set; }
            public Color Color { get; set; }
            public float Rotation { get; set; }
            public SpriteEffects SpriteEffects { get; set; }

            public Text(Layer layer, SpriteFont font, string text, float fontSize, float rotation, Vector2 position)
                : this(layer, font, new StringBuilder(text), Vector2.One * fontSize * FONTSIZEMULTIPLIER, rotation, position, new Vector2(0.5f, 0.5f), Color.White, SpriteEffects.None)
            { }
            public Text(Layer layer, SpriteFont font, StringBuilder text, float fontSize, float rotation, Vector2 position)
                : this(layer, font, text, Vector2.One * fontSize * FONTSIZEMULTIPLIER, rotation, position, new Vector2(0.5f, 0.5f), Color.White, SpriteEffects.None)
            { }
            public Text(Layer layer, SpriteFont font, string text, float fontSize, float rotation, Vector2 position, Vector2 origin)
                : this(layer, font, new StringBuilder(text), Vector2.One * fontSize * FONTSIZEMULTIPLIER, rotation, position, origin, Color.White, SpriteEffects.None)
            { }
            public Text(Layer layer, SpriteFont font, StringBuilder text, float fontSize, float rotation, Vector2 position, Vector2 origin)
                : this(layer, font, text, Vector2.One * fontSize * FONTSIZEMULTIPLIER, rotation, position, origin, Color.White, SpriteEffects.None)
            { }
            public Text(Layer layer, SpriteFont font, string text, float fontSize, float rotation, Vector2 position, Color color)
                : this(layer, font, new StringBuilder(text), Vector2.One * fontSize * FONTSIZEMULTIPLIER, rotation, position, new Vector2(0.5f, 0.5f), color, SpriteEffects.None)
            { }
            public Text(Layer layer, SpriteFont font, StringBuilder text, float fontSize, float rotation, Vector2 position, Color color)
                : this(layer, font, text, Vector2.One * fontSize * FONTSIZEMULTIPLIER, rotation, position, new Vector2(0.5f, 0.5f), color, SpriteEffects.None)
            { }
            public Text(Layer layer, SpriteFont font, string text, float fontSize, float rotation, Vector2 position, Vector2 origin, Color color)
                : this(layer, font, new StringBuilder(text), Vector2.One * fontSize * FONTSIZEMULTIPLIER, rotation, position, origin, color, SpriteEffects.None)
            { }
            public Text(Layer layer, SpriteFont font, StringBuilder text, float fontSize, float rotation, Vector2 position, Vector2 origin, Color color)
                : this(layer, font, text, Vector2.One * fontSize * FONTSIZEMULTIPLIER, rotation, position, origin, color, SpriteEffects.None)
            { }

            public Text(Layer layer, SpriteFont font, StringBuilder text, Vector2 scale, float rotation, Vector2 position, Vector2 origin, Color color, SpriteEffects spriteEffects)
            {
                Layer = layer;
                Font = font;
                String = text;
                Scale = scale;
                Rotation = rotation;
                Position = position;
                Origin = origin;
                Color = color;
                SpriteEffects = spriteEffects;
            }

            public override void Draw(SpriteBatch spriteBatch, Camera camera, float deltaTime)
            {
                spriteBatch.DrawString(Font, String, Position + Offset.ToVector2(), Color, Rotation * DEGTORAD, Origin, Scale, SpriteEffects, Layer.LayerDepth);
            }           
        }

        public class Custom : Renderer
        {
            public DrawCommand Command { get; private set; }

            public Custom(DrawCommand drawCommand, Layer layer)
            {
                Command = drawCommand;
                Layer = layer;
            }

            public override void Draw(SpriteBatch spriteBatch, Camera camera, float deltaTime)
            {
                Command.Invoke(spriteBatch, camera, deltaTime, Layer.LayerDepth);
            }

            public void SetCommand(DrawCommand drawCommand) => Command = drawCommand;
        }

        public delegate void DrawCommand(SpriteBatch spriteBatch, Camera camera, float deltaTime, float managedLayer);
    }

    abstract class RendererIGUI : Renderer, IGUIMember
    {
        public Point Offset { get; set; }
        Point IGUIMember.Origin { get => Offset; set => Offset = value; }

        void IGUIMember.Draw(SpriteBatch spriteBatch, MouseState mouse, KeyboardState keyboard, float unscaledDeltaTime)
        {
            Draw(spriteBatch, null, unscaledDeltaTime);
        }
    }
}
