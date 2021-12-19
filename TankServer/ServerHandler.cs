using System;
using System.Collections.Generic;
using System.Text;

namespace TankServer
{
    internal class ServerHandler
    {
        public static void Handle(int _clientId, byte[] _data)
        {
            Packet packet = new Packet(_data);
            Console.WriteLine(packet.ToHexString());
            packet.ReadByte();
            byte packetType = packet.ReadByte();
            switch (packetType)
            {
                case (byte)ClientPackets.cConnect:
                    ConnectHandler(_clientId, packet);
                    break;
            }
        }

        private static void ConnectHandler(int _clientId, Packet packet)
        {
            Server.clients[_clientId].username = packet.ReadString();
            Console.WriteLine($"Client {_clientId} has name:{Server.clients[_clientId].username}");
            ServerSender.ConnectSender(_clientId);
        }
    }
}
