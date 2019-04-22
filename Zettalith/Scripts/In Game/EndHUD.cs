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
        GUI.Collection Collection;

        GUI.Button bQuit;

        public EndHUD(GUI.Collection collection, InGameController igc, PlayerLocal p, ClientSideController csc) : base(igc, p, csc)
        {
            Collection = collection;

            Vector2 res = Settings.GetResolution.ToVector2();

            Texture2D quitTexture = Load.Get<Texture2D>("Button1");
            bQuit = new GUI.Button(new Layer(MainLayer.GUI, 15), new Rectangle((int)(res.X * -0.1f), (int)(res.Y * 0.05f), (int)(Ztuff.SizeResFactor * quitTexture.Width * 5), (int)(Ztuff.SizeResFactor * quitTexture.Height * 5)), quitTexture) { ScaleEffect = true };
            bQuit.AddText("Quit Game", 4, true, Color.White, Font.Bold);
            bQuit.OnClick += QuitGame;

            Collection.Add(bQuit);

            Collection.Active = false;
        }

        public void Update(float deltaTime)
        {

        }

        public void Open()
        {
            Collection.Active = true;
        }

        void QuitGame()
        {
            XNAController.Quit();
        }
    }
}
