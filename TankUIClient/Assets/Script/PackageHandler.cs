using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TankClient;
using UnityEngine.SceneManagement;

public class PacketHandler
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
            ////sTankPosition,
            //case (byte)ServerPackets.sTankPosition:
            //    TankPositionHandler(packet);
            //    break;
            ////sTankShoot,
            //case (byte)ServerPackets.sTankShoot:
            //    TankShootHandler(packet);
            //    break;
            ////sTankSpecial,
            //case (byte)ServerPackets.sTankSpecial:
            //    TankSpecialHandler(packet);
            //    break;
            ////sTankHealth,
            //case (byte)ServerPackets.sTankHealth:
            //    TankHealthHandler(packet);
            //    break;
            ////sTankDeath,
            //case (byte)ServerPackets.sTankDeath:
            //    TankDeathHandler(packet);
            //    break;
            ////sWinRound,
            //case (byte)ServerPackets.sWinRound:
            //    WinRoundHandler(packet);
            //    break;
            ////sWinGame,
            //case (byte)ServerPackets.sWinGame:
            //    WinGameHandler(packet);
            //    break;
            //sDisconnect
            case (byte)ServerPackets.sDisconnect:
                DisconnectHandler(packet);
                break;
                ////sLeaveRoom
                //case (byte)ServerPackets.sLeaveRoom:
                //    DisconnectHandler(packet);
                //    break;
        }
    }

    //sConnect,
    private static void ConnectHandler(Packet packet)
    {
        try
        {
            Client.instance.id = packet.ReadInt();
            Debug.Log($"Receive id: {Client.instance.id} from server.");
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
            Client.instance.Disconnect();
        }
    }

    //sError,
    private static void ErrorHandler(Packet packet)
    {
        Debug.Log($"Error: {packet.ReadString()}");
        Client.instance.Disconnect();
    }

    //sRoomInfo,
    private static void RoomInfoHandler(Packet packet)
    {
        try
        {
            if (Client.instance.players != null)
            {
                Client.instance.players.Clear();
            }
            else
            {
                Client.instance.players = new List<PlayerInfo>();
            }
            Client.instance.roomId = packet.ReadInt();
            Client.instance.roomName = packet.ReadString();
            Client.instance.hostId = packet.ReadInt();
            int _memberCount = packet.ReadInt();
            for (int i = 0; i < _memberCount; i++)
            {
                Client.instance.players.Add(new PlayerInfo(_playerId: packet.ReadInt(), _username: packet.ReadString(), _team: packet.ReadByte(), _tankId: packet.ReadByte()));
            }

            InRoomManager.instance.ListingPlayer(Client.instance.players);
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }

    //sRoomList,
    private static void RoomListHandler(Packet packet)
    {
        try
        {
            if (Client.instance.rooms != null)
            {
                Client.instance.rooms.Clear();
            }
            else
            {
                Client.instance.rooms = new List<Room>();
            }
            int roomCount = packet.ReadInt();
            for (int i = 0; i < roomCount; i++)
            {
                Client.instance.rooms.Add(new Room(_id: packet.ReadInt(), _name: packet.ReadString(), _numberOfMembers: packet.ReadInt()));
            }

            for (int i = 0; i < roomCount; i++)
            {
                Debug.Log(Client.instance.rooms[i].Name);
            }

            //TODO
            RoomListing.instance.ListingRoom(Client.instance.rooms);

            //Program.JoinRoom();
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }

    //sLeaveRoom,
    private static void LeaveRoomHandler(Packet packet)
    {
        try
        {
            int _clientId = packet.ReadInt();
            foreach (PlayerInfo player in Client.instance.players)
            {
                if (player.playerId == _clientId)
                {
                    Client.instance.players.Remove(player);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }

    //sGameStart,
    private static void GameStartHandler(Packet packet)
    {
        try
        {

        }
        catch
        {

        }
    }
    //sTankPosition,
    private static void TankPositionHandler(Packet packet)
    {
        try
        {

        }
        catch
        {

        }
    }
    //sTankShoot,
    //sTankSpecial,
    //sTankHealth,
    //sTankDeath,
    //sWinRound,
    //sWinGame,
    //sDisconnect
    private static void DisconnectHandler(Packet packet)
    {
        try
        {
            //TODO: host out
            int _client = packet.ReadInt();
            foreach(PlayerInfo player in Client.instance.players)
            {
                if (player.playerId == _client)
                {
                    Client.instance.players.Remove(player);
                    break;
                }
            }
            Debug.Log("Hello");
            InRoomManager.instance.ListingPlayer(Client.instance.players);
            //TODO: Destroy
        }
        catch
        {

        }
    }
}
