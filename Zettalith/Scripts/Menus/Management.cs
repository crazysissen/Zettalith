using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    class Management
    {
        MainController controller;

        GameSetup setup;

        GUI.Collection collection, main;
        Renderer.Text managementText;
        GUI.Button bRed, bBlue, bGreen;
        Renderer.SpriteScreen grey;
        Texture2D greySeeThrough2D, bMana2D;
        Layer managementLayer, managementBackgroundLayer;

        public void Initialize(MainController controller)
        {
            this.controller = controller;

            int buttonHeight = Settings.GetResolution.Y / 36, buttonSpace = Settings.GetResolution.Y / 48;
            int tempButtonWidth = 5 * Settings.GetResolution.X / 48;

            collection = new GUI.Collection();
            main = new GUI.Collection() { Origin = new Point((int)(Settings.GetHalfResolution.X - tempButtonWidth * 0.5), (int)(Settings.GetResolution.Y * 0.36)) };

            managementLayer = new Layer(MainLayer.GUI, 5);
            managementBackgroundLayer = new Layer(MainLayer.GUI, 4);

            collection.Add(main);

            greySeeThrough2D = Load.Get<Texture2D>("GreySeeThrough");
            bMana2D = Load.Get<Texture2D>("ManagementButton");

            RendererController.GUI.Add(collection);

            Color red = new Color(226, 28, 28, 255), blue = new Color(28, 28, 226, 255), green = new Color(28, 226, 28, 255), textColor = new Color(0, 160, 255, 255);

            managementText = new Renderer.Text(managementLayer, Font.Styled, "Management", 7, 0, new Vector2((float)(Settings.GetResolution.X * -0.1), (float)(Settings.GetResolution.Y * -0.18)));

            bRed = new GUI.Button(managementLayer, new Rectangle(-tempButtonWidth, 2 * (buttonHeight + buttonSpace), tempButtonWidth, tempButtonWidth), bMana2D, red) { ScaleEffect = true };
            bRed.OnClick += BRed;

            bBlue = new GUI.Button(managementLayer, new Rectangle(0, 2 * (buttonHeight + buttonSpace), tempButtonWidth, tempButtonWidth), bMana2D, blue) { ScaleEffect = true };
            bBlue.OnClick += BBlue;

            bGreen = new GUI.Button(managementLayer, new Rectangle(tempButtonWidth, 2 * (buttonHeight + buttonSpace), tempButtonWidth, tempButtonWidth), bMana2D, green) { ScaleEffect = true };
            bGreen.OnClick += BGreen;

            grey = new Renderer.SpriteScreen(managementBackgroundLayer, greySeeThrough2D, new Rectangle(0, 0, Settings.GetResolution.X, Settings.GetResolution.Y));

            main.Add(managementText, bRed, bBlue, bGreen);
        }

        public void Update()
        {

        }

        public void BRed()
        {
            //Red++;
        }

        private void BBlue()
        {
            //Blue++;
        }

        public void BGreen()
        {
            //Green++;
        }
    }
}
