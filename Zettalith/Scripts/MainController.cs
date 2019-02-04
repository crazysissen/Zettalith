﻿using System;
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

        private System.Net.IPEndPoint endPoint;
        private Random r;
        private StateManager stateManager;

        // Separate testing window
        private Process clone/*, debugConsole*/;

        // Update Fork
        private InGameController inGameController;
        private MainMenu mainMenu;
        private DeckDesigner deckDesigner;

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

        public void Initialize(XNAController game, StartType type, Process parent = null)
        {
            //if (XNAController.DebugConsole)
            //{
            //    StartDebugConsole();
            //}

            stateManager = new StateManager(GameState.MainMenu, 0);

            RendererController.Initialize(XNAController.Graphics, new Vector2(0, 0), 1);
            NetworkManager.Initialize(game);
            NetworkManager.Listen("TEST", RecieveTestMessage);

            if (type == StartType.LocalHost)
            {
                StartClone();
                NetworkManager.CreateLocalGame();
            }

            if (type == StartType.LocalClient)
            {
                clone = parent;
                NetworkManager.CreateClient();
                NetworkManager.StartPeerSearch(LOCALHOST);
            }
        }

        public void LateInitialize(XNAController game)
        {
        }

        public void Update(XNAController game, GameTime gameTime)
        {
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
                    break;

                case GameState.ArmyDesigner:
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

        public void PeerFound(System.Net.IPEndPoint ipEndPoint, bool host, string message)
        {
            Test.Log((!host ? "Server found: " + message + ". " : "Peer found. ") + "IP: " + ipEndPoint + ". Local peer is host: " + host);

            if (XNAController.LocalGameClient)
            {
                NetworkManager.TryJoin(ipEndPoint.Address.ToString(), ipEndPoint.Port, "Local Server", TestCallback);
            }

            endPoint = ipEndPoint;
        }

        public void Connected(System.Net.IPEndPoint ipEndPoint, bool host)
        {
            inGameController = new InGameController(host);
        }

        void TestMessage()
        {
            NetworkManager.Send("TEST", r.Next(0, 0xFFFF));
        }

        void TestHost()
        {
            NetworkManager.CreateHost("server.exe");
        }

        void BeginSearch()
        {
            NetworkManager.CreateClient();
            NetworkManager.StartPeerSearch(TESTIP);
        }

        void TestJoin()
        {
            Test.Log("Attempting join: " + endPoint);

            NetworkManager.TryJoin(endPoint.Address.ToString(), endPoint.Port, "JoinTest!", TestCallback);
        }

        void RecieveTestMessage(byte[] data)
        {
            int integer = data.ToObject<int>();

            Test.Log(integer.ToString("X4"));
        }

        void TestCallback(bool success)
        {

        }

        void CloseClone()
        {
            clone.Close();
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
