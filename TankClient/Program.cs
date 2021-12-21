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
            
            Console.WriteLine("What's your name: ");
            Console.Write("> ");
            string username = Console.ReadLine();

            new Client("127.0.0.1", 8000);
            if (Client.instance == null)
            {
                Console.WriteLine("Can't connect to server. Please try again soon.");
            }
            PacketSender.ConnectSender(username);

            RoomMenu();
            Console.ReadKey();
        }

        private static void RoomMenu()
        {
            Console.WriteLine("What do you want to do?\n1. Create room\n2. Join room\nOther key to exit.");
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.D1:
                    CreateRoom();
                    break;
                case ConsoleKey.D2:
                    JoinRoom();
                    break;
                default:
                    break;
            };
        }

        private static void CreateRoom()
        {
            Console.WriteLine("CreateRoom");
            Packet packet = new Packet(0x00, (byte)ClientPackets.cCreateRoom);
            packet.Write(Client.instance.id);
            packet.Write("Weird room.");
            Client.instance.tcp.SendData(packet);
        }

        private static void JoinRoom()
        {
            Console.WriteLine("JoinRoom");
        }

        private static void RoomInfo()
        {

        }
    }
}
