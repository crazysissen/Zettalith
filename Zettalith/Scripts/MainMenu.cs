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

        GUI.Collection collection;

        Renderer.Text title;
        GUI.Button bHost, bJoin, bArmies, bSettings, bQuit;

        public void Initialize(MainController controller)
        {
            this.controller = controller;

            collection = new GUI.Collection();
            collection.Origin = new Point(40, 240);

            RendererController.GUI.Add(collection);

            int buttonHeight = 30, buttonSpace = 15;
            int tempButtonWidth = 100;

            bHost = new GUI.Button(new Rectangle(0, 0, tempButtonWidth, buttonHeight));
            bHost.OnClick += BHost;

            bJoin = new GUI.Button(new Rectangle(0, buttonHeight + buttonSpace, tempButtonWidth, buttonHeight));
            bJoin.OnClick += BJoin;

            bArmies = new GUI.Button(new Rectangle(0, 2 * buttonHeight + 2 * buttonSpace, tempButtonWidth, buttonHeight));
            bArmies.OnClick += BArmies;

            bSettings = new GUI.Button(new Rectangle(0, 3 * buttonHeight + 3 * buttonSpace, tempButtonWidth, buttonHeight));
            bSettings.OnClick += BSettings;

            bQuit = new GUI.Button(new Rectangle(0, 4 * buttonHeight + 4 * buttonSpace, tempButtonWidth, buttonHeight));
            bQuit.OnClick += BQuit;

            collection.Add(bHost, bJoin, bSettings, bArmies, bQuit);
        }

        public void Update()
        {

        }

        private void BHost()
        {

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
