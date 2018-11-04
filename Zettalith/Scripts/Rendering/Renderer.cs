using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    abstract class Renderer
    {
        public abstract void Draw(SpriteBatch spriteBatch, Camera camera, float deltaTime);

        public abstract Layer Layer { get; set; }

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
            /// <summary>
            /// The texture of the object
            /// </summary>
            public virtual Texture2D Texture { get; set; }

            /// <summary>
            /// The x & y coordinates of the object in world space
            /// </summary>
            public virtual Vector2 Position { get; set; }

            /// <summary>
            /// The width/height of the object
            /// </summary>
            public virtual Vector2 Size { get; set; }

            /// <summary>
            /// The rotation angle of the object measured in degrees (0-360)
            /// </summary>
            public virtual float Rotation { get; set; }

            /// <summary>
            /// The point on the object around which it rotates
            /// </summary>
            public virtual Vector2 RotationOrigin { get; set; }

            /// <summary>
            /// The color multiplier of the object
            /// </summary>
            public virtual Color Color { get; set; }

            /// <summary>
            /// Wether or not the sprite is flipped somehow, stack using binary OR operator (|)
            /// </summary>
            public virtual SpriteEffects Effects { get; set; }

            public override Layer Layer { get; set; }

            public Sprite(Texture2D texture, Vector2 position, Vector2 size, Color color, float rotation, Vector2 rotationOrigin, SpriteEffects effects)
            {
                Texture = texture;
                Position = position;
                Size = size;
                Rotation = rotation;
                RotationOrigin = rotationOrigin;
                Color = color;
                Effects = effects;
            }

            public override void Draw(SpriteBatch spriteBatch, Camera camera, float deltaTime)
            {
                spriteBatch.Draw(Texture, camera.WorldToScreenPosition(Position), null, Color, Rotation, RotationOrigin, Size, Effects, Layer.LayerDepth);
            }
        }

        //public class SpriteAuto : Sprite
        //{
        //    Func<Texture2D> getTexture;
        //    Func<Vector2> getPosition, getSize;
        //    Func<Color> getColor;

        //    public override Texture2D Texture => getTexture.Invoke();
        //    public override Vector2 Position => getPosition.Invoke();
        //    public override Vector2 Size => getSize.Invoke();
        //    public override Color Color => getColor.Invoke();
        //    public override SpriteEffects Effects => SpriteEffects.None;

        //    public SpriteAuto()
        //    {

        //    }
        //}

        public class SpriteScreen : Renderer
        {
            /// <summary>
            /// The texture of the object
            /// </summary>
            public virtual Texture2D Texture { get; set; }

            /// <summary>
            /// The x & y coordinates of the object in world space
            /// </summary>
            public virtual Rectangle Transform { get; set; }

            /// <summary>
            /// The rotation angle of the object measured in degrees (0-360)
            /// </summary>
            public virtual float Rotation { get; set; }

            /// <summary>
            /// The point on the object around which it rotates
            /// </summary>
            public virtual Vector2 RotationOrigin { get; set; }

            /// <summary>
            /// The color multiplier of the object
            /// </summary>
            public virtual Color Color { get; set; }

            /// <summary>
            /// Wether or not the sprite is flipped somehow, stack using binary OR operator (|)
            /// </summary>
            public virtual SpriteEffects Effects { get; set; }

            public override Layer Layer { get; set; }

            public SpriteScreen(Texture2D texture, Rectangle transform) : this(texture, transform, Color.White, 0, Vector2.Zero, SpriteEffects.None) { }

            public SpriteScreen(Texture2D texture, Rectangle transform, Color color) : this(texture, transform, color, 0, Vector2.Zero, SpriteEffects.None) { }

            public SpriteScreen(Texture2D texture, Rectangle transform, Color color, float rotation, Vector2 rotationOrigin, SpriteEffects effects)
            {
                Texture = texture;
                Transform = transform;
                Rotation = rotation;
                RotationOrigin = rotationOrigin;
                Color = color;
                Effects = effects;
            }

            public override void Draw(SpriteBatch spriteBatch, Camera camera, float deltaTime)
            {
                spriteBatch.Draw(Texture, Transform, null, Color, Rotation, RotationOrigin, Effects, Layer.LayerDepth);
            }
        }

        public class Text : Renderer
        {
            public SpriteFont Font { get; set; }
            public StringBuilder String { get; set; }
            public Vector2 Position { get; set; }
            public Vector2 Scale { get; set; }
            public Vector2 Origin { get; set; }
            public Color Color { get; set; }
            public float Rotation { get; set; }
            public SpriteEffects SpriteEffects { get; set; }

            public override Layer Layer { get; set; }

            public override void Draw(SpriteBatch spriteBatch, Camera camera, float deltaTime)
            {
                spriteBatch.DrawString(Font, String, Position, Color, Rotation, Origin, Scale, SpriteEffects, Layer.LayerDepth);
            }
        }

        public class Custom : Renderer
        {
            public DrawCommand Command { get; private set; }

            public override Layer Layer { get; set; }

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

        public class Animator : Renderer
        {
            public override Layer Layer { get; set; }

            public override void Draw(SpriteBatch spriteBatch, Camera camera, float deltaTime)
            {
                
            }
        }

        public delegate void DrawCommand(SpriteBatch spriteBatch, Camera camera, float deltaTime, float managedLayer);
    }
}
