using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    class GameSetup
    {
        const int
            WINDOWWIDTH = 400,
            WINDOWHEIGHT = 500,
            WINDOWMARGIN = 20;

        readonly int[]
            presetSizes = { 8, 12, 16, 20 };

        public GUI.Collection Collection { get; private set; }

        private MainController controller;
        private InGameController inGameController;

        private StartupConfig config;

        private ParameterizedAction<int>[] actions;

        private GUI.Collection content;
        private Renderer.SpriteScreen background;
        private Renderer.Text title;
        private GUI.Button[] bSizes;
        private GUI.Button bConfirm;

        public void Initialize(MainController controller, InGameController inGameController)
        {
            this.controller = controller;
            this.inGameController = inGameController;

            config = new StartupConfig()
            {
                mapDiameter = new Point(presetSizes[0], presetSizes[0])
            };

            Collection = new GUI.Collection()
            {
                Origin = Settings.GetHalfResolution - new Point(WINDOWWIDTH / 2, WINDOWHEIGHT / 2)
            };

            background = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 9), Load.Get<Texture2D>("Square"), new Rectangle(0, 0, WINDOWHEIGHT, WINDOWHEIGHT), Color.DarkGray);

            content = new GUI.Collection()
            {
                Origin = new Point(WINDOWMARGIN, WINDOWMARGIN)
            };

            int buttonHeight = 60, buttonDistance = 20, sizeButtonWidth = 100;

            title = new Renderer.Text(new Layer(MainLayer.GUI, 11), Font.Bold, "Host Game", 6, 0, Vector2.Zero);

            actions = new ParameterizedAction<int>[presetSizes.Length];
            bSizes = new GUI.Button[presetSizes.Length];
            for (int i = 0; i < bSizes.Length; ++i)
            {
                actions[i] = new ParameterizedAction<int>(MapSize, presetSizes[i]);

                bSizes[i] = new GUI.Button(new Layer(MainLayer.GUI, 11), new Rectangle(i * (sizeButtonWidth + 10), buttonHeight + buttonDistance, sizeButtonWidth, buttonHeight));
                bSizes[i].AddText(presetSizes[i].ToString(), 3, true, Color.Black, Font.Default);
            }

            bConfirm = new GUI.Button(new Layer(MainLayer.GUI, 11), new Rectangle(0, WINDOWHEIGHT - WINDOWMARGIN * 2 - buttonHeight, 200, buttonHeight), Color.LightYellow);
            bConfirm.AddText("Confirm", 4, false, Color.Black, Font.Bold);
            bConfirm.OnClick += Confirm;

            Collection.Add(content, background);
            content.Add(bSizes);
            content.Add(title, bConfirm);
        }

        public void Update(float deltaTime)
        {

        }

        private void MapSize(int size)
        {
            config.mapDiameter = new Point(size, size);
        }

        private void Confirm()
        {
            Collection.Active = false;

            controller.ToLobby(config);
        }
    }

    [Serializable]
    struct StartupConfig
    {
        public Point mapDiameter;
    }
}
