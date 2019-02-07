using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    class LoadGame
    {
        InGameController controller;
        StartupConfig config;

        Thread loadThread;

        Renderer.AnimatorScreen loading;
        Texture2D animation;

        volatile LoadedConfig loadedConfig;
        volatile bool complete, host;

        public void Initialize(StartupConfig config, InGameController controller, bool host)
        {
            this.config = config;
            this.controller = controller;

            loadedConfig = new LoadedConfig();

            loadThread = new Thread(Setup);
            loadThread.Start();

            animation = Load.Get<Texture2D>("LoadingScreen");
            loading = new Renderer.AnimatorScreen(Layer.Default, animation, new Point(128, 72), new Rectangle(Point.Zero, Settings.Resolution), Vector2.Zero, 0, Color.White, 0.05f, 0, true, SpriteEffects.None);
        }

        public void Update(float deltaTime)
        {
            if (complete)
            {

            }
        }

        private void Setup()
        {
            Grid grid = new Grid(config.mapDiameter.X, config.mapDiameter.Y);

            complete = true;
        }

        // Output type
    }

    class LoadedConfig
    {
        public Grid grid;
    }
}
