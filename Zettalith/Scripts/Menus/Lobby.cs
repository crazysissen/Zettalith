﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Zettalith
{
    class Lobby
    {
        bool host;

        GUI.Collection collection;
        GUI.Button start;
        Renderer.SpriteScreen localBackground, globalBackground, playerBackground, opponentBackground;
        Renderer.Text title, localIP, globalIP, playerName, playerNameTitle, opponentName, opponentNameTitle;

        StartupConfig? config;

        public void Initialize(string playerName, StartupConfig? config = null)
        {
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
                NetworkManager.CreateHost(playerName + "'s Game");
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
    }
}