using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Zettalith
{
    class MainMenu
    {
        MainController controller;

        GameSetup setup;

        GUI.Collection collection, main;
        //Renderer.Text title;
        Renderer.AnimatorScreen background;
        GUI.Button bHost, bJoin, bArmies, bSettings, bQuit;

        SoundEffect song;

        public void Initialize(MainController controller)
        {
            this.controller = controller;

            int buttonHeight = Settings.GetResolution.Y / 20, buttonSpace = Settings.GetResolution.Y / 48;
            int tempButtonWidth = 5 * Settings.GetResolution.X / 48;

            if (!XNAController.LocalGameClient)
            {
                song = Load.Get<SoundEffect>("Druids");
                Sound.PlaySong(song);
            }

            collection = new GUI.Collection();
            main = new GUI.Collection()
            {
                Origin = new Point((int)(Settings.GetHalfResolution.X - tempButtonWidth * 0.5 ), (int)(Settings.GetResolution.Y * 0.36))
            };

            Texture2D menuTexture = Load.Get<Texture2D>("Menu3");
            background = new Renderer.AnimatorScreen(new Layer(MainLayer.GUI, -1), menuTexture, new Point(480, 270), new Rectangle(Point.Zero, Settings.GetResolution), Vector2.Zero, 0, Color.White, 0.05f, 0, true, SpriteEffects.None);

            collection.Add(background, main);

            RendererController.GUI.Add(collection);

            Color buttonColor = new Color(240, 240, 240, 255), textColor = new Color(255, 255, 255, 255);
            Texture2D buttonTexture = Load.Get<Texture2D>("Button1");

            float textSize = 4 * (Settings.GetResolution.Y / 1080f);

            //title = new Renderer.Text(new Layer(MainLayer.GUI, 0), Font.Styled, "Zettalith", textSize * 2.5f, 0, new Vector2((float)(Settings.GetResolution.X * -0.09), (float)(Settings.GetResolution.Y * -0.18)));

            bHost = new GUI.Button(new Layer(MainLayer.GUI, 0), new Rectangle(0, 0, tempButtonWidth, buttonHeight), buttonTexture, buttonColor) { ScaleEffect = true };
            bHost.AddText("Host", textSize, true, textColor, Font.Default);
            bHost.OnClick += BHost;

            bJoin = new GUI.Button(new Layer(MainLayer.GUI, 0), new Rectangle(0, buttonHeight + buttonSpace, tempButtonWidth, buttonHeight), buttonTexture, buttonColor) /*{ ScaleEffect = true }*/;
            bJoin.AddText("Join", textSize, true, textColor, Font.Default);
            bJoin.OnClick += BJoin;

            bArmies = new GUI.Button(new Layer(MainLayer.GUI, 0), new Rectangle(0, 2 * (buttonHeight + buttonSpace), tempButtonWidth, buttonHeight), buttonTexture, buttonColor) { ScaleEffect = true };
            bArmies.AddText("Collection", textSize, true, textColor, Font.Default);
            bArmies.OnClick += BArmies;

            bSettings = new GUI.Button(new Layer(MainLayer.GUI, 0), new Rectangle(0, 3 * (buttonHeight + buttonSpace), tempButtonWidth, buttonHeight), buttonTexture, buttonColor) { ScaleEffect = true };
            bSettings.AddText("Settings", textSize, true, textColor, Font.Default);
            bSettings.OnClick += BSettings;

            bQuit = new GUI.Button(new Layer(MainLayer.GUI, 0), new Rectangle(0, 4 * (buttonHeight + buttonSpace), tempButtonWidth, buttonHeight), buttonTexture, buttonColor) { ScaleEffect = true };
            bQuit.AddText("Quit", textSize, true, textColor, Font.Default);
            bQuit.OnClick += BQuit;

            main.Add(/*title, */bHost, bJoin, bSettings, bArmies, bQuit);
        }

        public void Update()
        {

        }

        public void StopMusic()
        {
            song?.Dispose();
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

            controller.ToLobby(null);
        }

        private void BArmies()
        {
            controller.ToArmies();
        }

        private void BSettings()
        {
            Action GoBackToMain = controller.ToMenu;

            controller.ToSettings(GoBackToMain, new Layer(MainLayer.GUI, 1));
        }

        private void BQuit()
        {
            XNAController.Quit();
        }
    }
}
