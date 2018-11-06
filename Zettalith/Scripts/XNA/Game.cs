using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Management;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Zettalith
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    class XNAController : Game
    {
        public const string
            PARENTQUERY = "SELECT * FROM Win32_Process WHERE ProcessId=",

            // CMD Args
            LOCALTEST = "-local",
            DEBUG = "-debug";

        // Boot config
        public static readonly bool localGame = true;

        public static GraphicsDeviceManager Graphics { get; private set; }
        public static SpriteBatch SpriteBatch { get; private set; }
        public static MainController MainController { get; private set; }

        public static bool LocalGameClient { get; private set; } = false;
        public static bool LocalGameHost { get; private set; } = false;

        string[] _commandLineArgs;

        Color background = new Color(20, 20, 60);

        public XNAController()
        {
            MainController = new MainController();

            Graphics = new GraphicsDeviceManager(this)
            {
                PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8
            };

            Content.RootDirectory = "Content";

            _commandLineArgs = System.Environment.GetCommandLineArgs();

            foreach (string arg in _commandLineArgs)
            {
                switch (arg)
                {
                    case LOCALTEST:
                        LocalGameClient = true;
                        break;

                    default:
                        break;
                }
            }
        }

        protected override void Initialize()
        {
            base.Initialize();

            StartType startType = StartType.Main;
            List<object> args = new List<object>();

            if (localGame)
            {
                // This is the host for a local game
                if (!LocalGameClient)
                {
                    LocalGameHost = true;
                    Window.Title = "";
                    startType = StartType.LocalHost;
                }

                // This is the client for a local game
                else
                {
                    startType = StartType.LocalClient;

                    background = new Color(60, 20, 20);

                    Process process = Process.GetCurrentProcess();
                    Process parent;

                    try
                    {
                        using (ManagementObjectSearcher query = new ManagementObjectSearcher(PARENTQUERY + process.Id))
                        {
                            IEnumerable<ManagementObject> obj = query.Get().OfType<ManagementObject>();

                            parent = obj.Select(p => Process.GetProcessById((int)(uint)p["ParentProcessId"])).First();

                            args.Add(parent);
                        }
                    }
                    catch
                    {
                        parent = null;
                        throw new Exception("Crash when getting parent process");
                    }

                    MainController.LocalGameClientInitialize(parent);
                }
            }

            MainController.Initialize(
                game: this, 
                type: startType,
                args: args.ToArray());

            IsMouseVisible = true;

            //System.Diagnostics.Debug.WriteLine();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            ContentController.Initialize(Content, true);

            MainController.LateInitialize(game: this);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            MainController.Update(
                game: this, 
                gameTime: gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(background);

            base.Draw(gameTime);

            MainController.Draw(
                game: this, 
                gameTime: gameTime, 
                graphics: Graphics, 
                spriteBatch: SpriteBatch);
        }
    }

    enum StartType { Main, LocalHost, LocalClient }
}
