using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Deployment.Application;
using Microsoft.Xna.Framework;
using Lidgren.Network;

namespace Zettalith
{
    public delegate void NetworkListener(byte[] message);

    public delegate void Callback(bool sucess);

    static class NetworkManager
    {
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

        #region Framework

        const int hostPort = 0;

        static NetPeer localPeer;

        public static void Host()
        {
            Version ver = ApplicationDeployment.CurrentDeployment.CurrentVersion;

            NetPeerConfiguration config = new NetPeerConfiguration(string.Format("Zettalith [{0}, {1}, {2}, {3}]", ver.Major, ver.Minor, ver.Build, ver.Revision))
            {
                MaximumHandshakeAttempts = 8,
                MaximumConnections = 1
            };

            localPeer = new NetServer(config);
        }

        public static void TryJoin(Callback callback)
        {

        }

        public static void Update()
        {
            NetIncomingMessage message;
            while ((message = localPeer.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Error:
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        break;

                    case NetIncomingMessageType.UnconnectedData:
                        break;

                    case NetIncomingMessageType.ConnectionApproval:
                        break;

                    case NetIncomingMessageType.Data:
                        string header = message.ReadString();
                        byte[] data = message.Data;
                        Recieve(header, data);
                        break;

                    case NetIncomingMessageType.Receipt:
                        break;

                    case NetIncomingMessageType.DiscoveryRequest:
                        break;

                    case NetIncomingMessageType.DiscoveryResponse:
                        break;

                    case NetIncomingMessageType.VerboseDebugMessage:
                        break;

                    case NetIncomingMessageType.DebugMessage:
                        break;

                    case NetIncomingMessageType.WarningMessage:
                        break;

                    case NetIncomingMessageType.ErrorMessage:
                        break;

                    case NetIncomingMessageType.NatIntroductionSuccess:
                        break;

                    case NetIncomingMessageType.ConnectionLatencyUpdated:
                        break;

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
