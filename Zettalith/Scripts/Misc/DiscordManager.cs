﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordRPC;
using DiscordRPC.Logging;

namespace Zettalith
{
    class DiscordManager
    {
        public DiscordRpcClient Discord { get; private set; }

        bool isClient;

        public void Init(bool isClient)
        {
            this.isClient = isClient;

            if (isClient)
                return;

            Discord = new DiscordRpcClient("570224520460369926");

            Discord.Logger = new ConsoleLogger() { Level = LogLevel.Warning, Colored = true };

            Discord.OnReady += (sender, e) =>
            {
                Console.WriteLine("Received Ready from user {0}", e.User.Username);
            };

            Discord.OnPresenceUpdate += (sender, e) =>
            {
                Console.WriteLine("Received Update! {0}", e.Presence);
            };

            Discord.Initialize();

            SetMenu("Main Menu");
        }

        public void SetMenu(string subMenu, Party party = null)
        {
            if (!isClient)
            {
                Discord.SetPresence(new RichPresence()
                {
                    Details = "In Menu",
                    State = subMenu,

                    Assets = new Assets()
                    {
                        LargeImageKey = "biglogo",
                        SmallImageKey = "wait",
                        SmallImageText = "Menu"
                    }
                });
            }
        }

        public void SetCollection()
        {
            if (!isClient)
            {
                Discord.SetPresence(new RichPresence()
                {
                    Details = "In Collection",
                    State = "Assembling Pieces",

                    Assets = new Assets()
                    {
                        LargeImageKey = "biglogo",
                        SmallImageKey = "collection",
                        SmallImageText = "Collection"
                    }
                });
            }
        }

        public void SetBattle(string standing)
        {
            if (!isClient)
            {
                Discord.SetPresence(new RichPresence()
                {
                    Details = "In Battle!",
                    State = standing,

                    Assets = new Assets()
                    {
                        LargeImageKey = "biglogo",
                        SmallImageKey = "battle",
                        SmallImageText = "Battle"
                    }
                });
            }
        }
    }
}
