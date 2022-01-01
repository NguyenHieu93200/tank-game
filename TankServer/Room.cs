using System.Collections.Generic;

namespace TankServer
{
    internal class Room
    {
        public const int MaxPlayersPerTeam = 2;
        public int roomId;
        public string roomName;
        public int hostId;
        public List<Client>[] teams = new List<Client>[2];
        public bool isInGame;


        public Room()
        {
            roomId = 0;
            teams[0] = new List<Client>();
            teams[1] = new List<Client>();
            hostId = 0;
            isInGame = false;
        }

        public Room(int _id, string _roomName, Client _host)
        {
            roomId = _id;
            hostId = _host.id;
            teams[0] = new List<Client>
            {
                _host
            };
            teams[1] = new List<Client>();
            _host.roomId = roomId;
            _host.teamId = 0;
            _host.tankId = 1;
            isInGame = false;
            roomName = _roomName;
        }

        public void AddClient(Client _client)
        {
            if (teams[0].Count < 2)
            {
                teams[0].Add(_client);
                _client.teamId = 0;
                _client.tankId = 1;
            }
            else
            {
                teams[1].Add(_client);
                _client.teamId = 1;
                _client.tankId = 1;
            }
            _client.roomId = roomId;
        }

        public void ChangeTeam(Client _client, byte _oldTeamId, byte _newTeamId)
        {
            teams[_oldTeamId].Remove(_client);
            teams[_newTeamId].Add(_client);
            _client.teamId = _newTeamId;
        }

        public List<Client> GetClient()
        {
            List<Client> clients = new();
            clients.AddRange(teams[0]);
            clients.AddRange(teams[1]);
            return clients;
        }

        public void RemoveClient(Client _client)
        {
            if (GetClient().Count == 1 || _client.id == hostId)
            {
                CloseRoom();
                return;
            }
            teams[_client.teamId].Remove(_client);
            _client.roomId = 0;
            _client.tankId = 0;
            _client.teamId = 0;
        }

        public void CloseRoom()
        {
            foreach (Client client in GetClient())
            {
                client.roomId = 0;
                client.tankId = 0;
                client.teamId = 0;
            }
            teams[0] = null;
            teams[1] = null;
            teams = null;
            Server.rooms[roomId] = null;
        }
    }
}
