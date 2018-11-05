using System;
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
        private GUI gui;

        // Separate testing window
        private Process clone;

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

        public void Initialize(XNAController game, StartType type, object[] args)
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
                clone = (System.Diagnostics.Process)args[0];
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
            RendererController.TestGUI.New();
            RendererController.TestGUI.Add(
                new TestGUI.Button(new Rectangle(10, 10, 100, 20), ContentController.Get<Texture2D>("Square"), Color.White, Color.Gray, Color.Green, TestHost),
                new TestGUI.Button(new Rectangle(10, 40, 100, 20), ContentController.Get<Texture2D>("Square"), Color.White, Color.Gray, Color.Green, BeginSearch),
                new TestGUI.Button(new Rectangle(10, 70, 100, 20), ContentController.Get<Texture2D>("Square"), Color.White, Color.Gray, Color.Green, TestJoin));

            //if (XNAController.LocalLocalGame)
            //    RendererController.TestGUI.Add(new TestGUI.Button(new Rectangle(120, 10, 100, 20), ContentController.Get<Texture2D>("Square"), Color.Green, Color.Gray, Color.Green, StartClone));

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

        void StartClone()
        {
            clone = new Process();

            clone.StartInfo.FileName = System.Reflection.Assembly.GetEntryAssembly().Location;
            clone.StartInfo.Arguments = XNAController.LOCALTEST;
            clone.StartInfo.UseShellExecute = false;
            clone.StartInfo.RedirectStandardInput = true;

            clone.Start();
        }
    }
}
