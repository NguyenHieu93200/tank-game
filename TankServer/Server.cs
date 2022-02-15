using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace TankServer
{
    internal class Server
    {
        public static int MaxPlayers { get; private set; }
        public static int MaxRooms { get; private set; }
        public static int Port { get; private set; }
        public static Dictionary<int, Client> clients = new();
        public static Dictionary<int, Room> rooms = new();

        private static TcpListener tcpListener = null;
        private static UdpClient udpListener = null;

        public static void Start(int _maxPlayers, int _maxRooms, int _port)
        {
            Console.WriteLine("Starting server....");

            Port = _port;
            MaxPlayers = _maxPlayers;
            MaxRooms = _maxRooms;
            Console.WriteLine($"Set MaxPlayers to {MaxPlayers}.\nSet MaxRooms to {MaxRooms}.");
            InitializeServerData();

            tcpListener = new TcpListener(new IPEndPoint(IPAddress.IPv6Any, Port));
            tcpListener.Server.DualMode = true;

            udpListener = new UdpClient(AddressFamily.InterNetworkV6);
            udpListener.Client.DualMode = true;
            udpListener.Client.Bind(new IPEndPoint(IPAddress.IPv6Any, Port));
            udpListener.BeginReceive(UDPReceiveCallback, null);

            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(callback: TcpClientAcceptCallback, state: null);

            Console.WriteLine($"Server started on port: {Port}.");
        }

        private static void TcpClientAcceptCallback(IAsyncResult _result)
        {
            TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
            tcpListener.BeginAcceptTcpClient(TcpClientAcceptCallback, null);

            Console.WriteLine($"Incoming connection from {_client.Client.RemoteEndPoint}...");

            for (int i = 1; i <= MaxPlayers; i++)
            {
                if (clients[i].tcp.socket == null)
                {
                    clients[i].tcp.Connect(_client);
                    return;
                }
            }

            Console.WriteLine($"{_client.Client.RemoteEndPoint} failed to connect: Server full!");
            _client.Close();
        }

        private static void UDPReceiveCallback(IAsyncResult _result)
        {
            try
            {
                IPEndPoint _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] _data = udpListener.EndReceive(_result, ref _clientEndPoint);
                udpListener.BeginReceive(UDPReceiveCallback, null);

                if (_data.Length <= 0)
                {
                    return;
                }

                Packet _packet = new Packet(_data);
                _packet.ReadInt();
                _packet.ReadByte();
                _packet.ReadByte();
                int _clientId = _packet.ReadInt();

                if (_clientId == 0)
                {
                    return;
                }

                if (clients[_clientId].udp.endPoint == null)
                {
                    // If this is a new connection
                    clients[_clientId].udp.Connect(_clientEndPoint);
                    return;
                }

                if (clients[_clientId].udp.endPoint.ToString() == _clientEndPoint.ToString())
                {
                    // Ensures that the client is not being impersonated by another by sending a false clientID
                    clients[_clientId].udp.HandleData(_packet);
                }
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error receiving UDP data: {_ex}");
            }
        }

        public static void SendUDPData(IPEndPoint _clientEndPoint, Packet _packet)
        {
            try
            {
                if (_clientEndPoint != null)
                {
                    udpListener.BeginSend(_packet.ToArray(), _packet.ToArray().Length, _clientEndPoint, null, null);
                }
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error sending data to {_clientEndPoint} via UDP: {_ex}");
            }
        }

        private static void InitializeServerData()
        {
            for (int i = 1; i <= MaxPlayers; i++)
            {
                clients.Add(i, new Client(i));
            }
            for (int i = 1; i <= MaxRooms; i++)
            {
                rooms.Add(i, null);
            }
        }
    }
}
