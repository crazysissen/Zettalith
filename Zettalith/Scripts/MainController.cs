using System;
using System.IO;
using System.IO.Pipes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Zettalith.Pieces;

namespace Zettalith
{
    class MainController
    {
        const string
            TESTIP = "10.156.46.30",
            LOCALHOST = "localhost",
            CONSOLEPATH = @"\Debug\ZettalithDebugConsole.exe";

        public static InGameController InGame { get; private set; }
        public static MainController Main { get; private set; }

        public static GameState CurrentState => Main.stateManager.GameState;
        public static int CurrentSubState => Main.stateManager.Peek;

        private Random r;
        private StateManager stateManager;

        // Separate testing window
        private Process clone/*, debugConsole*/;

        // Update Fork
        private InGameController inGameController;
        private MainMenu mainMenu;
        private SetDesigner setDesigner;
        private Lobby lobby;

        public MainController()
        {
            Main = this;
            r = new Random();
        }

        public void NormalInitialize()
        {

        }

        public void LocalGameHostInitialize()
        {

        }

        public void LocalGameClientInitialize(Process parentProcess)
        {
            
        }

        public void Initialize(XNAController game, StartType type, Color backgroundColor, Process parent = null)
        {
            //if (XNAController.DebugConsole)
            //{
            //    StartDebugConsole();
            //}

            SaveLoad.Initialize();

            stateManager = new StateManager(GameState.MainMenu, 0);

            RendererController.Initialize(XNAController.Graphics, new Vector2(0, 0), 1, backgroundColor);
            NetworkManager.Initialize(game);

            if (type == StartType.LocalHost)
            {
                StartClone();
                //NetworkManager.CreateLocalGame();
            }

            if (type == StartType.LocalClient)
            {
                clone = parent;
                //NetworkManager.CreateClient();
                //NetworkManager.StartPeerSearch(LOCALHOST);
            }

            CreateMainMenu();

            //SAVE LOAD TESTING, TOM LISTA AV PERSONALDATA SPARAS OCH LADDAS

            //SaveLoad.Save(PersonalData.UserData);

            //PersonalData.UserData = SaveLoad.Load();
        }

        public void LateInitialize(XNAController game)
        {
        }

        public void Update(XNAController game, GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (XNAController.localGame && clone.HasExited)
            {
                game.Exit();
            }

            NetworkManager.Update();

            switch (CurrentState)
            {
                case GameState.Splash:
                    stateManager.SetGameState(GameState.MainMenu, 0);
                    break;

                case GameState.MainMenu:
                    mainMenu.Update();
                    break;

                case GameState.ArmyDesigner:
                    setDesigner.Update(deltaTime);
                    break;

                case GameState.Lobby:
                    break;

                case GameState.GameLoad:
                    break;

                case GameState.InGame:
                    break;

                default:
                    break;
            }

            // Last
            In.UpdateMethods();
        }

        public void Draw(XNAController game, GameTime gameTime, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            RendererController.Render(graphics, spriteBatch, gameTime, (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public void OnExit()
        {
            //if (debugConsole != null && !debugConsole.HasExited)
            //{
            //    debugConsole.Kill();
            //}
        }

        void CloseClone()
        {
            clone.Close();
        }

        public void ToArmies()
        {
            stateManager.SetGameState(GameState.ArmyDesigner, 0);

            mainMenu.CloseMenu();

            setDesigner = new SetDesigner();
            setDesigner.Initialize(this);
        }

        public void ToLobby(StartupConfig? config = null)
        {
            stateManager.SetGameState(GameState.Lobby, 0);

            mainMenu.CloseMenu();

            lobby = new Lobby();
            lobby.Initialize("Player", config);
        }

        public void ToMenu()
        {
            stateManager.SetGameState(GameState.MainMenu, 0);

            mainMenu = new MainMenu();
            mainMenu.Initialize(this);
        }

        //void StartDebugConsole()
        //{
        //    if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + CONSOLEPATH))
        //    {
        //        Test.Log("No debug console found.");
        //        return;
        //    }

        //    //using (AnonymousPipeServerStream host = new AnonymousPipeServerStream(PipeDirection.In, HandleInheritability.Inheritable))
        //    //{

        //    debugConsole = new Process();

        //    //string clientHandle = host.GetClientHandleAsString();
        //    //Test.Log("Client handle: " + clientHandle);
        //    //Test.Log("Debugged handle: " + (XNAController.A_SERVERHANDLE + ":" + clientHandle).Split(':')[1]);

        //    //debugConsole.StartInfo.Arguments = clientHandle;
        //    //debugConsole.StartInfo.UseShellExecute = false;
        //    //debugConsole.StartInfo.RedirectStandardOutput = true;

        //    debugConsole.StartInfo.Arguments = Process.GetCurrentProcess().Id.ToString();
        //    debugConsole.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + CONSOLEPATH;
        //    debugConsole.StartInfo.RedirectStandardOutput = true;
        //    debugConsole.StartInfo.UseShellExecute = false;
        //    debugConsole.Start();

        //    readConsoleThread = new Thread(ReadConsole);
        //    readConsoleThread.Start(debugConsole);

        //        //try
        //        //{
        //        //    using (StreamReader sr = new StreamReader(host))
        //        //    {
        //        //        string readData = sr.ReadLine();
        //        //    }
        //        //}
        //        //catch (IOException exception)
        //        //{

        //        //}
        //    //}
        //}

        //void ReadConsole(object process)
        //{
        //    Process targetProcess = (Process)process;

        //    //Thread.Sleep(2000);

        //    //System.Diagnostics.Stopwatch

        //    while (true)
        //    {
        //        string read = targetProcess.StandardOutput.ReadLine();
        //    }
        //}

        void CreateMainMenu()
        {
            mainMenu = new MainMenu();
            mainMenu.Initialize(this);
        }

        void StartClone()
        {
            clone = new Process();

            string args = string.Join(" ",
                XNAController.A_LOCALTEST,
                XNAController.A_PARENT + ":" + Process.GetCurrentProcess().Id,
                XNAController.DebugConsole ? XNAController.A_DEBUG : "");

            Test.Log(args);

            clone.StartInfo.FileName = System.Reflection.Assembly.GetEntryAssembly().Location;
            clone.StartInfo.UseShellExecute = false;
            clone.StartInfo.RedirectStandardInput = true;
            clone.StartInfo.Arguments = args;

            clone.Start();
        }
    }
}
