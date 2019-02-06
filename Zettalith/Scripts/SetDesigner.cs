using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    class SetDesigner
    {
        MainController controller;

        GUI.Collection collection;

        Renderer.Text title;
        GUI.Button bHost, bJoin, bArmies, bSettings, bQuit, bBlah;

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

            bBlah = new GUI.Button(new Rectangle(0, 5 * buttonHeight + 5 * buttonSpace, tempButtonWidth, buttonHeight));
            bBlah.OnClick += BBlah;

            collection.Add(bHost, bJoin, bSettings, bArmies, bQuit, bBlah);
        }

        public void Update(float deltaTime)
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
            
        }

        private void BSettings()
        {

        }

        private void BQuit()
        {
            XNAController.Quit();
        }

        private void BBlah()
        {

        }
    }
}
