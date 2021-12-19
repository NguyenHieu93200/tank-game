using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace TankServer
{
    internal class Server
    {
        public static int MaxPlayers { get; private set; }
        public static int MaxRooms { get; private set; }   
        public static int Port { get; private set; }
        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
        public static Dictionary<int, Room> rooms = new Dictionary<int,Room>();

        private static TcpListener tcpListener = null;

        public static void Start(int _maxPlayers, int _maxRooms, int _port)
        {
            Console.WriteLine("Starting server....");

            Port = _port;
            MaxPlayers = _maxPlayers;
            MaxRooms = _maxRooms;
            Console.WriteLine($"Set MaxPlayers to {MaxPlayers}.\nSet MaxRooms to {MaxRooms}.");
            InitializeServerData();

            tcpListener = new TcpListener(localaddr: IPAddress.Any, port: Port);
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

        private static void InitializeServerData()
        {
            for (int i = 1; i <= MaxPlayers; i++)
            {
                clients.Add(i, new Client(i));
            }
            for (int i = 1;i <= MaxRooms; i++)
            {
                rooms.Add(i, null);
            }
        }
    }
}
