using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Scripts.Menus
{
    class PauseMenu
    {
        MainController controller;

        GameSetup setup;

        GUI.Collection collection, main;
        Renderer.Text pauseText;
        GUI.Button bResume, bSettings, bResign, bQuit;
        Renderer.SpriteScreen grey;
        Texture2D greySeeThrough;

        public void Initialize(MainController controller)
        {
            this.controller = controller;

            int buttonHeight = Settings.GetResolution.Y / 36, buttonSpace = Settings.GetResolution.Y / 48;
            int tempButtonWidth = 5 * Settings.GetResolution.X / 48;

            collection = new GUI.Collection();
            main = new GUI.Collection() { Origin = new Point((int)(Settings.GetHalfResolution.X - tempButtonWidth * 0.5), (int)(Settings.GetResolution.Y * 0.36)) };

            collection.Add(main);

            greySeeThrough = Load.Get<Texture2D>("GreySeeThrough");

            RendererController.GUI.Add(collection);

            Color buttonColor = new Color(220, 220, 220, 255), textColor = new Color(0, 160, 255, 255);

            pauseText = new Renderer.Text(new Layer(MainLayer.GUI, 1), Font.Styled, "Paused", 10, 0, new Vector2((float)(Settings.GetResolution.X * -0.09), (float)(Settings.GetResolution.Y * -0.18)));

            bResume = new GUI.Button(new Layer(MainLayer.GUI, 1), new Rectangle(0, 0, tempButtonWidth, buttonHeight), buttonColor) { ScaleEffect = true };
            bResume.AddText("Resume", 4, false, textColor, Font.Default);
            bResume.OnClick += BResume;

            bSettings = new GUI.Button(new Layer(MainLayer.GUI, 1), new Rectangle(0, buttonHeight + buttonSpace, tempButtonWidth, buttonHeight), buttonColor);
            bSettings.AddText("Settings", 4, false, textColor, Font.Default);
            bSettings.OnClick += BSettings;

            bResign = new GUI.Button(new Layer(MainLayer.GUI, 1), new Rectangle(0, 2 * (buttonHeight + buttonSpace), tempButtonWidth, buttonHeight), buttonColor) { ScaleEffect = true };
            bResign.AddText("Resign", 4, false, textColor, Font.Default);
            bResign.OnClick += BResign;

            bQuit = new GUI.Button(new Layer(MainLayer.GUI, 1), new Rectangle(0, 4 * (buttonHeight + buttonSpace), tempButtonWidth, buttonHeight), buttonColor) { ScaleEffect = true };
            bQuit.AddText("Quit", 4, false, textColor, Font.Default);
            bQuit.OnClick += BQuit;

            grey = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 0), greySeeThrough, new Rectangle(0, 0, Settings.GetResolution.X, Settings.GetResolution.Y));

            main.Add(pauseText, bResume, bSettings, bResign, bQuit);
        }

        public void Update()
        {

        }

        public void BResume()
        {
            collection.Active = false;
        }

        private void BSettings()
        {
            Action GoBackToPause = null;
            controller.ToSettings(GoBackToPause);
        }

        public void BResign()
        {
            //TODO Resign
        }

        private void BQuit()
        {
            //TODO Quit
        }
    }
}
