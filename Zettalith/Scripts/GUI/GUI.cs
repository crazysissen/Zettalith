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
        const MainLayer
            LAYER = MainLayer.GUI;

        public void Initialize()
        {
            Members = new List<IGUIMember>();
        }

        public void Draw(SpriteBatch spriteBatch, MouseState mouse, KeyboardState keyboard, float deltaTime)
        {
            DrawContainer(this, spriteBatch, mouse, keyboard, deltaTime, Point.Zero);
        }

        public static void DrawContainer(GUIContainer container, SpriteBatch spriteBatch, MouseState mouse, KeyboardState keyboard, float deltaTime, Point additiveOrigin)
        {
            foreach(IGUIMember member in container.Members)
            {
                if (member is GUIContainer)
                {
                    if (member is GUIContainerMasked)
                    {
                        RendererController.TemporaryAddMask(member as GUIContainerMasked, additiveOrigin);

                        continue;
                    }

                    DrawContainer((member as GUIContainer), spriteBatch, mouse, keyboard, deltaTime, additiveOrigin + (member as GUIContainer).Origin);
                }

                member.Draw(spriteBatch, mouse, keyboard, deltaTime);
            }
        }

        public static GUI operator +(GUI gui, IGUIMember member)
        {
            gui.Add(member);
            return gui;
        }

        public static GUI operator +(GUI gui, IGUIMember[] members)
        {
            gui.Add(members);
            return gui;
        }

        public static GUI operator -(GUI gui, IGUIMember member)
        {
            gui.Remove(member);
            return gui;
        }
    }

    public abstract class GUIContainer
    {
        public virtual void Add(params IGUIMember[] members)
        {
            foreach (IGUIMember member in members)
            {
                if (member is Renderer)
                {
                    (member as Renderer).Automatic = false;
                }

                Members.Add(member);
            }
        }

        public virtual void Remove(IGUIMember member)
        {
            Members.Remove(member);
        }

        public virtual Point Origin { get; set; }

        public virtual List<IGUIMember> Members { get; protected set; } = new List<IGUIMember>();

        public static GUIContainer operator +(GUIContainer container, IGUIMember member)
        {
            container.Add(member);
            return container;
        }

        public static GUIContainer operator +(GUIContainer container, IGUIMember[] members)
        {
            container.Add(members);
            return container;
        }

        public static GUIContainer operator -(GUIContainer container, IGUIMember member)
        {
            container.Remove(member);
            return container;
        }
    }

    public abstract class GUIContainerMasked : GUIContainer
    {
        public Mask Mask { get; set; }
    }

    public interface IGUIMember
    {
        void Draw(SpriteBatch spriteBatch, MouseState mouse, KeyboardState keyboard, float deltaTime);
    }

    public struct Mask
    {
        public Texture2D MaskTexture { get; set; }
        public Color Color { get; set; }
        public Rectangle Rectangle { get; set; }
        public bool RenderOutside { get; set; }

        public Mask(Texture2D mask, Rectangle rectangle, Color color, bool renderOutside)
        {
            MaskTexture = mask;
            Rectangle = rectangle;
            RenderOutside = renderOutside;
            Color = color;
        }

        public static implicit operator Mask((Texture2D texture, Rectangle rectangle, bool renderOutside) tuple) 
            => new Mask(tuple.texture, tuple.rectangle, Color.White, tuple.renderOutside);
    }
}
