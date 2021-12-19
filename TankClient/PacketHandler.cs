using System;
using System.Collections.Generic;
using System.Text;

namespace TankClient
{
    internal class PacketHandler
    {
        public static void Handle(byte[] _data)
        {
            Packet packet = new Packet(_data);
            packet.ReadByte();
            byte packetType = packet.ReadByte();
            switch (packetType)
            {
                case (byte)ServerPackets.sConnect:
                    ConnectHandler(packet);
                    break;
            }
        }

        private static void ConnectHandler(Packet packet)
        {
            try
            {
                Client.instance.id = packet.ReadInt();
                Console.WriteLine($"Receive id: {Client.instance.id} from server.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Client.instance.Disconnect();
            }
        }
    }
}
