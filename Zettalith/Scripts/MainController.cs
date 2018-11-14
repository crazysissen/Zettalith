using System;
using System.IO;
using System.IO.Pipes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            LOCALHOST = "localhost";

        public static InGameController InGame { get; private set; }

        private System.Net.IPEndPoint endPoint;
        private InGameController inGameController;
        private Random r;
        private Renderer renderer;

        private Renderer.SpriteScreen image;

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
            Test.Category = type.ToString();

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

            renderer = new Renderer.AnimatorScreen(ContentController.Get<Texture2D>("Animation Test"), new Point(16, 16), new Rectangle(200, 200, 32, 32), Vector2.Zero, 0, Color.White, 1, 0, true, SpriteEffects.None);

            GUI.Mask maskedContainer = new GUI.Mask();
            maskedContainer.Mask = new Mask(ContentController.Get<Texture2D>("TestMask"), new Rectangle(100, 100, 300, 300), false);

            RendererController.GUI.Add(maskedContainer);

            //GUIContainer maskedContainer = new GUI.Collection();

            image = new Renderer.SpriteScreen(ContentController.Get<Texture2D>("Animation Test"), new Rectangle(100, 100, 300, 300));
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
            using (AnonymousPipeServerStream host = new AnonymousPipeServerStream(PipeDirection.In))
            {
                debugConsole = new Process();

                debugConsole.StartInfo.Arguments = host.GetClientHandleAsString();
                debugConsole.StartInfo.UseShellExecute = false;
                debugConsole.Start();

                host.DisposeLocalCopyOfClientHandle();

                try
                {
                    using (StreamReader sr = new StreamReader(host))
                    {

                    }
                }
                catch (IOException exception)
                {

                }
            }
        }

        void StartClone()
        {
            clone = new Process();

            string args = string.Join(" ",
                XNAController.LOCALTEST,
                XNAController.PARENT + Process.GetCurrentProcess().Id);

            Test.Log(args);

            clone.StartInfo.FileName = System.Reflection.Assembly.GetEntryAssembly().Location;
            clone.StartInfo.UseShellExecute = false;
            clone.StartInfo.RedirectStandardInput = true;
            clone.StartInfo.Arguments = args;

            clone.Start();


        }
    }
}
