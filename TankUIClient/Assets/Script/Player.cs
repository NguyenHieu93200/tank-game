using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
        public int playerId;
        public string username;
        //public Tank tank;

        public byte team;

        public int health;
        public float xPos;
        public float yPos;
        public float angle;

        public Player(int _playerId, string _username, byte _team, byte _tankId)
        {
            playerId = _playerId;
            username = _username;
            // tank = Tank.CreateTank(_tankId);
            //health = tank.maxhealth;
            team = _team;
        }

        public void Print()
        {
            //Console.WriteLine($"Player: {username} - tank: {tank.tankName} - team: {team}");
        }
}
