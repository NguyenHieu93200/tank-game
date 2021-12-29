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
                //sError,
                case (byte)ServerPackets.sError:
                    ErrorHandler(packet);
                    break;
                //sConnect,
                case (byte)ServerPackets.sConnect:
                    ConnectHandler(packet);
                    break;
                //sRoomInfo,
                case (byte)ServerPackets.sRoomInfo:
                    RoomInfoHandler(packet);
                    break;
                //sRoomList,
                case (byte)ServerPackets.sRoomList:
                    RoomListHandler(packet);
                    break;
                //sGameStart,
                case (byte)ServerPackets.sGameStart:
                    GameStartHandler(packet);
                    break;
                //sTankPosition,
                case (byte)ServerPackets.sTankPosition:
                    TankPositionHandler(packet);
                    break;
                //sTankShoot,
                case (byte)ServerPackets.sTankShoot:
                    TankShootHandler(packet);
                    break;
                //sTankSpecial,
                case (byte)ServerPackets.sTankSpecial:
                    TankSpecialHandler(packet);
                    break;
                //sTankHealth,
                case (byte)ServerPackets.sTankHealth:
                    TankHealthHandler(packet);
                    break;
                //sTankDeath,
                case (byte)ServerPackets.sTankDeath:
                    TankDeathHandler(packet);
                    break;
                //sWinRound,
                case (byte)ServerPackets.sWinRound:
                    WinRoundHandler(packet);
                    break;
                //sWinGame,
                case (byte)ServerPackets.sWinGame:
                    WinGameHandler(packet);
                    break;
                //sDisconnect
                case (byte)ServerPackets.sDisconnect:
                    DisconnectHandler(packet);
                    break;
                //sLeaveRoom
                case (byte)ServerPackets.sLeaveRoom:
                    LeaveRoomHandler(packet);
                    break;
            }
        }

        //sConnect,
        private static void ConnectHandler(Packet packet)
        {
            try
            {
                Client.instance.id = packet.ReadInt();
                Console.WriteLine($"Receive id: {Client.instance.id} from server.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Client.instance.Disconnect();
            }
        }

        //sError,
        private static void ErrorHandler(Packet packet)
        {
            Console.WriteLine($"Error: {packet.ReadString()}");
            Client.instance.Disconnect();
        }

        //sRoomInfo,
        private static void RoomInfoHandler(Packet packet)
        {
            try
            {
                Client.instance.players.Clear();
                Client.instance.roomId = packet.ReadInt();
                Client.instance.roomName = packet.ReadString();
                Client.instance.hostId = packet.ReadInt();
                int _memberCount = packet.ReadInt();
                for (int i = 0; i < _memberCount; i++)
                {
                    Client.instance.players.Add(new Player(_playerId: packet.ReadInt(), _username: packet.ReadString(), _team: packet.ReadByte(), _tankId: packet.ReadByte()));
                }
                Program.InsideRoom();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        //sRoomList,
        private static void RoomListHandler(Packet packet)
        {
            try
            {
                Client.instance.rooms.Clear();
                int roomCount = packet.ReadInt();
                for (int i = 0; i < roomCount; i++)
                {
                    Client.instance.rooms.Add(new Room(_id: packet.ReadInt(), _name: packet.ReadString(), _numberOfMembers: packet.ReadInt()));
                }
                Program.JoinRoom();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        //sLeaveRoom,
        private static void LeaveRoomHandler(Packet packet)
        {
            try
            {
                int _clientId = packet.ReadInt();
                if (Client.instance.id == _clientId)
                {
                    Client.instance.players.Clear();
                    Client.instance.roomId = 0;
                    Client.instance.roomName = "";
                    Client.instance.hostId = 0;
                    return;
                }
                foreach (Player player in Client.instance.players)
                {
                    if (player.playerId == _clientId)
                    {
                        Client.instance.players.Remove(player);
                        break;
                    }
                }
                Program.RoomInfo();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        //sGameStart,
        private static void GameStartHandler(Packet packet)
        {
            try
            {
                Console.WriteLine("------------------------------------------------");
                Console.WriteLine("Start game!!!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        //sTankPosition,
        private static void TankPositionHandler(Packet packet)
        {
            try
            {
                Console.WriteLine("------------------------------------------------");
                Console.WriteLine("Start game!!!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        //sTankShoot,
        private static void TankShootHandler(Packet packet)
        {
            try
            {
                Console.WriteLine("------------------------------------------------");
                Console.WriteLine("Start game!!!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        //sTankSpecial,
        private static void TankSpecialHandler(Packet packet)
        {
            try
            {
                Console.WriteLine("------------------------------------------------");
                Console.WriteLine("Start game!!!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        //sTankHealth,
        private static void TankHealthHandler(Packet packet)
        {
            try
            {
                Console.WriteLine("------------------------------------------------");
                Console.WriteLine("Start game!!!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        //sTankDeath,
        private static void TankDeathHandler(Packet packet)
        {
            try
            {
                Console.WriteLine("------------------------------------------------");
                Console.WriteLine("Start game!!!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        //sWinRound,
        private static void WinRoundHandler(Packet packet)
        {
            try
            {
                Console.WriteLine("------------------------------------------------");
                Console.WriteLine("Start game!!!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        //sWinGame,
        private static void WinGameHandler(Packet packet)
        {
            try
            {
                Console.WriteLine("------------------------------------------------");
                Console.WriteLine("Start game!!!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        //sDisconnect
        private static void DisconnectHandler(Packet packet)
        {
            try
            {
                Console.WriteLine("------------------------------------------------");
                Console.WriteLine("Start game!!!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

    }
}
