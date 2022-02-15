using System;

namespace TankServer
{
    internal class Program
    {
        static void Main()
        {
            Console.Title = "Tank Server";
            Server.Start(_maxPlayers: 4, _maxRooms: 1, _port: 3636);
            Console.ReadKey();
        }
    }
}
