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

        GUI.Collection collection, mainCollection;
        Renderer.Text Header;
        GUI.Button bHost, bJoin, bArmies, bSettings, bQuit;

        Action GoBack;

        public void Initialize(MainController controller, Action goBack)
        {
            GoBack = goBack;

            this.controller = controller;

            collection = new GUI.Collection();
            mainCollection = new GUI.Collection()
            {
                Origin = new Point(40, 240)
            };

            collection.Add(mainCollection);

            RendererController.GUI.Add(collection);

            Color buttonColor = new Color(220, 220, 220, 255), textColor = new Color(0, 160, 255, 255);

            Header = new Renderer.Text(new Layer(MainLayer.GUI, 0), Font.Styled, "Settings", 10, 0, new Vector2(0, -200));

            bQuit = new GUI.Button(new Layer(MainLayer.GUI, 0), new Rectangle(1, 1, 1, 1), buttonColor) { ScaleEffect = true };
            bQuit.AddText("Quit", 4, false, textColor, Font.Default);

            mainCollection.Add(Header, bHost, bJoin, bSettings, bArmies, bQuit);
        }

        public void Update()
        {

        }

        public void BGoBack()
        {
            GoBack.Invoke();
        }
    }
}
