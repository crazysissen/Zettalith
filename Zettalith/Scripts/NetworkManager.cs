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

namespace Zettalith
{
    public delegate void NetworkListener(byte[] message);

    public delegate void PeerFound();

    public delegate void Callback(bool sucess);

    static class NetworkManager
    {
        public static string PublicIP { get; private set; }
        public static string ServerName { get; private set; }

        static Dictionary<string, EventListener> listeners = new Dictionary<string, EventListener>();

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
                    outMessage.Data = message;

                    localPeer.SendMessage(outMessage, localPeer.Connections[0], NetDeliveryMethod.ReliableOrdered);
                }
            }
        }

        public static void Listen(string callsign, NetworkListener listener)
        {
            if (!listeners.ContainsKey(callsign))
            {
                listeners.Add(callsign, new EventListener());
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

        }

        public static void GetPublicIP()
        {
            string url = "http://checkip.dyndns.org";
            System.Net.WebRequest req = System.Net.WebRequest.Create(url);
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            string response = sr.ReadToEnd().Trim();
            string[] a = response.Split(':');
            string a2 = a[1].Substring(1);
            string[] a3 = a2.Split('<');
            string a4 = a3[0];
            PublicIP = a4;

            System.Diagnostics.Debug.WriteLine(PublicIP);
        }

        #region Framework

        const int PORT = 14242;

        static NetPeer localPeer;

        static string password;

        static Callback callback;

        public static void Initialize(XNAController xnaController)
        {
            xnaController.Exiting += DestroyPeerEvent;

            Thread thread = new Thread(GetPublicIP);
            thread.Start();
        }

        public static void CreateHost(string serverName)
        {
            if (localPeer != null)
                DestroyPeer();

            ServerName = serverName;

            //Version ver = ApplicationDeployment.CurrentDeployment.CurrentVersion;

            NetPeerConfiguration config = new NetPeerConfiguration(/*string.Format("Zettalith [{0}, {1}, {2}, {3}]", ver.Major, ver.Minor, ver.Build, ver.Revision)*/ "Test!")
            {
                MaximumHandshakeAttempts = 8,
                MaximumConnections = 2,
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
            if (localPeer != null)
                DestroyPeer();

            //Version ver = ApplicationDeployment.CurrentDeployment.CurrentVersion;

            NetPeerConfiguration config = new NetPeerConfiguration(/*string.Format("Zettalith [{0}, {1}, {2}, {3}]", ver.Major, ver.Minor, ver.Build, ver.Revision)*/ "Test")
            {
                MaximumHandshakeAttempts = 8,
                MaximumConnections = 2,
                Port = PORT
            };

            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            config.EnableMessageType(NetIncomingMessageType.StatusChanged);

            localPeer = new NetClient(config);

            localPeer.Start();
        }

        public static void StartPeerSearch(string ip)
        {
            localPeer.DiscoverKnownPeer(ip, PORT);
        }

        public static void StopPeerSearch()
        {

        }

        public static void TryJoin(IPEndPoint endPoint, string message, Callback callback)
        {
            NetworkManager.callback = callback;

            NetOutgoingMessage connectionApproval = localPeer.CreateMessage();
            connectionApproval.Write(message);
            localPeer.Connect(endPoint, connectionApproval);
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

        }

        static bool connected = false;
        public static void Update()
        {
            if (localPeer != null)
            {
                if (localPeer.Connections.Count > 0 && connected == false)
                {
                    connected = true;
                    Debug.WriteLine("Connected!");
                }

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
                    //case NetIncomingMessageType.Error:
                    //    break;

                    case NetIncomingMessageType.StatusChanged:
                        Debug.WriteLine("Status changed: " + (NetConnectionStatus)message.ReadByte());
                        break;

                    //case NetIncomingMessageType.UnconnectedData:
                    //    break;

                    case NetIncomingMessageType.ConnectionApproval:
                        string attachedMessage = message.ReadString();
                        Debug.WriteLine("Connection attempt: " + attachedMessage);
                        message.SenderConnection.Approve();
                        break;

                    case NetIncomingMessageType.Data:
                        string header = message.ReadString();
                        byte[] data = message.Data;
                        Recieve(header, data);
                        break;

                    //case NetIncomingMessageType.Receipt:
                    //    break;

                    case NetIncomingMessageType.DiscoveryRequest:
                        System.Diagnostics.Debug.WriteLine("Request recieved: " + message.SenderEndPoint);
                        NetOutgoingMessage response = localPeer.CreateMessage();
                        response.Write(ServerName);
                        localPeer.SendDiscoveryResponse(response, message.SenderEndPoint);
                        break;

                    case NetIncomingMessageType.DiscoveryResponse:
                        XNAController.MainController.PeerFound(message.SenderEndPoint, false, message.ReadString());
                        break;

                    //case NetIncomingMessageType.VerboseDebugMessage:
                    //    break;

                    //case NetIncomingMessageType.DebugMessage:
                    //    break;

                    //case NetIncomingMessageType.WarningMessage:
                    //    break;

                    //case NetIncomingMessageType.ErrorMessage:
                    //    break;

                    //case NetIncomingMessageType.NatIntroductionSuccess:
                    //    break;

                    //case NetIncomingMessageType.ConnectionLatencyUpdated:
                    //    break;

                    default:
                        System.Diagnostics.Debug.WriteLine("Unhandled type: " + message.MessageType);
                        break;
                }
            }
        }

        class EventListener
        {
            protected event NetworkListener Listener;

            public void Call(byte[] message)
            {
                Listener?.Invoke(message);
            }

            public static EventListener operator +(EventListener kElement, NetworkListener kDelegate)
            {
                kElement.Listener += kDelegate;
                return kElement;
            }

            public static EventListener operator -(EventListener kElement, NetworkListener kDelegate)
            {
                kElement.Listener -= kDelegate;
                return kElement;
            }
        }

        #endregion
    }
}
