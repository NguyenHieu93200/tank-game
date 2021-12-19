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
            Packet packet = new Packet(0x00, (byte)ClientPackets.cConnect);
            packet.Write(username);
            Client.instance.tcp.SendData(packet);
        }
    }
}
