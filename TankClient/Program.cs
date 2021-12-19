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
            PacketSender.ConnectSender(username);

            Console.ReadKey();
        }
    }
}
