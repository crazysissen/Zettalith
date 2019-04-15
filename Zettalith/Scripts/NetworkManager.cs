using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Deployment.Application;
using Microsoft.Xna.Framework;
using Lidgren.Network;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace Zettalith
{
    public delegate void NetworkListener(byte[] message);

    public delegate void PeerFound();

    public delegate void Callback(bool sucess);

    static class NetworkManager
    {
        public const string tempIP = "10.156.46.100";

        public static string PublicIP { get; private set; }
        public static string LocalIP { get; private set; }
        public static string ServerName { get; private set; }

        static Dictionary<string, SerializedEvent> listeners = new Dictionary<string, SerializedEvent>();

        public static void Send(string callsign, object message)
        {
            Send(callsign, message.ToBytes());
        }

        public static void Send(string callsign, byte[] message)
        {
            if (localPeer != null)
            {
                if (localPeer.Connections.Count == 1)
                {
                    NetOutgoingMessage outMessage = localPeer.CreateMessage();
                    outMessage.Write(callsign);
                    outMessage.Write(message.Length);
                    outMessage.Write(message);

                    localPeer.SendMessage(outMessage, localPeer.Connections[0], NetDeliveryMethod.ReliableOrdered);
                }
            }
        }

        public static void Listen(string callsign, NetworkListener listener)
        {
            if (!listeners.ContainsKey(callsign))
            {
                listeners.Add(callsign, new SerializedEvent());
            }

            listeners[callsign] += listener; 
        }

        public static void RevokeListen(string callsign, NetworkListener listener)
        {
            if (listeners.ContainsKey(callsign))
            {
                listeners[callsign] -= listener;
            }
        }

        static void Recieve(string tag, byte[] message)
        {
            if (listeners.ContainsKey(tag))
            {
                listeners[tag].Call(message);
            }
        }

        #region Framework

        const int
            PORT = 14242,
            ALTERNATIVEPORT = 14243;

        static NetPeer localPeer;

        static string password;

        static Callback callback;

        public static event Action OnConnected, OnDisconnected; 

        public static void Initialize(XNAController xnaController)
        {
            xnaController.Exiting += DestroyPeerEvent;

            Thread threadG = new Thread(GetPublicIP), threadL = new Thread(GetLocalIP);
            threadG.Start();
            threadL.Start();
        }

        public static void CreateHost(string serverName)
        {
            Test.Log("Server created");

            if (localPeer != null)
                DestroyPeer();

            ServerName = serverName;

            //Version ver = ApplicationDeployment.CurrentDeployment.CurrentVersion;

            NetPeerConfiguration config = new NetPeerConfiguration(/*string.Format("Zettalith [{0}, {1}, {2}, {3}]", ver.Major, ver.Minor, ver.Build, ver.Revision)*/ "Test!")
            {
                MaximumHandshakeAttempts = 8,
                MaximumConnections = 10,
                Port = PORT
            };

            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            localPeer = new NetServer(config);

            localPeer.Start();
        }

        public static void CreateClient()
        {
            Test.Log("Client created");

            if (localPeer != null)
                DestroyPeer();

            //Version ver = ApplicationDeployment.CurrentDeployment.CurrentVersion;

            NetPeerConfiguration config = new NetPeerConfiguration(/*string.Format("Zettalith [{0}, {1}, {2}, {3}]", ver.Major, ver.Minor, ver.Build, ver.Revision)*/ "Test!")
            {
                MaximumHandshakeAttempts = 8,
                MaximumConnections = 10,
                Port = PORT
            };

            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            config.EnableMessageType(NetIncomingMessageType.StatusChanged);

            localPeer = new NetClient(config);

            localPeer.Start();
        }

        public static void CreateLocalGame()
        {
            Test.Log("Local server created");

            if (localPeer != null)
                DestroyPeer();

            ServerName = "localserver";

            //Version ver = ApplicationDeployment.CurrentDeployment.CurrentVersion;

            NetPeerConfiguration config = new NetPeerConfiguration("Test!")
            {
                MaximumHandshakeAttempts = 8,
                MaximumConnections = 10,
                Port = ALTERNATIVEPORT
            };

            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            localPeer = new NetServer(config);

            localPeer.Start();
        }

        public static void StartPeerSearch(string ip)
        {
            Test.Log("Peer Search Started");
            localPeer.DiscoverKnownPeer(ip, XNAController.localGame ? ALTERNATIVEPORT : PORT);
        }

        public static void StopPeerSearch()
        {

        }

        public static void TryJoin(string ip, int port, string message, Callback callback)
        {
            NetworkManager.callback = callback;

            NetOutgoingMessage connectionApproval = localPeer.CreateMessage();
            connectionApproval.Write(message);
            localPeer.Connect(ip, port, connectionApproval);
        }

        public static void DestroyPeerEvent(object s, EventArgs e)
        {
            DestroyPeer();
            return;
        }

        /// <summary>
        /// Cancel all current network activity.
        /// </summary>
        public static void DestroyPeer()
        {
            localPeer.Shutdown("Bye");
            localPeer = null;
        }

        public static void Update()
        {
            if (localPeer != null)
            {
                GetMessage();
            }
        }

        static void GetMessage()
        {
            NetIncomingMessage message;
            while ((message = localPeer.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.StatusChanged:

                        NetConnectionStatus state = (NetConnectionStatus)message.ReadByte();

                        Test.Log("Status changed: " + state);

                        if (state == NetConnectionStatus.Connected)
                        {
                            OnConnected?.Invoke();
                        }

                        if (state == NetConnectionStatus.Disconnected)
                        {
                            OnDisconnected?.Invoke();
                        }

                        break;

                    // This is the host and a connection attempt was recieved
                    case NetIncomingMessageType.ConnectionApproval:

                        string attachedMessage = message.ReadString();
                        Test.Log("Connection attempt: " + attachedMessage);
                        message.SenderConnection.Approve();

                        break;

                    case NetIncomingMessageType.Data:

                        string header = message.ReadString();
                        byte[] data = message.ReadBytes(message.ReadInt32());
                        Test.Log("Data recieved. Header: " + header);
                        Recieve(header, data);

                        break;

                    // This is the host and a discovery request was recieved
                    case NetIncomingMessageType.DiscoveryRequest:

                        NetOutgoingMessage response = localPeer.CreateMessage();
                        response.Write(ServerName);
                        localPeer.SendDiscoveryResponse(response, message.SenderEndPoint);
                        Lobby.PeerFound(message.SenderEndPoint, true, message.ReadString());

                        break;

                    // This is the client and a discovery request was returned with response
                    case NetIncomingMessageType.DiscoveryResponse:

                        Lobby.PeerFound(message.SenderEndPoint, false, message.ReadString());

                        break;

                    case NetIncomingMessageType.WarningMessage:

                        Test.Log("Warning: " + message.ReadString());

                        break;

                    //case NetIncomingMessageType.Error:
                    //    break;

                    //case NetIncomingMessageType.UnconnectedData:
                    //    break;

                    //case NetIncomingMessageType.Receipt:
                    //    break;

                    //case NetIncomingMessageType.VerboseDebugMessage:
                    //    break;

                    //case NetIncomingMessageType.DebugMessage:
                    //    break;

                    //case NetIncomingMessageType.ErrorMessage:
                    //    break;

                    //case NetIncomingMessageType.NatIntroductionSuccess:
                    //    break;

                    //case NetIncomingMessageType.ConnectionLatencyUpdated:
                    //    break;

                    default:
                        Test.Log("Unhandled type: " + message.MessageType);
                        break;
                }
            }
        }

        private static void GetLocalIP()
        {
            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ip in localIPs)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    Test.Log("Local IP Retrieved: " + ip);
                    LocalIP = ip.ToString();
                }
            }
        }

        private static void GetPublicIP()
        {
            System.Net.WebRequest request = System.Net.WebRequest.Create("http://checkip.dyndns.org");
            System.Net.WebResponse webResponse = request.GetResponse();
            System.IO.StreamReader reader = new System.IO.StreamReader(webResponse.GetResponseStream());

            string response = reader.ReadToEnd().Trim();
            string[] a = response.Split(':');
            string[] a2 = a[1].Substring(1).Split('<');

            PublicIP = a2[0];

            Test.Log("Public IP retrieved: " + PublicIP);
        }

        class SerializedEvent
        {
            protected event NetworkListener Listener;

            public void Call(byte[] message)
            {
                Listener?.Invoke(message);
            }

            public static SerializedEvent operator +(SerializedEvent kElement, NetworkListener kDelegate)
            {
                kElement.Listener += kDelegate;
                return kElement;
            }

            public static SerializedEvent operator -(SerializedEvent kElement, NetworkListener kDelegate)
            {
                kElement.Listener -= kDelegate;
                return kElement;
            }
        }

        #endregion
    }
}
