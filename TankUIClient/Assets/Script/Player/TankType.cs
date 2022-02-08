using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankType : MonoBehaviour
{
    public static Dictionary<int, string> TankTypes = new Dictionary<int,string>()
    {
        {1, "Balancer" },
        {2, "Tanky" }
    };

    public static TankType instance;

    public GameObject SpecialShellPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
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
        manager.MAX_HEALTH = 100f;
        manager.m_SpecialCooldown = 3f;
        manager.m_TypeColor = new Color(0.57f, 0.91f, 0.74f);       // green
        manager.SpecialFire = delegate()
        {
            // Create an instance of the shell and store a reference to it's rigidbody.
            GameObject shell = Instantiate(SpecialShellPrefab, manager.m_FireTransform.position, manager.m_FireTransform.rotation);
            ShellExplosion shellExplosion = shell.GetComponent<ShellExplosion>();
            shellExplosion.m_Damage = manager.Damage*2;
            Rigidbody shellInstance = shell.GetComponent<Rigidbody>();
            // Set the shell's velocity to the launch force in the fire position's forward direction.
            shellInstance.velocity = manager.m_LaunchForce * 1.5f * manager.m_FireTransform.forward;

            //// Change the clip to the firing clip and play it.
            //m_ShootingAudio.clip = m_FireClip;
            //m_ShootingAudio.Play();

            //// Reset the launch force.  This is a precaution in case of missing button events.
            //m_CurrentLaunchForce = m_MinLaunchForce;
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
