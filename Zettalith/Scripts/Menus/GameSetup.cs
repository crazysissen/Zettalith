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

            Point res = Settings.GetResolution;

            int
                windowWidth = (int)(res.X * 0.2f),
                windowHeight = (int)(res.Y * 0.4f),
                windowMargain = (int)(windowWidth * 0.05f),
                screenDistance = (int)(res.X * 0.02f),
                buttonHeight = (int)(windowHeight * 0.08f),
                contentWidth = windowWidth - windowMargain * 2; 

            config = new StartupConfig()
            {
                mapDiameter = new Point(presetSizes[0], presetSizes[0])
            };

            Collection = new GUI.Collection()
            {
                Origin = new Point(screenDistance, (res.Y - windowHeight) / 2)
            };

            background = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 9), Load.Get<Texture2D>("Square"), new Rectangle(0, 0, windowWidth, windowHeight), Color.DarkGray);

            content = new GUI.Collection()
            {
                Origin = new Point(windowMargain, windowMargain)
            };

            title = new Renderer.Text(new Layer(MainLayer.GUI, 11), Font.Bold, "Host Game", 6, 0, Vector2.Zero);

            float spaceProportion = 0.2f;

            int buttonWidth = (int)(contentWidth / ((presetSizes.Length - 1) * spaceProportion + presetSizes.Length)),
                spaceWidth = (int)(contentWidth / ((presetSizes.Length - 1) * (1 / spaceProportion) + presetSizes.Length));

            actions = new ParameterizedAction<int>[presetSizes.Length];
            bSizes = new GUI.Button[presetSizes.Length];
            for (int i = 0; i < bSizes.Length; ++i)
            {
                actions[i] = new ParameterizedAction<int>(MapSize, presetSizes[i]);

                bSizes[i] = new GUI.Button(new Layer(MainLayer.GUI, 11), new Rectangle(i * (buttonWidth + spaceWidth), windowHeight / 5, buttonWidth, buttonHeight));
                bSizes[i].AddText(presetSizes[i].ToString(), 3, true, Color.Black, Font.Default);
                bSizes[i].OnClick += actions[i].Activate;
            }


            bConfirm = new GUI.Button(new Layer(MainLayer.GUI, 11), new Rectangle(0, windowHeight - windowMargain * 2 - buttonHeight, contentWidth, windowHeight / 12), Color.LightYellow);
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

            config.type = MapGen.Type.SquareMap;

            controller.ToLobby(config);
        }
    }

    [Serializable]
    struct StartupConfig
    {
        public SPoint mapDiameter;
        public int seed;
        public MapGen.Type type;
    }
}
