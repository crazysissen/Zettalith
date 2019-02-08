using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    class MainMenu
    {
        MainController controller;

        GameSetup setup;

        GUI.Collection collection, main;
        Renderer.Text title;
        GUI.Button bHost, bJoin, bArmies, bSettings, bQuit;

        public void Initialize(MainController controller)
        {
            this.controller = controller;

            int buttonHeight = Settings.GetResolution.Y / 36, buttonSpace = Settings.GetResolution.Y / 48;
            int tempButtonWidth = (5 * Settings.GetResolution.X / 48);

            collection = new GUI.Collection();
            main = new GUI.Collection()
            {
                Origin = new Point((int)(Settings.GetHalfResolution.X - tempButtonWidth * 0.5 ), (int)(Settings.GetResolution.Y * 0.36))
            };

            collection.Add(main);

            RendererController.GUI.Add(collection);


            Color buttonColor = new Color(220, 220, 220, 255), textColor = new Color(0, 160, 255, 255);

            title = new Renderer.Text(new Layer(MainLayer.GUI, 0), Font.Styled, "Zettalith", 10, 0, new Vector2((float)(Settings.GetResolution.X * -0.09), (float)(Settings.GetResolution.Y * -0.18)));

            bHost = new GUI.Button(new Layer(MainLayer.GUI, 0), new Rectangle(0, 0, tempButtonWidth, buttonHeight), buttonColor) { ScaleEffect = true };
            bHost.AddText("Host", 4, false, textColor, Font.Default);
            bHost.OnClick += BHost;

            bJoin = new GUI.Button(new Layer(MainLayer.GUI, 0), new Rectangle(0, buttonHeight + buttonSpace, tempButtonWidth, buttonHeight), buttonColor) { ScaleEffect = true };
            bJoin.AddText("Join", 4, false, textColor, Font.Default);
            bJoin.OnClick += BJoin;

            bArmies = new GUI.Button(new Layer(MainLayer.GUI, 0), new Rectangle(0, 2 * (buttonHeight + buttonSpace), tempButtonWidth, buttonHeight), buttonColor) { ScaleEffect = true };
            bArmies.AddText("Collection", 4, false, textColor, Font.Default);
            bArmies.OnClick += BArmies;

            bSettings = new GUI.Button(new Layer(MainLayer.GUI, 0), new Rectangle(0, 3 * (buttonHeight + buttonSpace), tempButtonWidth, buttonHeight), buttonColor) { ScaleEffect = true };
            bSettings.AddText("Settings", 4, false, textColor, Font.Default);
            bSettings.OnClick += BSettings;

            bQuit = new GUI.Button(new Layer(MainLayer.GUI, 0), new Rectangle(0, 4 * (buttonHeight + buttonSpace), tempButtonWidth, buttonHeight), buttonColor) { ScaleEffect = true };
            bQuit.AddText("Quit", 4, false, textColor, Font.Default);
            bQuit.OnClick += BQuit;

            main.Add(title, bHost, bJoin, bSettings, bArmies, bQuit);
        }

        public void Update()
        {

        }

        public void CloseMenu()
        {
            collection.Active = false;
        }

        public void OpenMenu()
        {
            collection.Active = true;
        }

        private void BHost()
        {
            if (setup == null)
            {
                setup = new GameSetup();
                setup.Initialize(controller, MainController.InGame);

                collection.Add(setup.Collection);

                return;
            }

            if (setup.Collection.Active == true)
            {
                setup.Collection.Active = false;
                return;
            }

            setup.Collection.Active = true;
        }

        private void BJoin()
        {
            if (XNAController.localGame)
            {
                if (XNAController.LocalGameClient)
                {
                    controller.ToLobby(null);
                }

                return;
            }
        }

        private void BArmies()
        {
            controller.ToArmies();
        }

        private void BSettings()
        {
            Action GoBackToMain = controller.ToMenu;

            controller.ToSettings(GoBackToMain);
        }

        private void BQuit()
        {
            XNAController.Quit();
        }
    }
}
