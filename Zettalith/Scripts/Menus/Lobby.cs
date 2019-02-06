using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Zettalith
{
    class Lobby
    {
        private static Lobby singleton;
        private static Callback callback;

        private bool host;

        private System.Net.IPEndPoint endPoint;

        private GUI.Collection collection;
        private GUI.Button bStart, bBack;
        private Renderer.SpriteScreen localBackground, globalBackground, playerBackground, opponentBackground;
        private Renderer.Text title, localIP, globalIP, playerName, playerNameTitle, opponentName, opponentNameTitle;

        private StartupConfig? config;

        public void Initialize(string playerName, StartupConfig? config = null)
        {
            singleton = this;
            host = config != null;
            this.config = config;

            collection = new GUI.Collection()
            {
                Origin = new Microsoft.Xna.Framework.Point(40, 40)
            };


            title = new Renderer.Text(new Layer(MainLayer.GUI, 0), Font.Styled, "Lobby", 10, 0, Vector2.Zero);

            UpdateIPs();

            RendererController.GUI.Add(collection);
            collection.Add(title, localIP, globalIP);

            if (XNAController.LocalGameHost)
            {
                NetworkManager.CreateLocalGame();
            }

            if (XNAController.LocalGameClient)
            {
                NetworkManager.CreateClient();
                NetworkManager.StartPeerSearch("localhost");
            }
        }

        public void Update(float deltaTime)
        {
            
        }

        private void UpdateIPs()
        {
            globalIP = new Renderer.Text(new Layer(MainLayer.GUI, 0), Font.Bold, "Local IP:\n" + NetworkManager.LocalIP, 4, 0, new Vector2(0, 180));
            localIP = new Renderer.Text(new Layer(MainLayer.GUI, 0), Font.Bold, "Global IP:\n" + NetworkManager.PublicIP, 4, 0, new Vector2(0, 300));
        }

        public static void PeerFound(System.Net.IPEndPoint ipEndPoint, bool host, string message)
        {
            Test.Log((!host ? "Server found: " + message + ". " : "Peer found. ") + "IP: " + ipEndPoint + ". Local peer is host: " + host);

            if (XNAController.LocalGameClient)
            {
                NetworkManager.TryJoin(ipEndPoint.Address.ToString(), ipEndPoint.Port, "Local Server", callback);
            }

            singleton.endPoint = ipEndPoint;
        }

        void TestJoin()
        {
            Test.Log("Attempting join: " + endPoint);

            NetworkManager.TryJoin(endPoint.Address.ToString(), endPoint.Port, "JoinTest!", callback);
        }
    }
}
