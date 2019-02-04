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
        GUI.Button bHost, bJoin, bSettings, bArmies, bQuit;

        public void Initialize(MainController controller)
        {
            this.controller = controller;

            collection = new GUI.Collection();

            bHost = new GUI.Button(new Rectangle(0, 0, 300, 80));
            bJoin = new GUI.Button(new Rectangle(0, 120, 300, 80));
            bSettings = new GUI.Button(new Rectangle(0, 240, 300, 80));
            bArmies = new GUI.Button(new Rectangle(0, 360, 300, 80));
            bQuit = new GUI.Button(new Rectangle(0, 480, 300, 80));
        }

        public void Update()
        {

        }
    }
}
