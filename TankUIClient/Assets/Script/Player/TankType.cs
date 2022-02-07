using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankType
{
    public static Dictionary<int, string> TankTypes = new Dictionary<int,string>()
    {
        {1, "Balancer" },
        {2, "Tanky" }
    };

    private static TankType instance;

    private TankType() { }

    public static TankType Instance { 
        get
        {
            if (instance == null)
            {
                instance = new TankType();
            }
            return instance;
        } 
    }
    
    public void InitializeSpecialInfo(PlayerManager manager)
    {
        switch (manager.TankType)
        {
            case 1:
                DefaultTankInfo(manager);
                break;
            case 2:
                TankyTankInfo(manager);
                break;
            default:
                DefaultTankInfo(manager);
                break;
        }
    }

    private void DefaultTankInfo(PlayerManager manager)
    {
        manager.MAX_HEALTH = 150f;
        manager.m_TypeColor = new Color(0.57f, 0.91f, 0.74f);       // green
        manager.SpecialFire = delegate()
        {
            Debug.Log("Default Tank SPECIAL MOVE");
        };
    }

    private void TankyTankInfo(PlayerManager manager)
    {
        manager.MAX_HEALTH = 200f;
        manager.m_TypeColor = new Color(0.05f, 0.6f, 0.73f);       // blue
        manager.SpecialFire = delegate ()
        {
            Debug.Log("Tanky Tank SPECIAL MOVE");
        };
    }
}
