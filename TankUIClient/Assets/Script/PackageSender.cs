using System;
using System.Collections.Generic;
using System.Text;

namespace TankClient
{
    public class PacketSender
    {
        //cConnect = 1,
        public static void ConnectSender(string username)
        {
            Packet packet = new Packet(0x00, (byte)ClientPackets.cConnect);
            packet.Write(username);
            Client.instance.tcp.SendData(packet);
        }

        //cCreateRoom
        public static void CreateRoomSender(int _clientid, string roomname)
        {
            Packet packet = new Packet(0x00, (byte)ClientPackets.cCreateRoom);
            packet.Write(_clientid);
            packet.Write(roomname);
            Client.instance.tcp.SendData(packet);
        }

        //cRoomList,
        public static void RoomListSender()
        {
            Packet packet = new Packet(0x00, (byte)ClientPackets.cRoomList);
            Client.instance.tcp.SendData(packet);
        }

        //cJoinRoom,
        public static void JoinRoomSender(int _clientid, int _roomid)
        {
            Packet packet = new Packet(0x00, (byte)ClientPackets.cJoinRoom);
            packet.Write(_clientid);
            packet.Write(_roomid);
            Client.instance.tcp.SendData(packet);
        }

        //cLeaveRoom
        public static void LeaveRoomSender(int _clientid, int _roomid)
        {
            Packet packet = new Packet(0x00, (byte)ClientPackets.cLeaveRoom);
            packet.Write(_clientid);
            packet.Write(_roomid);
            Client.instance.tcp.SendData(packet);
        }

        //cInfoChange,
        public static void InfoChangeSender(int _clientid, int _roomid, byte team_id, byte tank_id)
        {
            Packet packet = new Packet(0x00, (byte)ClientPackets.cInfoChange);
            packet.Write(_clientid);
            packet.Write(_roomid);
            packet.Write(team_id);
            packet.Write(tank_id);
            Client.instance.tcp.SendData(packet);
        }

        //cStartGame,
        public static void StartGameSender(int _roomid)
        {
            Packet packet = new Packet(0x00, (byte)ClientPackets.cStartGame);
            packet.Write(_roomid);
            Client.instance.tcp.SendData(packet);
        }

        //cTankMove
        public static void TankMoveSender(int _clientid, int _roomid, float _x, float _z, float _angley, float _anglew)
        {
            Packet packet = new Packet(0x00, (byte)ClientPackets.cTankMove);
            packet.Write(_clientid);
            packet.Write(_roomid);
            packet.Write(_x);
            packet.Write(_z);
            packet.Write(_angley);
            packet.Write(_anglew);
            Client.instance.udp.SendData(packet);
        }

        //cTankShoot,
        public static void TankShootSender(int _clientid, int _roomid, float _x, float _z, float _angley, float _anglew)
        {
            Packet packet = new Packet(0x00, (byte)ClientPackets.cTankShoot);
            packet.Write(_clientid);
            packet.Write(_roomid);
            packet.Write(_x);
            packet.Write(_z);
            packet.Write(_angley);
            packet.Write(_anglew);
            Client.instance.udp.SendData(packet);
        }

        //cTankSpecial,
        public static void TankSpecialSender(int _clientid, int _roomid, float _x, float _z, float _angley, float _anglew)
        {
            Packet packet = new Packet(0x00, (byte)ClientPackets.cTankSpecial);
            packet.Write(_clientid);
            packet.Write(_roomid);
            packet.Write(_x);
            packet.Write(_z);
            packet.Write(_angley);
            packet.Write(_anglew);
            Client.instance.udp.SendData(packet);
        }

        //cTankHealth,
        public static void TankHealthSender(int _clientid, int _roomid, float _health)
        {
            Packet packet = new Packet(0x00, (byte)ClientPackets.cTankHealth);
            packet.Write(_clientid);
            packet.Write(_roomid);
            packet.Write(_health);
            Client.instance.tcp.SendData(packet);
        }

        //cTankDeath,
        public static void TankDeathSender(int _clientid, int _roomid)
        {
            Packet packet = new Packet(0x00, (byte)ClientPackets.cTankDeath);
            packet.Write(_clientid);
            packet.Write(_roomid);
            Client.instance.tcp.SendData(packet);
        }

        //cWinRound,
        public static void WinRoundSender(int _roomid, byte _teamid)
        {
            Packet packet = new Packet(0x00, (byte)ClientPackets.cWinRound);
            packet.Write(_roomid);
            packet.Write(_teamid);
            Client.instance.tcp.SendData(packet);
        }

        //cWinGame,
        public static void WinGameSender(int _roomid, byte _teamid)
        {
            Packet packet = new Packet(0x00, (byte)ClientPackets.cWinGame);
            packet.Write(_roomid);
            packet.Write(_teamid);
            Client.instance.tcp.SendData(packet);
        }

        //cDisconnect
        public static void DisconnectSender(int _clientid, int _roomid)
        {
            Packet packet = new Packet(0x00, (byte)ClientPackets.cDisconnect);
            packet.Write(_clientid);
            packet.Write(_roomid);
            Client.instance.tcp.SendData(packet);
        }
    }

}
