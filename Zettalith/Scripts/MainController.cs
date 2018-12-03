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

namespace Zettalith
{
    class MainController
    {
        const string
            TESTIP = "10.156.46.30",
            LOCALHOST = "localhost",
            CONSOLEPATH = @"\Debug\ZettalithDebugConsole.exe";


        public static InGameController InGame { get; private set; }

        private System.Net.IPEndPoint endPoint;
        private InGameController inGameController;
        private Random r;
        private Renderer renderer;
        private Renderer.SpriteScreen image;
        private Thread readConsoleThread;

        // Separate testing window
        private Process clone, debugConsole;

        public MainController()
        {
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

            RendererController.Initialize(XNAController.Graphics, new Vector2(0, 0), 1);
            NetworkManager.Initialize(game);
            NetworkManager.Listen("TEST", RecieveTestMessage);
            RendererController.TestGUI = new TestGUI();

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

            renderer = new Renderer.AnimatorScreen((MainLayer.Main, 0), ContentController.Get<Texture2D>("Animation Test"), new Point(16, 16), new Rectangle(200, 200, 32, 32), Vector2.Zero, 0, Color.White, 1, 0, true, SpriteEffects.None);

            GUI.MaskedCollection maskedContainer = new GUI.MaskedCollection();
            maskedContainer.Mask = new Mask(ContentController.Get<Texture2D>("TestMask"), new Rectangle(100, 100, 300, 300), false);

            RendererController.GUI.Add(maskedContainer);

            //GUIContainer maskedContainer = new GUI.Collection();

            image = new Renderer.SpriteScreen((MainLayer.AbsoluteBottom, 0), ContentController.Get<Texture2D>("Animation Test"), new Rectangle(100, 100, 300, 300));
            maskedContainer.Add(image);
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
            RendererController.TestGUI.New();
            RendererController.TestGUI.Add(
                new TestGUI.Button(new Rectangle(10, 10, 100, 20), ContentController.Get<Texture2D>("Square"), Color.White, Color.Gray, Color.Green, TestHost),
                new TestGUI.Button(new Rectangle(10, 40, 100, 20), ContentController.Get<Texture2D>("Square"), Color.White, Color.Gray, Color.Green, BeginSearch),
                new TestGUI.Button(new Rectangle(10, 70, 100, 20), ContentController.Get<Texture2D>("Square"), Color.White, Color.Gray, Color.Green, TestJoin));

            //if (XNAController.LocalLocalGame)
            //    RendererController.TestGUI.Add(new TestGUI.Button(new Rectangle(120, 10, 100, 20), ContentController.Get<Texture2D>("Square"), Color.Green, Color.Gray, Color.Green, StartClone));

            image.Transform = new Rectangle(In.MousePosition, image.Transform.Size);

            // Last
            In.UpdateMethods();
        }

        public void Draw(XNAController game, GameTime gameTime, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            RendererController.Render(graphics, spriteBatch, gameTime);
        }

        public void OnExit()
        {
            if (debugConsole != null && !debugConsole.HasExited)
            {
                debugConsole.Kill();
            }
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

        void StartDebugConsole()
        {
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + CONSOLEPATH))
            {
                Test.Log("No debug console found.");
                return;
            }

            //using (AnonymousPipeServerStream host = new AnonymousPipeServerStream(PipeDirection.In, HandleInheritability.Inheritable))
            //{

            debugConsole = new Process();

            //string clientHandle = host.GetClientHandleAsString();
            //Test.Log("Client handle: " + clientHandle);
            //Test.Log("Debugged handle: " + (XNAController.A_SERVERHANDLE + ":" + clientHandle).Split(':')[1]);

            //debugConsole.StartInfo.Arguments = clientHandle;
            //debugConsole.StartInfo.UseShellExecute = false;
            //debugConsole.StartInfo.RedirectStandardOutput = true;

            debugConsole.StartInfo.Arguments = Process.GetCurrentProcess().Id.ToString();
            debugConsole.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + CONSOLEPATH;
            debugConsole.StartInfo.RedirectStandardOutput = true;
            debugConsole.StartInfo.UseShellExecute = false;
            debugConsole.Start();

            readConsoleThread = new Thread(ReadConsole);
            readConsoleThread.Start(debugConsole);

                //try
                //{
                //    using (StreamReader sr = new StreamReader(host))
                //    {
                //        string readData = sr.ReadLine();
                //    }
                //}
                //catch (IOException exception)
                //{

                //}
            //}
        }

        void ReadConsole(object process)
        {
            Process targetProcess = (Process)process;

            //Thread.Sleep(2000);

            //System.Diagnostics.Stopwatch

            while (true)
            {
                string read = targetProcess.StandardOutput.ReadLine();
            }
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
