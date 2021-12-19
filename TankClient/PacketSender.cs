using System;
using System.Collections.Generic;
using System.Text;

namespace TankClient
{
    internal class PacketSender
    {
        public static void ConnectSender(string username)
        {
            Client.instance.username = username;
            List<byte> data = new List<byte>
            {
                (byte)0x00,
                (byte)ClientPackets.cConnect
            };
            Packet packet = new Packet(data.ToArray());
            packet.Write(username);
            Client.instance.tcp.SendData(packet);
        }
    }
}
