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
        private Renderer.Text sizeTitle;
        private GUI.Button[] bSizes;
        private GUI.Button bFlat, bNoise;

        public void Initialize(MainController controller, InGameController inGameController)
        {
            this.controller = controller;
            this.inGameController = inGameController;

            Point res = Settings.GetResolution;

            int
                windowWidth = (int)(res.X * 0.2f),
                windowHeight = (int)(res.Y * 0.4f),
                windowMargain = (int)(windowWidth * 0.09f),
                screenDistance = (int)(res.X * 0.02f),
                buttonHeight = (int)(windowHeight * 0.1f),
                contentWidth = windowWidth - windowMargain * 2;

            float textSize = (res.X / 1920f);

            config = new StartupConfig()
            {
                mapDiameter = new Point(presetSizes[0], presetSizes[0])
            };

            Collection = new GUI.Collection()
            {
                Origin = new Point(screenDistance, (res.Y - windowHeight) / 2)
            };

            background = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 9), Load.Get<Texture2D>("GameSetup"), new Rectangle(0, 0, windowWidth, windowHeight), Color.White);

            content = new GUI.Collection()
            {
                Origin = new Point(windowMargain, windowMargain)
            };

            sizeTitle = new Renderer.Text(new Layer(MainLayer.GUI, 11), Font.Bold, "Map Diameter: ", 3 * textSize, 0, new Vector2(0, windowHeight * 0.17f), Color.White);

            Texture2D buttonTexture = Load.Get<Texture2D>("Button3"), confirmTexture = Load.Get<Texture2D>("Button2");

            float spaceProportion = 0.2f;

            int buttonWidth = (int)(contentWidth / ((presetSizes.Length - 1) * spaceProportion + presetSizes.Length)/* * 0.9f*/),
                spaceWidth = (int)(contentWidth / ((presetSizes.Length - 1) * (1 / spaceProportion) + presetSizes.Length));

            actions = new ParameterizedAction<int>[presetSizes.Length];
            bSizes = new GUI.Button[presetSizes.Length];
            for (int i = 0; i < bSizes.Length; ++i)
            {
                actions[i] = new ParameterizedAction<int>(MapSize, presetSizes[i]);

                bSizes[i] = new GUI.Button(new Layer(MainLayer.GUI, 11), new Rectangle(i * (buttonWidth + spaceWidth), windowHeight / 4, buttonWidth, buttonHeight), buttonTexture);
                bSizes[i].AddText(presetSizes[i].ToString(), 4 * textSize, true, Color.White, Font.Default);
                bSizes[i].OnClick += actions[i].Activate;
            }

            bFlat = new GUI.Button(new Layer(MainLayer.GUI, 11), new Rectangle(0, windowHeight - windowMargain * 2 - windowHeight / 7, contentWidth, windowHeight / 8), confirmTexture, Color.LightYellow);
            bFlat.AddText("Start Flat Map", 4f * textSize, true, Color.Black, Font.Bold);
            bFlat.OnClick += Confirm;

            bNoise = new GUI.Button(new Layer(MainLayer.GUI, 11), new Rectangle(0, windowHeight - windowMargain * 2 - 2 * windowHeight / 7, contentWidth, windowHeight / 8), confirmTexture, Color.LightYellow);
            bNoise.AddText("Start Noise Map", 3.8f * textSize, true, Color.Black, Font.Bold);
            bNoise.OnClick += ConfirmNoise;

            Collection.Add(content, background);
            content.Add(bSizes);
            content.Add(sizeTitle, bFlat, bNoise);
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

        private void ConfirmNoise()
        {
            Collection.Active = false;

            config.type = MapGen.Type.NoiseMap;

            controller.ToLobby(config);
        }

        private void ConfirmMirror()
        {
            Collection.Active = false;

            config.type = MapGen.Type.NoiseMirrorMap;

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
