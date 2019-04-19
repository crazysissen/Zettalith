using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Zettalith
{
    class EndHUD : HUD
    {
        GUI.Collection collection;

        GUI.Button bReturn;

        public EndHUD(GUI.Collection collection, InGameController igc, PlayerLocal p, ClientSideController csc) : base(igc, p, csc)
        {
            this.collection = collection;

            Vector2 res = Settings.GetResolution.ToVector2();

            bReturn = new GUI.Button(new Layer(MainLayer.GUI, 100), new Rectangle((int)(-res.X * 0.15f), (int)(-res.Y * 0.05f), (int)(res.X * 0.3f), (int)(res.Y * 0.1f))) { ScaleEffect = true };
            bReturn.AddText("Back to Menu", 3, true, Color.Black, Font.Bold);
            bReturn.OnClick += Return;

            collection.Add(bReturn);

            collection.Active = false;
        }

        public void Update(float deltaTime)
        {

        }

        public void Open()
        {
            collection.Active = true;
        }

        void Return()
        {

        }
    }
}
