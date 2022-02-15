using System.Collections;
using System.Collections.Generic;
using TankClient;
using UnityEngine;

public class TankType : MonoBehaviour
{
    public static string[] TankTypes = 
    {
        "Balancer",
        "Tanky",
        "Tricker",
        "Speed"
    };

    public static TankType instance;

    public GameObject SpecialShellPrefab;
    public GameObject HealingPrefab;
    public GameObject SmokePrefab;
    public GameObject SpeedPrefab;

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
            case 0:
                DefaultTankInfo(manager);
                break;
            case 1:
                TankyTankInfo(manager);
                break;
            case 2:
                TrickerTankInfo(manager);
                break;
            case 3:
                SpeedTankInfo(manager);
                break;
            default:
                DefaultTankInfo(manager);
                break;
        }
    }

    private void DefaultTankInfo(PlayerManager manager)
    {
        manager.MAX_HEALTH = 100f;
        manager.m_SpecialCooldown = 5f;
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
        manager.MAX_HEALTH = 150f;
        manager.Damage = 15f;
        manager.m_SpecialCooldown = 15f;
        manager.m_TypeColor = new Color(0.05f, 0.6f, 1f);       // blue
        manager.SpecialFire = delegate ()
        {
            GameObject healing = Instantiate(HealingPrefab, manager.m_SelfEffectTransform.position, new Quaternion(0.0f, -0.7f, -0.7f, 0.0f));
            if (Client.instance.hostId == Client.instance.id)
            {
                manager.HEALTH = manager.HEALTH + 40 < manager.MAX_HEALTH ? manager.HEALTH + 40 : manager.MAX_HEALTH;
                PacketSender.TankHealthSender(manager.m_PlayerNumber, Client.instance.roomId, manager.HEALTH);
            }
            healing.transform.parent = manager.gameObject.transform;
            Destroy(healing, healing.GetComponent<ParticleSystem>().main.duration);
        };
    }

    private void TrickerTankInfo(PlayerManager manager)
    {
        manager.MAX_HEALTH = 100f;
        manager.Damage = 20f;
        manager.m_TypeColor = new Color(0.5373f, 0.5882f, 0.5451f);       // smoke
        manager.SpecialFire = delegate ()
        {
            // Create an instance of the shell and store a reference to it's rigidbody.
            GameObject shell = Instantiate(SmokePrefab, manager.m_FireTransform.position, manager.m_FireTransform.rotation);
            SmokeExplosion shellExplosion = shell.GetComponent<SmokeExplosion>();
            Rigidbody shellInstance = shell.GetComponent<Rigidbody>();
            // Set the shell's velocity to the launch force in the fire position's forward direction.
            shellInstance.velocity = manager.m_LaunchForce * 0.3f * manager.m_FireTransform.forward;
        };
    }

    private void SpeedTankInfo(PlayerManager manager)
    {
        manager.MAX_HEALTH = 100f;
        manager.Damage = 25f;
        manager.m_TypeColor = new Color(1f, 0.91f, 0f);       // yellow 
        manager.SpecialFire = delegate ()
        {
            Quaternion temp = manager.gameObject.transform.rotation;
            temp.x = temp.w;
            temp.z = -temp.y;
            temp.y = 0;
            temp.w = 0;
            GameObject speed = Instantiate(SpeedPrefab, manager.m_SelfEffectTransform.position, temp);
            speed.transform.parent = manager.gameObject.transform;
            Destroy(speed, 5);
            if (manager.m_PlayerNumber == Client.instance.id)
            {
                manager.gameObject.GetComponent<TankMovement>().m_Speed *= 2;
                StartCoroutine(DelayFadeSpeed(5));
            }
        };

        IEnumerator DelayFadeSpeed(float delayTime)
        {
            //Wait for the specified delay time before continuing.
            yield return new WaitForSeconds(delayTime);

            FadeSpeed();
        }

        void FadeSpeed() {
            if(manager && manager.gameObject)
            manager.gameObject.GetComponent<TankMovement>().m_Speed /= 2;
        }
    }
}
