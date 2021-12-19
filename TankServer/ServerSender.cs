using System;
using System.Collections.Generic;
using System.Text;

namespace TankServer
{
    internal class ServerSender
    {
        public static void ConnectSender(int _clientId)
        {
            List<byte> data = new List<byte>
            {
                (byte)0x01,
                (byte)ServerPackets.sConnect
            };
            Packet packet = new Packet(data.ToArray());
            packet.Write(_clientId);
            Server.clients[_clientId].tcp.SendData(packet);
        }
    }
}
