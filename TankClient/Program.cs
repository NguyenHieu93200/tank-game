using System;
using System.Collections.Generic;
using System.Text;

namespace TankClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Tank Client";
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("What's your name: ");
            Console.Write("> ");
            string username = Console.ReadLine();

            new Client("127.0.0.1", 3636);
            if (Client.instance == null)
            {
                Console.WriteLine("------------------------------------------------");
                Console.WriteLine("Can't connect to server. Please try again soon.");
            }
            PacketSender.ConnectSender(username);
            Client.instance.username = username;

            RoomMenu();

            while (true) ;
        }

        public static void RoomMenu()
        {
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("What do you want to do?\n1. Create room\n2. Get room list\n3. Join room\nEcs to exit.");

            while (true)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        CreateRoom();
                        return;
                    case ConsoleKey.D2:
                        GetRoomList();
                        return;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                };
            }
        }

        public static void CreateRoom()
        {
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("CreateRoom: ");
            Console.Write("Enter room name: ");
            string _roomName = Console.ReadLine();
            PacketSender.CreateRoomSender(Client.instance.id, _roomName);
        }

        public static void GetRoomList()
        {
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("JoinRoom: ");
            PacketSender.RoomListSender();
        }

        public static void JoinRoom()
        {
            int i = 0;
            Console.WriteLine("------------------------------------------------");
            foreach (Room room in Client.instance.rooms)
            {
                Console.Write($"{i++}) ");
                room.Print();
            }
            Console.WriteLine("Choose a room: ");
            int _idchoose = Convert.ToInt32(Console.ReadLine());
            if (_idchoose < 0)
            {
                RoomMenu();
            }
            PacketSender.JoinRoomSender(Client.instance.id, Client.instance.rooms[_idchoose].Id);
        }

        public static void RoomInfo()
        {
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine($"Room {Client.instance.roomName}:");
            foreach (Player player in Client.instance.players)
            {
                player.Print();
            }
        }

        public static void InsideRoom()
        {
            RoomInfo();
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("What do you want to do?\n1. Start Game\n2. Leave Room\nEcs to exit.");

            while (true)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        if (Client.instance.id == Client.instance.hostId)
                        {
                            StartGame();
                            return;
                        }
                        break;
                    case ConsoleKey.D2:
                        LeaveRoom();
                        return;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        return;
                };
            }
        }

        private static void StartGame()
        {
            PacketSender.StartGameSender(Client.instance.roomId);
        }

        private static void LeaveRoom()
        {
            Console.WriteLine("------------------------------------------------");
            PacketSender.LeaveRoomSender(Client.instance.id, Client.instance.roomId);
            Console.WriteLine("Leave room");
            RoomMenu();
        }

        public static void Ingame()
        {
            while (true)
            {
                Console.WriteLine("------------------------------------------------");
                Console.WriteLine("What do you want to do?\n1. Random move\n2. Random shoot\n3. Random speciall skill\nEcs to exit.");
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        
                        break;
                    case ConsoleKey.D2:
                        LeaveRoom();
                        return;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        return;
                };
            }
        }
    }
}
