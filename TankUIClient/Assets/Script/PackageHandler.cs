using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TankClient;
using UnityEngine.SceneManagement;
using System.Net;

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
            //sTankPosition,
            case (byte)ServerPackets.sTankPosition:
                TankPositionHandler(packet);
                break;
            ////sTankShoot,
            case (byte)ServerPackets.sTankShoot:
                TankShootHandler(packet);
                break;
            ////sTankSpecial,
            //case (byte)ServerPackets.sTankSpecial:
            //    TankSpecialHandler(packet);
            //    break;
            ////sTankHealth,
            case (byte)ServerPackets.sTankHealth:
                TankHealthHandler(packet);
                break;
            ////sTankDeath,
            case (byte)ServerPackets.sTankDeath:
                TankDeathHandler(packet);
                break;
            ////sWinRound,
            case (byte)ServerPackets.sWinRound:
                WinRoundHandler(packet);
                  break;
            ////sWinGame,
            //case (byte)ServerPackets.sWinGame:
            //    WinGameHandler(packet);
            //    break;
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
            Debug.Log($"Receive id: {Client.instance.id} from server.");
            Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
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
        SceneManager.LoadScene(1);
    }

    //sRoomInfo,
    private static void RoomInfoHandler(Packet packet)
    {
        try
        {
            Client.instance.count1 = 0;
            Client.instance.count2 = 0;
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
                if (Client.instance.players[i].playerId == Client.instance.id)
                {
                    Client.instance.team = Client.instance.players[i].team;
                }
                if (Client.instance.players[i].team == 0)
                {
                    Client.instance.count1++;
                } else
                {
                    Client.instance.count2++;
                }

            }
            
            while(true)
            {
                if (SceneManager.GetSceneByBuildIndex(3).isLoaded)
                {
                    InRoomManager.instance.ListingPlayer(Client.instance.players);
                    break;
                }
            }
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
            //TODO: host out
            int _client = packet.ReadInt();
            if (_client == Client.instance.hostId)
            {
                SceneManager.LoadScene(1);
            }
            foreach(PlayerInfo player in Client.instance.players)
            {
                if (player.playerId == _client)
                {
                    if (player.team == 0)
                    {
                        Client.instance.count1--;
                    }
                    else
                    {
                        Client.instance.count2--;
                    }
                    Client.instance.players.Remove(player);
                    break;
                }
            }
            Debug.Log("Hello");

            InRoomManager.instance.ListingPlayer(Client.instance.players);
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
            SceneManager.LoadScene(4);
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }
    //sTankPosition,
    private static void TankPositionHandler(Packet packet)
    {
        try
        {
            int _clientid = packet.ReadInt();
            int _roomid = packet.ReadInt();
            if (Client.instance.id != _clientid){
                float _x = packet.ReadFloat();
                float _z = packet.ReadFloat();
                float _angley = packet.ReadFloat();
                float _anglew = packet.ReadFloat();

                PlayerManager tank = GameManager.instance.m_Tanks[_clientid];
                Rigidbody body = tank.GetComponent<Rigidbody>();

                body.MovePosition(new Vector3(_x, 0, _z));

                body.MoveRotation(new Quaternion(0f, _angley, 0f, _anglew));
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }
    //sTankShoot,
    private static void TankShootHandler(Packet packet)
    {
        try
        {
            int _clientid = packet.ReadInt();
            int _roomid = packet.ReadInt();

                float _x = packet.ReadFloat();
                float _z = packet.ReadFloat();
                float _angley = packet.ReadFloat();
                float _anglew = packet.ReadFloat();

                PlayerManager tank = GameManager.instance.m_Tanks[_clientid];
                Rigidbody body = tank.GetComponent<Rigidbody>();

                body.MovePosition(new Vector3(_x, 0, _z));

                body.MoveRotation(new Quaternion(0f, _angley, 0f, _anglew));

                tank.Fire();

        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }
    //sTankSpecial,
    //sTankHealth,
    private static void TankHealthHandler(Packet packet)
        {
        try
        {
            //TODO: host out
            int _client = packet.ReadInt();
            int _roomId = packet.ReadInt();
            float _heath = packet.ReadFloat();

            PlayerManager tank = GameManager.instance.m_Tanks[_client];
            tank.HEALTH = _heath;
        }
        catch
        {

        }

    }
    //sTankDeath,
    private static void TankDeathHandler(Packet packet)
    {
        try
        {
            //TODO: host out
            int _client = packet.ReadInt();
            int _roomId = packet.ReadInt();

            PlayerManager tank = GameManager.instance.m_Tanks[_client];
            tank.OnDeath();
               
            if(Client.instance.id == Client.instance.hostId) {
           if (GameManager.instance.CheckTeamWinRound(out int winner))
           {
             Debug.Log($"Winner: {winner}");
             PacketSender.WinRoundSender(Client.instance.roomId, (byte)(winner));
           }
           }
        }
        catch
        {

        }
    }
    //sWinRound,
    private static void WinRoundHandler(Packet packet)
    {
        int _room = packet.ReadInt();
        int _team = packet.ReadByte();
        Debug.Log($"Winner: {_team}");

        GameManager.instance.Reset();


    }    
        //sWinGame,
    //sDisconnect
    private static void DisconnectHandler(Packet packet)
    {
        try
        {
            //TODO: host out
            int _client = packet.ReadInt();
            if (_client == Client.instance.hostId)
            {
                SceneManager.LoadScene(1);
            }
            foreach(PlayerInfo player in Client.instance.players)
            {
                if (player.playerId == _client)
                {
                    if (player.team == 0)
                    {
                        Client.instance.count1--;
                    }
                    else
                    {
                        Client.instance.count2--;
                    }
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
