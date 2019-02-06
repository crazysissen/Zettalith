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
            A_LOCALTEST = "-local",
            A_DEBUG = "-debug",
            A_PARENT = "-parent",
            A_SERVERHANDLE = "-serverhandle";

        // Boot config
        public static readonly bool localGame = true;

        public static bool DebugConsole { get; private set; } = true;

        public static GraphicsDeviceManager Graphics { get; private set; }
        public static SpriteBatch SpriteBatch { get; private set; }
        public static MainController MainController { get; private set; }
        public static bool LocalGameClient { get; private set; } = false;
        public static bool LocalGameHost { get; private set; } = false;
        public static Dictionary<string, string> CommandLineArgs { get; private set; }

        private static XNAController _singleton;

        private string[] _commandLineArgs;
        private Color _background = new Color(20, 20, 60);

        public XNAController()
        {
            _singleton = this;

            Graphics = new GraphicsDeviceManager(this)
            {
                PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8,
                PreferredBackBufferHeight = 1080,
                PreferredBackBufferWidth = 1920
            };

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            SetupCommandLineArgs();
            SetupLocalMachineGame();
            WriteCommandLineArgs();

            MainController = new MainController();

            base.Initialize();

            StartType startType = StartType.Main;
            Process parent = null;

            if (localGame)
            {
                // This is the host for a local game
                if (!LocalGameClient)
                {
                    LocalGameHost = true;
                    startType = StartType.LocalHost;
                }

                // This is the client for a local game
                else
                {

                    startType = StartType.LocalClient;

                    _background = new Color(60, 20, 20);

                    Process process = Process.GetCurrentProcess();

                    if (!CommandLineArgs.ContainsKey("-parent"))
                    {
                        throw new Exception("Child window initialized without feeding parent process ID.");
                    }

                    parent = Process.GetProcessById(int.Parse(CommandLineArgs["-parent"]));

                    MainController.LocalGameClientInitialize(parent);
                }
            }

            MainController.Initialize(
                game: this,
                type: startType,
                backgroundColor: _background,
                parent: parent);

            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            Load.Initialize(Content, true);
            Font.Initialize();

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

            base.Draw(gameTime);

            MainController.Draw(
                game: this, 
                gameTime: gameTime, 
                graphics: Graphics, 
                spriteBatch: SpriteBatch);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            MainController.OnExit();
        }

        public static void Quit()
        {
            _singleton.Exit();
        }

        public static string GetWindowTitle() => _singleton.Window.Title;

        public static void SetWindowTitle(string title) => _singleton.Window.Title = title;

        private void SetupLocalMachineGame()
        {
            if (localGame)
            {
                if (LocalGameClient)
                {
                    Test.Category = "CLIENT";
                    Window.Title = "ZETTALITH: Local-Game Client";
                }
                else
                {
                    LocalGameHost = true;
                    Test.Category = "HOST";
                    Window.Title = "ZETTALITH: Local-Game Host";
                }
            }
            else
            {
                Test.Category = "ZETTALITH";
                Window.Title = "ZETTALITH";
            }
        }

        private void SetupCommandLineArgs()
        {
            _commandLineArgs = System.Environment.GetCommandLineArgs();
            CommandLineArgs = new Dictionary<string, string>();

            foreach (string arg in _commandLineArgs)
            {
                if (arg.Contains(":\\"))
                {
                    continue;
                }

                string key, value;

                try
                {
                    string[] args = arg.Split(':');

                    key = args[0];
                    value = args[1];
                }
                catch
                {
                    key = arg;
                    value = default(string);
                }

                CommandLineArgs.Add(key, value);
            }

            foreach (string arg in _commandLineArgs)
            {
                switch (arg)
                {
                    case A_LOCALTEST:
                        LocalGameClient = true;
                        break;

                    case A_DEBUG:
                        DebugConsole = true;
                        break;

                    default:
                        break;
                }
            }
        }

        private void WriteCommandLineArgs()
        {
            foreach (KeyValuePair<string, string> arg in CommandLineArgs)
            {
                if (arg.Value == default(string))
                {
                    Test.Log("Command Line Argument: {0}", arg.Key);
                    continue;
                }

                Test.Log("Command Line Argument: {0}:{1}", arg.Key, arg.Value);
            }
        }
    }

    enum StartType { Main, LocalHost, LocalClient }
}
