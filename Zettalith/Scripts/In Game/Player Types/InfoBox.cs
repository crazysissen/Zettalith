using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    class InfoBox
    {
        const float
            DISTANCE = 0.08f,
            SCREENSIZE = 0.25f;

        GUI.Collection collection;

        Renderer.SpriteScreen background;
        Renderer.SpriteScreenFloating top, middle, bottom;
        Renderer.Text topTitle, topDescription, topAbilityCost, topManaCost,
            middleTitle, middleManaCost;
            


        public InfoBox()
        {
            Point size = (Settings.GetResolution.ToVector2() * SCREENSIZE).RoundToPoint(), halfSize = new Point(size.X / 2, size.Y / 2);

            collection = new GUI.Collection() { Origin = Settings.GetHalfResolution - halfSize };

            background = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, -1), Load.Get<Texture2D>("InfoOverlay"), new Rectangle(Point.Zero, size));



            collection.Add(background);
        }

        public void Set(InGamePiece piece)
        {
            
        }

        public void Open()
        {

        }

        public void Close()
        {

        }
    }
}
