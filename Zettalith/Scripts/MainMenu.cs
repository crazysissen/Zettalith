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

            title = new Renderer.Text(new Layer(MainLayer.GUI, 0), Font.Styled, "Zettalith", 10, 0, new Vector2(0, -200));

            bHost = new GUI.Button(new Layer(MainLayer.GUI, 0), new Rectangle(0, 0, tempButtonWidth, buttonHeight));
            bHost.AddText("Host", 4, false, Color.Black, Font.Default);
            bHost.OnClick += BHost;

            bJoin = new GUI.Button(new Layer(MainLayer.GUI, 0), new Rectangle(0, buttonHeight + buttonSpace, tempButtonWidth, buttonHeight));
            bJoin.AddText("Join", 4, false, Color.Black, Font.Default);
            bJoin.OnClick += BJoin;

            bArmies = new GUI.Button(new Layer(MainLayer.GUI, 0), new Rectangle(0, 2 * buttonHeight + 2 * buttonSpace, tempButtonWidth, buttonHeight));
            bArmies.AddText("Collection", 4, false, Color.Black, Font.Default);
            bArmies.OnClick += BArmies;

            bSettings = new GUI.Button(new Layer(MainLayer.GUI, 0), new Rectangle(0, 3 * buttonHeight + 3 * buttonSpace, tempButtonWidth, buttonHeight));
            bSettings.AddText("Settings", 4, false, Color.Black, Font.Default);
            bSettings.OnClick += BSettings;

            bQuit = new GUI.Button(new Layer(MainLayer.GUI, 0), new Rectangle(0, 4 * buttonHeight + 4 * buttonSpace, tempButtonWidth, buttonHeight));
            bQuit.AddText("Quit", 4, false, Color.Black, Font.Default);
            bQuit.OnClick += BQuit;

            main.Add(title, bHost, bJoin, bSettings, bArmies, bQuit);
        }

        public void Update()
        {

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
            collection.Active = false;

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
