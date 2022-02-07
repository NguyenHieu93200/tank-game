using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
        public int playerId;
        public string username;
        public int tank;

        public byte team;

        public PlayerInfo(int _playerId, string _username, byte _team, byte _tankId)
        {
            playerId = _playerId;
            username = _username;
            tank = _tankId;
            //health = tank.maxhealth;
            team = _team;
        }

        public void Print()
        {
            Debug.Log($"Player: {username} - team: {team}");
        }
}
