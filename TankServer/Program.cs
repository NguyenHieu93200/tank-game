using System;

namespace TankServer
{
    internal class Program
    {
        static void Main()
        {
            Console.Title = "Tank Server";
            Server.Start(_maxPlayers: 8, _maxRooms: 2, _port: 3636);
            Console.ReadKey();
        }
    }
}
