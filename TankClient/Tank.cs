using System;
using System.Collections.Generic;
using System.Text;

namespace TankClient
{
    public enum TankList
    {
        Tank1,
        Tank2,
        Tank3,
        Tank4
    }

    public abstract class Tank
    {
        public int damage;
        public int maxhealth;
        public float Speed;
        public float TurnSpeed;       
        public static Tank CreateTank(int _tankId)
        {
            switch (_tankId)
            {
                case (int)TankList.Tank1:
                    return new Tank1();
            }
            return null;
        }
        public int TankShoot()
        {
            return damage;
        };

        public abstract void TankSkill();
    }

    public class Tank1 : Tank
    {
        public string tankName = "Tank1";
        public Tank1()
        {
            damage = 10;
            maxhealth = 100;
        }
        public override void TankSkill()
        {
            Console.WriteLine("skill of tank1");
        }
    }
}