using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    class ManagementHUD : HUD
    {
        GUI.Collection collection;
        Renderer.Text managementText;
        GUI.Button bRed, bBlue, bGreen;
        Renderer.SpriteScreen grey;

        ClientSideController theCSC;

        public ManagementHUD(GUI.Collection collection, InGameController igc, PlayerLocal p, Buff[] someBuffs, ClientSideController csc) : base(igc, p, csc)
        {
            this.collection = collection;
            theCSC = csc;
            
            Texture2D mana2D = Load.Get<Texture2D>("ManagementButton");

            grey = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 4), Load.Get<Texture2D>("GreySeeThrough"), new Rectangle(0, 0, Settings.GetResolution.X, Settings.GetResolution.Y));

            managementText = new Renderer.Text(new Layer(MainLayer.GUI, 5), Font.Styled, "Management", 6, 0, new Vector2());
            managementText.Position = new Vector2(Settings.GetResolution.X * 0.5f - managementText.Font.MeasureString(managementText.String).X * managementText.Scale.X * 0.5f, Settings.GetResolution.Y * 0.2f);

            bRed = new GUI.Button(new Layer(MainLayer.GUI, 5), new Rectangle((int)(Settings.GetResolution.X * 0.35f), (int)(Settings.GetResolution.Y * 0.5f), (int)(Ztuff.SizeResFactor * mana2D.Bounds.Width * 8), (int)(Ztuff.SizeResFactor * mana2D.Bounds.Height * 10)), mana2D, Color.Red) { ScaleEffect = true };
            bRed.OnClick += BRed;

            bGreen = new GUI.Button(new Layer(MainLayer.GUI, 5), new Rectangle((int)(Settings.GetResolution.X * 0.45f), (int)(Settings.GetResolution.Y * 0.5f), (int)(Ztuff.SizeResFactor * mana2D.Bounds.Width * 8), (int)(Ztuff.SizeResFactor * mana2D.Bounds.Height * 10)), mana2D, Color.Green) { ScaleEffect = true };
            bGreen.OnClick += BGreen;

            bBlue = new GUI.Button(new Layer(MainLayer.GUI, 5), new Rectangle((int)(Settings.GetResolution.X * 0.55f), (int)(Settings.GetResolution.Y * 0.5f), (int)(Ztuff.SizeResFactor * mana2D.Bounds.Width * 8), (int)(Ztuff.SizeResFactor * mana2D.Bounds.Height * 10)), mana2D, Color.Blue) { ScaleEffect = true };
            bBlue.OnClick += BBlue;

            collection.Add(grey, managementText, bRed, bBlue, bGreen);
            collection.Active = false;
        }

        public void Update(float deltaTime)
        {

        }

        public void BRed()
        {
            InGameController.LocalMana = new Mana(InGameController.LocalMana.Red + 1, InGameController.LocalMana.Blue, InGameController.LocalMana.Green);

            CloseManagement();
        }

        private void BBlue()
        {
            InGameController.LocalMana = new Mana(InGameController.LocalMana.Red, InGameController.LocalMana.Blue + 1, InGameController.LocalMana.Green);

            CloseManagement();
        }

        public void BGreen()
        {
            InGameController.LocalMana = new Mana(InGameController.LocalMana.Red, InGameController.LocalMana.Blue, InGameController.LocalMana.Green + 1);

            CloseManagement();
        }

        public void CloseManagement()
        {
            collection.Active = false;
            theCSC.ForceOpenLogistics();
        }
    }
}
