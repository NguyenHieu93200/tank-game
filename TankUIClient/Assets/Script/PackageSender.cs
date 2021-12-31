using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TankClient 
{
    internal class PacketSender
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

        }
        //cStartGame,







        //cTankMove,
        //cTankShoot,
        //cTankSpecial,
        //cTankHealth,
        //cTankDeath,
        //cWinRound,
        //cWinGame,
        //cDisconnect

    }













}
