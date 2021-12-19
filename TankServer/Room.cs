using System;
using System.Collections.Generic;
using System.Text;

namespace TankServer
{
    internal class Room
    {
        public const int MaxPlayers = 4;
        public int roomId;
        public string roomName;
        public int hostId;
        public List<Client> clients;
        public bool isInGame;
        

        public Room()
        {
            roomId = 0;
            clients = new List<Client>();
            hostId = 0;
            isInGame = false;
        }

        public void AddClient(Client _client)
        {
            clients.Add(_client);
        }
    }
}
