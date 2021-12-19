using System;

namespace TankServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Tank Server";
            Server.Start(2, 8000);
            Console.ReadKey();
        }
    }
}
