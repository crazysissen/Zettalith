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

        public static IGUIMember[] GetMembers(GUIContainer container, MouseState mouse, KeyboardState keyboard, float deltaTime, Point additiveOrigin, bool drawThis = false)
        {
            // Recursive method to retrieve all 

            List<IGUIMember> newMembers = new List<IGUIMember>();

            foreach (IGUIMember member in container.Members)
            {
                if (member is GUIContainer)
                {
                    if (!(member as GUIContainer).Active)
                    {
                        continue;
                    }

                    if (member is GUIContainerMasked)
                    {
                        RendererController.TemporaryAddMask(member as GUIContainerMasked, additiveOrigin);

                        continue;
                    }

                    newMembers.AddRange(GetMembers((member as GUIContainer), mouse, keyboard, deltaTime, additiveOrigin + (member as GUIContainer).Origin));
                }

                if (member != null)
                {
                    member.Origin = additiveOrigin;
                    newMembers.Add(member);
                }

            }

            return newMembers.ToArray();
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
        public bool Active = true;

        ~GUIContainer()
        {
            foreach (IGUIMember member in Members)
            {
                //member.
            }
        }

        public virtual void Add(params IGUIMember[] members)
        {
            foreach (IGUIMember member in members)
            {
                Members.Add(member);

                if (member is Renderer)
                {
                    (member as Renderer).AutomaticDraw = false;
                }

                if (member is GUI.Button && (member as GUI.Button).Text != null)
                {
                    Add((member as GUI.Button).Text);
                }
            }
        }

        public virtual void Remove(IGUIMember member)
        {
            if (Members.Contains(member))
            {
                Members.Remove(member);
                Members.TrimExcess();
            }
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
        Layer Layer { get; }

        void Draw(SpriteBatch spriteBatch, MouseState mouse, KeyboardState keyboard, float deltaTime);

        Point Origin { get; set; }
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
