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
        const string remoteTest = "10.156.46.56";

        public static InGameController InGame { get; private set; }

        public MainController()
        {
            
        }

        public void Initialize(XNAController game)
        {
            RendererController.Initialize(new Vector2(0, 0), 1);
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
                new TestGUI.Button(new Rectangle(10, 40, 100, 20), ContentController.Get<Texture2D>("Square"), Color.White, Color.Gray, Color.Green, TestJoin)
                );

            // Last
            DirectInput.UpdateMethods();
        }

        public void Draw(XNAController game, GameTime gameTime, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            RendererController.Render(graphics, spriteBatch, gameTime);
        }

        void TestHost()
        {
            NetworkManager.CreateHost("server.exe");
        }

        void TestJoin()
        {
            System.Diagnostics.Debug.WriteLine("Hello");
            NetworkManager.CreateClient();
            NetworkManager.StartPeerSearch(remoteTest);
        }
    }
}
