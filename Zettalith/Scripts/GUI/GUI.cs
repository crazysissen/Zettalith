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
        const Layer.Main
            LAYER = Layer.Main.GUI;

        public void Initialize()
        {
            Members = new List<IGUIMember>();
        }

        public void Draw(SpriteBatch spriteBatch, MouseState mouse, KeyboardState keyboard, float scaledDeltaTime, float unscaledDeltaTime)
        {
            DrawContainer(this, spriteBatch, mouse, keyboard, scaledDeltaTime, unscaledDeltaTime);
        }

        public static void DrawContainer(GUIContainer container, SpriteBatch spriteBatch, MouseState mouse, KeyboardState keyboard, float scaledDeltaTime, float unscaledDeltaTime)
        {
            foreach(IGUIMember member in container.Members)
            {
                if (member is GUIContainer)
                {
                    DrawContainer((member as GUIContainer), spriteBatch, mouse, keyboard, scaledDeltaTime, unscaledDeltaTime);
                }

                member.Draw(spriteBatch, mouse, keyboard, scaledDeltaTime, unscaledDeltaTime);
            }
        }
    }

    public abstract class GUIContainer
    {
        public virtual void Add(IGUIMember member) => Members.Add(member);

        public virtual List<IGUIMember> Members { get; protected set; }

        public static GUIContainer operator +(GUIContainer container, IGUIMember member)
        {
            container.Members.Add(member);
            return container;
        }

        public static GUIContainer operator +(GUIContainer container, IGUIMember[] members)
        {
            container.Members.AddRange(members);
            return container;
        }
    }

    public interface IGUIMember
    {
        void Draw(SpriteBatch spriteBatch, MouseState mouse, KeyboardState keyboard, float scaledDeltaTime, float unscaledDeltaTime);
    }
}
