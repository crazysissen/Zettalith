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

            collection = new GUI.Collection();
            main = new GUI.Collection()
            {
                Origin = new Point(40, 240)
            };

            collection.Add(main);

            RendererController.GUI.Add(collection);

            int buttonHeight = 30, buttonSpace = 15;
            int tempButtonWidth = 200;

            Color buttonColor = new Color(220, 220, 220, 255), textColor = new Color(0, 160, 255, 255);

            title = new Renderer.Text(new Layer(MainLayer.GUI, 0), Font.Styled, "Zettalith", 10, 0, new Vector2(0, -200));

            bHost = new GUI.Button(new Layer(MainLayer.GUI, 0), new Rectangle(0, 0, tempButtonWidth, buttonHeight), buttonColor);
            bHost.AddText("Host", 4, false, textColor, Font.Default);
            bHost.OnClick += BHost;

            bJoin = new GUI.Button(new Layer(MainLayer.GUI, 0), new Rectangle(0, buttonHeight + buttonSpace, tempButtonWidth, buttonHeight), buttonColor);
            bJoin.AddText("Join", 4, false, textColor, Font.Default);
            bJoin.OnClick += BJoin;

            bArmies = new GUI.Button(new Layer(MainLayer.GUI, 0), new Rectangle(0, 2 * buttonHeight + 2 * buttonSpace, tempButtonWidth, buttonHeight), buttonColor);
            bArmies.AddText("Collection", 4, false, textColor, Font.Default);
            bArmies.OnClick += BArmies;

            bSettings = new GUI.Button(new Layer(MainLayer.GUI, 0), new Rectangle(0, 3 * buttonHeight + 3 * buttonSpace, tempButtonWidth, buttonHeight), buttonColor);
            bSettings.AddText("Settings", 4, false, textColor, Font.Default);
            bSettings.OnClick += BSettings;

            bQuit = new GUI.Button(new Layer(MainLayer.GUI, 0), new Rectangle(0, 4 * buttonHeight + 4 * buttonSpace, tempButtonWidth, buttonHeight), buttonColor);
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

        }

        private void BArmies()
        {
            controller.ToArmies();
        }

        private void BSettings()
        {
            
        }

        private void BQuit()
        {
            XNAController.Quit();
        }
    }
}
