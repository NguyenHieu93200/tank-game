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
            Console.Write($"Receive from {_clientId}: ");
            Console.WriteLine(packet.ToHexString());
            packet.ReadByte();
            byte packetType = packet.ReadByte();
            switch (packetType)
            {
                //cConnect
                case (byte)ClientPackets.cConnect:
                    ConnectHandler(_clientId, packet);
                    break;
                //cCreateRoom
                case (byte)ClientPackets.cCreateRoom:
                    CreateRoomHandler(_clientId, packet);
                    break;
                //cRoomList
                case (byte)ClientPackets.cRoomList:
                    RoomListHandler(_clientId, packet);
                    break;
                //cJoinRoom
                case (byte)ClientPackets.cJoinRoom:
                    JoinRoomHandler(_clientId, packet);
                    break;
                //cInfoChange
                case (byte)ClientPackets.cInfoChange:
                    InfoChangeHandler(_clientId, packet);
                    break;
                //cStartGame
                case (byte)ClientPackets.cStartGame:
                    StartGameHandler(_clientId, packet);
                    break;
                //cTankMove
                case (byte)ClientPackets.cTankMove:
                    TankMoveHandler(_clientId, packet);
                    break;
                //cTankShoot
                case (byte)ClientPackets.cTankShoot:
                    TankShootHandler(_clientId, packet);
                    break;
                //cTankSpecial
                case (byte)ClientPackets.cTankSpecial:
                    TankSpecialHandler(_clientId, packet);
                    break;
                //cTankHealth
                case (byte)ClientPackets.cTankHealth:
                    TankHealthHandler(_clientId, packet);
                    break;
                //cTankDeath
                case (byte)ClientPackets.cTankDeath:
                    TankDeathHandler(_clientId, packet);
                    break;
                //cWinRound
                case (byte)ClientPackets.cWinRound:
                    WinRoundHandler(_clientId, packet);
                    break;
                //cWinGame
                case (byte)ClientPackets.cWinGame:
                    WinGameHandler(_clientId, packet);
                    break;
                //cDisconnect
                case (byte)ClientPackets.cDisconnect:
                    DisconnectHandler(_clientId, packet);
                    break;
                //cLeaveRoom
                case (byte)ClientPackets.cLeaveRoom:
                    LeaveRoomHandler(_clientId, packet);
                    break;
            }
        }

        //cConnect
        private static void ConnectHandler(int _clientId, Packet packet)
        {
            Server.clients[_clientId].username = packet.ReadString();
            ServerSender.ConnectSender(_clientId);
            Console.WriteLine($"Client {_clientId} has name:{Server.clients[_clientId].username}");
        }

        //cCreateRoom
        private static void CreateRoomHandler(int _clientId, Packet packet)
        {
            for (int i = 1; i <= Server.rooms.Count; i++)
            {
                if (Server.rooms[i] == null)
                {
                    int _client = packet.ReadInt();
                    string _roomName = packet.ReadString();
                    Server.rooms[i] = new Room(i, _roomName, Server.clients[_client]);
                    ServerSender.RoomInfoSender(_client, i);
                    Console.WriteLine($"Client {Server.clients[_client].username} has created a room which name {_roomName}");
                    return;
                }
            }
            ServerSender.ErrorSender(_clientId, "Server is full.");
            Console.WriteLine($"Client {_clientId}:{Server.clients[_clientId].username} try to create a room, but server is full.");
        }

        //cRoomList
        private static void RoomListHandler(int _clientId, Packet packet)
        {
            ServerSender.RoomListSender(_clientId);
            Console.WriteLine($"Sent Room list to client {_clientId}.");
        }

        //cJoinRoom
        private static void JoinRoomHandler(int _clientId, Packet packet)
        {
            int _client = packet.ReadInt();
            int _roomId = packet.ReadInt();
            if (Server.rooms[_roomId].GetClient().Count < Room.MaxPlayersPerTeam*2)
            {
                Server.rooms[_roomId].AddClient(Server.clients[_client]);
                ServerSender.RoomInfoSender(_client, _roomId, _toAll: true);
                Console.WriteLine($"Client {_client} has joined room {_roomId}.");
            }
            else
            {
                ServerSender.ErrorSender(_client, "Room is full.");
                Console.WriteLine($"Client {_client} try to join room {_roomId} but room is full.");
            }
        }

        //cInfoChange
        private static void InfoChangeHandler(int _clientId, Packet packet)
        {
            int _client = packet.ReadInt();
            int _roomId = packet.ReadInt();
            byte _teamId = packet.ReadByte();
            byte _tankId = packet.ReadByte();
            if (Server.clients[_client].teamId != _teamId)
            {
                if (Server.rooms[_roomId].teams[_teamId].Count < Room.MaxPlayersPerTeam)
                {
                    Server.rooms[_roomId].ChangeTeam(Server.clients[_client], Server.clients[_client].teamId, _teamId);
                }
                else
                {
                    ServerSender.ErrorSender(_client, "Team is full");
                    Console.WriteLine($"Client {_client} try to join team {_teamId} in room {_roomId} but team is full.");
                    return;
                }
            }
            Server.clients[_client].tankId = _tankId;
            ServerSender.RoomInfoSender(_client, _roomId, _toAll: true);
            Console.WriteLine($"Client {_client} in room {_roomId} change to team {_teamId} and using tank {_tankId}.");
        }

        //cStartGame
        private static void StartGameHandler(int _clientId, Packet packet)
        {
            int _roomId = packet.ReadInt();
            Server.rooms[_roomId].isInGame = true;
            ServerSender.GameStartSender(_roomId);
            Console.WriteLine($"Game has started in room {_roomId}.");
        }

        //cTankMove
        private static void TankMoveHandler(int _clientId, Packet packet)
        {
            int _client = packet.ReadInt();
            int _roomId = packet.ReadInt();
            ServerSender.TankPositionSender(packet, _roomId);
            Console.WriteLine($"Client {_client} has moved in room {_roomId}.");
        }

        //cTankShoot
        private static void TankShootHandler(int _clientId, Packet packet)
        {
            int _client = packet.ReadInt();
            int _roomId = packet.ReadInt();
            ServerSender.TankShootSender(packet, _roomId);
            Console.WriteLine($"Client {_client} has shot in room {_roomId}.");
        }

        //cTankSpecial
        private static void TankSpecialHandler(int _clientId, Packet packet)
        {
            int _client = packet.ReadInt();
            int _roomId = packet.ReadInt();
            ServerSender.TankSpecialSender(packet, _roomId);
            Console.WriteLine($"Client {_client} has cast special ability in room {_roomId}.");
        }

        //cTankHealth
        private static void TankHealthHandler(int _clientId, Packet packet)
        {
            int _client = packet.ReadInt();
            int _roomId = packet.ReadInt();
            ServerSender.TankHealthSender(packet, _roomId);
            Console.WriteLine($"Client {_client} has been shot in room {_roomId}.");
        }

        //cTankDeath
        private static void TankDeathHandler(int _clientId, Packet packet)
        {
            int _client = packet.ReadInt();
            int _roomId = packet.ReadInt();
            ServerSender.TankPositionSender(packet, _roomId);
            Console.WriteLine($"Client {_client} has been slain moved in room {_roomId}.");
        }

        //cWinRound
        private static void WinRoundHandler(int _clientId, Packet packet)
        {
            int _roomId = packet.ReadInt();
            byte _teamId = packet.ReadByte();
            ServerSender.TankPositionSender(packet, _roomId);
            Console.WriteLine($"Team {_teamId} has won this round in room {_roomId}.");
        }

        //cWinGame
        private static void WinGameHandler(int _clientId, Packet packet)
        {
            int _roomId = packet.ReadInt();
            byte _teamId = packet.ReadByte();
            ServerSender.TankPositionSender(packet, _roomId);
            Console.WriteLine($"Team {_teamId} has won this game in room {_roomId}.");
            Server.rooms[_roomId].CloseRoom();
            Console.WriteLine($"Room {_roomId} is closed.");
        }

        //cDisconnect
        private static void DisconnectHandler(int _clientId, Packet packet)
        {
            int _client = packet.ReadInt();
            int _roomId = packet.ReadInt();
            ServerSender.DisconnectSender(packet, _roomId);
            if (_client == Server.rooms[_roomId].hostId)
            {
                Server.rooms[_roomId].CloseRoom();
                Console.WriteLine($"Client {_clientId} who is host, has disconnected. Room {_roomId} is closed.");
            }
            else
            {
                Server.rooms[_roomId].RemoveClient(Server.clients[_client]);
                Console.WriteLine($"Client {_clientId} has disconnected.");
            }
        }

        //cLeaveRoom
        private static void LeaveRoomHandler(int _clientId, Packet packet)
        {
            int _client = packet.ReadInt();
            int _roomId = packet.ReadInt();
            ServerSender.LeaveRoomSender(packet, _roomId);
            if (_client == Server.rooms[_roomId].hostId)
            {
                Server.rooms[_roomId].CloseRoom();
                Console.WriteLine($"Client {_clientId} who is host, has left room. Room {_roomId} is closed.");
            }
            else
            {
                Server.rooms[_roomId].RemoveClient(Server.clients[_client]);
                Console.WriteLine($"Client {_clientId} has left room.");
            }
        }

    }
}
