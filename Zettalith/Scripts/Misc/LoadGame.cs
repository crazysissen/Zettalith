using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zettalith.Pieces;

namespace Zettalith
{
    class LoadGame
    {
        const string
            PLAYERDATAHEADER = "GETPLAYERDATA";

        InGameController controller;
        StartupConfig config;
        PlayerSetupData playerData;

        Thread loadThread;

        Renderer.AnimatorScreen loading;
        Texture2D animation;

        volatile LoadedConfig loadedConfig;
        volatile bool complete, host;

        public void Initialize(StartupConfig config, InGameController controller, bool host)
        {
            this.config = config;
            this.controller = controller;
            this.host = host;

            loadedConfig = new LoadedConfig();

            NetworkManager.Listen(PLAYERDATAHEADER, GetPlayerData);

            loadThread = new Thread(Setup);
            loadThread.Start();

            animation = Load.Get<Texture2D>("LoadingScreen");
            loading = new Renderer.AnimatorScreen(Layer.Default, animation, new Point(128, 72), new Rectangle(Point.Zero, Settings.GetResolution), Vector2.Zero, 0, Color.White, 0.05f, 0, true, SpriteEffects.None);
        }

        public void Update(float deltaTime)
        {
            if (complete)
            {
                loading.Destroy();
                loading = null;

                controller.Initialize(loadedConfig);
            }
        }

        void GetPlayerData(byte[] data)
        {

        }

        private void Setup()
        {
            Thread.Sleep(2000);

            Random r = new Random(config.seed);

            Map map = MapGen.Generate(r, config.mapDiameter.X, config.mapDiameter.Y, config.type);

            int startPlayer = r.Next(2);

            // Wait for playerData
            while (PlayerSetupData.recievedData == null) { Thread.Sleep(5); }

            // Finalization

            loadedConfig = new LoadedConfig()
            {
                map = map,
                startPlayer = startPlayer
            };

            complete = true;
        }

        // Output type
    }

    class LoadedConfig
    {
        public Map map;
        public int startPlayer;
        public Set[] sets;
    }

    [Serializable]
    class PlayerSetupData
    {
        public Set set;

        public static PlayerSetupData recievedData;
        public static void RecieveData(byte[] data) => recievedData = data.ToObject<PlayerSetupData>();
    }
}
