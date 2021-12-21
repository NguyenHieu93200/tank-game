using System;

namespace TankClient
{
    internal class Player
    {
        public int playerId;
        public string username;
        public Tank tank;

        public int team;

        public int health;
        public float xPos;
        public float yPos;
        public float angle;

        public Player(int _playerId, string _username, int _tankId)
        {
            playerId = _playerId;
            username = _username;
            tank = Tank.CreateTank(_tankId);
            health = tank.maxhealth;
        }
    }
}