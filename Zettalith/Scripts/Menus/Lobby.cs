﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    class Lobby
    {
        const string
            STARTHEADER = "START",
            RECIEVEDATAHEADER = "RECIEVEPLAYERDATA";

        const float
            TIMEOUT = 5.0f;

        private static Lobby singleton;
        private static Callback callback;

        private bool host, connected, ready, connecting;
        private float timeOut;

        private System.Net.IPEndPoint endPoint;

        private GUI.Collection collection;
        private GUI.Button bStart, bBack;
        private GUI.TextField tIpField;
        private Renderer.SpriteScreen localBackground, globalBackground;
        private Renderer.Text title, localIP, globalIP, statusHeader, status, ipFieldTitle;

        private Set testSet;

        private StartupConfig? config;

        public void Initialize(string playerName, StartupConfig? config = null)
        {
            singleton = this;
            host = config != null;
            this.config = config;

            //testSet = host ? PersonalData.UserData.SavedSets.Last() : PersonalData.UserData.SavedSets[0];
            SaveLoad.Load();
            testSet = PersonalData.UserData.SavedSets.Last();

            //testSet = new Set()
            //{
            //    Pieces = new List<Piece>()
            //    {
            //        new Piece(0, 3, 8),
            //        new Piece(1, 4, 9),
            //        new Piece(2, 5, 10),
            //        new Piece(16, 6, 11),
            //        new Piece(17, 6, 11),
            //        new Piece(18, 4, 9),
            //    }
            //};

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

            if (!XNAController.localGame)
            {
                if (config == null)
                {
                    bStart.ChangeText(connected ? "Ready" : (connecting ? "Connecting" : "Connect"));

                    ipFieldTitle = new Renderer.Text(Layer.GUI, Font.Bold, "Enter IP:", 3, 0, new Vector2(340, 380), Vector2.Zero, Color.White);
                    tIpField = new GUI.TextField(Layer.GUI, new Layer(MainLayer.GUI, 1), Font.Default, 4, new Rectangle(340, 415, 420, 40), new Vector2(345, 420), Vector2.Zero, "", Color.Black, Color.DarkGray, Load.Get<Texture2D>("Square"));
                    tIpField.AllowedText = GUI.TextField.TextType.Numbers | GUI.TextField.TextType.Periods;
                    tIpField.MaxLetters = 24;
                    collection.Add(tIpField, ipFieldTitle);

                    NetworkManager.CreateClient();
                }
                else
                {
                    NetworkManager.CreateHost("Good Server");
                }
            }

            NetworkManager.OnConnected += Connected;
            NetworkManager.OnDisconnected += Disconnected;

            NetworkManager.Listen(STARTHEADER, Ready);
            NetworkManager.Listen(RECIEVEDATAHEADER, LoadGame.RecieveData);
        }

        public void Update(float deltaTime)
        {
            UpdateGUI();

            if (connecting)
            {
                timeOut += deltaTime;
                if (timeOut > TIMEOUT)
                {
                    connecting = false;
                    ipFieldTitle.String = new StringBuilder("Timed out. Try again:");
                }
            }
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
            else if (singleton.config == null)
            {
                NetworkManager.TryJoin(ipEndPoint.Address.ToString(), ipEndPoint.Port, "Trying to join from: AA", callback);
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

            if (!host && !XNAController.localGame)
            {
                ipFieldTitle.String = new StringBuilder("Connected!");
                bStart.ChangeText("Ready");
                connecting = false;
            }
        }

        void Disconnected()
        {
            ready = false;
            connected = false;
            connecting = false;
        }

        void BStart()
        {
            if (connected)
            {
                if (host)
                {
                    if (ready)
                    {
                        StartupConfig tempConfig = config.Value;

                        tempConfig.seed = (new Random()).Next();

                        NetworkManager.Send(STARTHEADER, tempConfig);

                        Start(tempConfig);
                    }

                    return;
                }

                ready = !ready;
                NetworkManager.Send(STARTHEADER, ready);
            }
            else if (!host && !XNAController.localGame && !connecting)
            {
                if (tIpField.Content == NetworkManager.LocalIP || tIpField.Content == NetworkManager.PublicIP)
                {
                    ipFieldTitle.String = new StringBuilder("Very funny..");
                    return;
                }

                connecting = true;
                timeOut = 0;

                ipFieldTitle.String = new StringBuilder("Connecting...");

                NetworkManager.StartPeerSearch(tIpField.Content);
                NetworkManager.OnError += OnError;
            }
        }

        void OnError()
        {
            if (connecting)
            {
                ipFieldTitle.String = new StringBuilder("Error. Try again:");
                connecting = false;
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

            PlayerSetupData playerData = new PlayerSetupData()
            {
                set = /*PersonalData.UserData.SavedSets.Last()*/testSet
            };

            NetworkManager.Send(RECIEVEDATAHEADER, playerData);

            LoadGame.playerData = playerData;

            MainController.Main.ToGame(config, host);
        }
    }
}
