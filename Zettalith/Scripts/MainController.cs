using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Zettalith
{
    class MainController
    {
        const string benneIP = "10.156.46.30";

        System.Net.IPEndPoint endPoint;

        public static InGameController InGame { get; private set; }

        public MainController()
        {
            
        }

        public void Initialize(XNAController game)
        {
            RendererController.Initialize(XNAController.Graphics, new Vector2(0, 0), 1);
            NetworkManager.Initialize(game);
            RendererController.TestGUI = new TestGUI();
        }

        public void LateInitialize(XNAController game)
        {

        }

        public void Update(XNAController game, GameTime gameTime)
        {
            NetworkManager.Update();
            RendererController.TestGUI.New();
            RendererController.TestGUI.Add(
                new TestGUI.Button(new Rectangle(10, 10, 100, 20), ContentController.Get<Texture2D>("Square"), Color.White, Color.Gray, Color.Green, TestHost),
                new TestGUI.Button(new Rectangle(10, 40, 100, 20), ContentController.Get<Texture2D>("Square"), Color.White, Color.Gray, Color.Green, BeginSearch),
                new TestGUI.Button(new Rectangle(10, 70, 100, 20), ContentController.Get<Texture2D>("Square"), Color.White, Color.Gray, Color.Green, TestJoin)
                );

            // Last
            DirectInput.UpdateMethods();
        }

        public void Draw(XNAController game, GameTime gameTime, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            RendererController.Render(graphics, spriteBatch, gameTime);
        }

        public void PeerFound(System.Net.IPEndPoint ipEndPoint, bool host, string message)
        {
            System.Diagnostics.Debug.WriteLine(!host ? "Server found: " + message + ". " : "Peer found. " + "IP: " + ipEndPoint + ". Local peer is host: " + host);

            endPoint = ipEndPoint;
        }

        void TestHost()
        {
            NetworkManager.CreateHost("server.exe");
        }

        void BeginSearch()
        {
            System.Diagnostics.Debug.WriteLine("Hello");
            NetworkManager.CreateClient();
            NetworkManager.StartPeerSearch(benneIP);
        }

        void TestJoin()
        {
            System.Diagnostics.Debug.WriteLine("Attempting join: " + endPoint);

            NetworkManager.TryJoin(endPoint.Address.ToString(), endPoint.Port, "JoinTest!", TestCallback);
        }

        void TestCallback(bool success)
        {

        }

        void TestSend()
        {

        }
    }
}
