﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordRPC;
using DiscordRPC.Logging;
using DiscordRPC.Message;
using System.Security.Cryptography;

namespace Zettalith
{
    class DiscordManager
    {
        public DiscordRpcClient Discord { get; private set; }
        public event Action<string> OnJoinEvent;

        string password = "F35F3B18C9694CAC1F0A3F91F62CDE3D5F54B5656215D66568DF1B4FF9BF41B8";

        bool isClient;
        string id;
        Party party;

        Aes encryption;

        public void Init(bool isClient)
        {
            this.isClient = isClient;

            encryption = Aes.Create();

            Random r = new Random();
            byte[] bytes = new byte[64];
            r.NextBytes(bytes);

            id = new string(System.Text.Encoding.UTF8.GetChars(bytes));

            party = new Party()
            {
                ID = id,
                Max = 2
            };

            if (isClient)
                return;

            Discord = new DiscordRpcClient("570224520460369926");
            Discord.RegisterUriScheme();

            Discord.Logger = new ConsoleLogger() { Level = LogLevel.Warning, Colored = true };

            Discord.OnReady += (sender, e) =>
            {
                Console.WriteLine("Received Ready from user {0}", e.User.Username);
            };

            Discord.Initialize();

            Discord.OnJoin += OnJoin;
            Discord.OnJoinRequested += OnJoinRequested;

            SetMenu("Main Menu");
        }

        public void Update()
        {
            IMessage[] messages = Discord.Invoke();

            foreach (IMessage message in messages)
            {
            }
        }

        private void OnJoinRequested(object sender, DiscordRPC.Message.JoinRequestMessage args)
        {
            
        }

        private void OnJoin(object sender, DiscordRPC.Message.JoinMessage args)
        {
            OnJoinEvent?.Invoke(AESThenHMAC.SimpleDecryptWithPassword(args.Secret, password));
        }

        public void SetMenu(string subMenu, string globalIP = null)
        {
            if (!isClient)
            {
                RichPresence presence = new RichPresence()
                {
                    Details = "In Menu",
                    State = subMenu,

                    Assets = new Assets()
                    {
                        LargeImageKey = "biglogo",
                        SmallImageKey = "wait",
                        SmallImageText = "Menu"
                    }
                };

                if (globalIP != null)
                {
                    presence.Party = party;
                    presence.Secrets = new Secrets()
                    {
                        JoinSecret = AESThenHMAC.SimpleEncryptWithPassword(globalIP, password)
                    };
                }

                Discord.SetPresence(presence);
            }
        }

        public void SetCollection(string globalIP = null)
        {
            if (!isClient)
            {
                RichPresence presence = new RichPresence()
                {
                    Details = "In Menu",
                    State = "Assembling Pieces",

                    Assets = new Assets()
                    {
                        LargeImageKey = "biglogo",
                        SmallImageKey = "collection",
                        SmallImageText = "Collection"
                    }
                };

                if (globalIP != null)
                {
                    presence.Party = party;
                    presence.Secrets = new Secrets()
                    {
                        JoinSecret = AESThenHMAC.SimpleEncryptWithPassword(globalIP, password)
                    };
                }

                Discord.SetPresence(presence);
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
