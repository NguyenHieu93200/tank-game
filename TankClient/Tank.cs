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
        public string tankName;
        public static Tank CreateTank(byte _tankId)
        {
            switch (_tankId)
            {
                case (byte)TankList.Tank1:
                    return new Tank1();
            }
            return null;
        }
        public int TankShoot()
        {
            return damage;
        }

        public abstract void TankSkill();
    }

    public class Tank1 : Tank
    {
        public Tank1()
        {
            damage = 10;
            maxhealth = 100;
            tankName = "tank1";
        }
        public override void TankSkill()
        {
            Console.WriteLine("skill of tank1");
        }
    }

    public class Tank2 : Tank
    {
        public Tank2()
        {
            damage = 20;
            maxhealth = 60;
            tankName = "tank2";
        }
        public override void TankSkill()
        {
            Console.WriteLine("skill of tank2");
        }
    }

    public class Tank3 : Tank
    {
        public Tank3()
        {
            damage = 5;
            maxhealth = 160;
            tankName = "tank3";
        }
        public override void TankSkill()
        {
            Console.WriteLine("skill of tank3");
        }
    }

    public class Tank4 : Tank
    {
        public Tank4()
        {
            damage = 25;
            maxhealth = 50;
            tankName = "tank4";
        }
        public override void TankSkill()
        {
            Console.WriteLine("skill of tank4");
        }
    }
}