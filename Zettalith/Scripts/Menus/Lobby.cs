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
        const string
            STARTHEADER = "START";

        private static Lobby singleton;
        private static Callback callback;

        private bool host, connected, ready;

        private System.Net.IPEndPoint endPoint;

        private GUI.Collection collection;
        private GUI.Button bStart, bBack;
        private Renderer.SpriteScreen localBackground, globalBackground;
        private Renderer.Text title, localIP, globalIP, statusHeader, status;

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

            status = new Renderer.Text(Layer.GUI, Font.Bold, "", 4, 0, new Vector2(Font.Default.MeasureString("Status: ").X, 200), Color.White);
            statusHeader = new Renderer.Text(Layer.GUI, Font.Default, "Status: ", 4, 0, new Vector2(0, 200));

            localIP = new Renderer.Text(Layer.GUI, Font.Default, "", 4, 0, new Vector2(0, 240));
            globalIP = new Renderer.Text(Layer.GUI, Font.Default, "", 4, 0, new Vector2(0, 280));

            bStart = new GUI.Button(Layer.GUI, new Rectangle(0, 380, 320, 80), Color.White) { ScaleEffect = true };
            bStart.AddText(host ? "Start" : "Ready", 6, true, Color.Black, Font.Bold);
            bStart.OnClick += BStart;

            bBack = new GUI.Button(Layer.GUI, new Rectangle(0, 500, 240, 60), Color.White) { ScaleEffect = true };
            bBack.AddText("Back", 4, true, Color.Black, Font.Default);
            bBack.OnClick += BBack;

            UpdateGUI();

            RendererController.GUI.Add(collection);
            collection.Add(title, localIP, globalIP, statusHeader, status, bStart, bBack);

            if (XNAController.LocalGameHost)
            {
                NetworkManager.CreateLocalGame();
            }

            if (XNAController.LocalGameClient)
            {
                NetworkManager.CreateClient();
                NetworkManager.StartPeerSearch("localhost");
            }

            NetworkManager.OnConnected += Connected;
            NetworkManager.OnDisconnected += Disconnected;

            NetworkManager.Listen(STARTHEADER, Ready);
        }

        public void Update(float deltaTime)
        {
            UpdateGUI();
        }

        private void UpdateGUI()
        {
            status.String = new StringBuilder(connected ? "Connected" : "Disconnected");
            status.Color = connected ? Color.GreenYellow : Color.OrangeRed;

            localIP.String = new StringBuilder("Global IP: " + NetworkManager.PublicIP);
            globalIP.String = new StringBuilder("Local IP: " + NetworkManager.LocalIP);

            bStart.SetPseudoDefaultColors(ready && connected ? Color.GreenYellow : Color.OrangeRed);
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

        void Connected()
        {
            connected = true;
        }

        void Disconnected()
        {
            ready = false;
            connected = false;
        }

        void BStart()
        {
            if (connected)
            {
                if (host)
                {
                    if (ready)
                    {
                        NetworkManager.Send(STARTHEADER, config.ToBytes());

                        Start(config.Value);
                    }

                    return;
                }

                ready = !ready;
                NetworkManager.Send(STARTHEADER, (ready).ToBytes());
            }
        }

        void BBack()
        {
            NetworkManager.DestroyPeer();

            collection.Active = false;
            MainController.Main.ToMenu();
        }

        void Ready(byte[] data)
        {
            if (host)
            {
                ready = data.ToObject<bool>();
                return;
            }

            Start(data.ToObject<StartupConfig>());
        }

        void Start(StartupConfig config)
        {
            collection.Active = false;

            MainController.Main.ToGame(config, host);
        }
    }
}
