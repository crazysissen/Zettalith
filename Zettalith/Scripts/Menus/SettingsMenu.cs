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

        GUI.Collection collection, mainCollection, fullscreenCollection, musicCollection;
        Renderer.Text Header, fullscreenText, resolutionText;
        GUI.Button bFullscreen;

        Action GoBack;

        Texture2D unchecked2D, checked2D;

        public void Initialize(MainController controller, Action goBack)
        {
            GoBack = goBack;

            this.controller = controller;

            collection = new GUI.Collection();
            mainCollection = new GUI.Collection() { Origin = new Point(0, 0) };
            fullscreenCollection = new GUI.Collection() { Origin = Settings.GetHalfResolution};

            collection.Add(mainCollection);

            checked2D = Load.Get<Texture2D>("CheckedBox");
            unchecked2D = Load.Get<Texture2D>("UncheckedBox");

            RendererController.GUI.Add(collection);

            Color buttonColor = new Color(220, 220, 220, 255), textColor = new Color(0, 160, 255, 255);

            //Header = new Renderer.Text(new Layer(MainLayer.GUI, 0), Font.Styled, "Settings", 10, 0, new Vector2(0, 0));

            fullscreenText = new Renderer.Text(new Layer(MainLayer.GUI, 0), Font.Default, "Fullscreen", 4, 0, new Vector2(0, 0), buttonColor);
            bFullscreen = new GUI.Button(new Layer(MainLayer.GUI, 0), new Rectangle((int)(Settings.GetResolution.X * 0.085), (int)(Settings.GetResolution.Y * 0.0126), (int)(Settings.GetResolution.X * 0.18 / 16), (int)(Settings.GetResolution.Y * 0.02)), PersonalData.Settings.FullScreen ? checked2D : unchecked2D);
            bFullscreen.OnClick += BFullscreen;



            fullscreenCollection.Add(fullscreenText, bFullscreen);
            mainCollection.Add(Header, fullscreenCollection);
        }

        public void Update()
        {

        }

        private void BFullscreen()
        {
            if (PersonalData.Settings.FullScreen == true)
            {
                PersonalData.Settings.FullScreen = false;
                bFullscreen.Texture = unchecked2D;
            }
            else
            {
                PersonalData.Settings.FullScreen = true;
                bFullscreen.Texture = checked2D;
            }
        }

        private void BGoBack()
        {
            GoBack.Invoke();
        }
    }
}
