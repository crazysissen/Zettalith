using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    class SettingsMenu
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

            Color buttonColor = new Color(220, 220, 220, 255), textColor = new Color(0, 160, 255, 255);

            title = new Renderer.Text(new Layer(MainLayer.GUI, 0), Font.Styled, "Zettalith", 10, 0, new Vector2(0, -200));

            bQuit = new GUI.Button(new Layer(MainLayer.GUI, 0), new Rectangle(1, 1, 1, 1), buttonColor) { ScaleEffect = true };
            bQuit.AddText("Quit", 4, false, textColor, Font.Default);

            main.Add(title, bHost, bJoin, bSettings, bArmies, bQuit);
        }

        public void Update()
        {

        }
    }
}
