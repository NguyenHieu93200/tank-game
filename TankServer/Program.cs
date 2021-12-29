using System;

namespace TankServer
{
    internal class Program
    {
        static void Main()
        {
            Console.Title = "Tank Server";
            Server.Start(_maxPlayers: 4, _maxRooms: 4, _port: 8000);
            Console.ReadKey();
        }
    }
}
