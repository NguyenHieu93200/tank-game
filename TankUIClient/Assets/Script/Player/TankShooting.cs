using UnityEngine;
using UnityEngine.UI;
using TankClient;

public class TankShooting : MonoBehaviour
{
    private string m_FireButton;                // The input axis that is used for launching shells.
    private string m_SpecialButton;
    private float m_BaseAttackTime = 1.0f;      // Base Attack Time (BAT)
    private float m_NextAttack = 0.0f;          // Time for the next attack
    private float m_SpecialCooldown = 10.0f;      // Special Skill Cooldown
    private float m_NextSpecial = 0.0f;          // Time for the next attack
    private Rigidbody m_Rigidbody;              // Reference used to move the tank.
    private PlayerManager m_PlayerManager;

    private void Start()
    {
        // The fire axis is based on the player number.
        m_FireButton = "Fire1";
        m_SpecialButton = "Fire2";
    }

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_PlayerManager = GetComponent<PlayerManager>();
        m_BaseAttackTime = m_PlayerManager.m_BaseAttackTime;
        m_SpecialCooldown = m_PlayerManager.m_SpecialCooldown;
    }

    public void UpdateAttackTime()
    {
        m_BaseAttackTime = m_PlayerManager.m_BaseAttackTime;
        m_SpecialCooldown = m_PlayerManager.m_SpecialCooldown;
    }

    private void Update()
    {
        if (Input.GetButtonUp(m_FireButton) && m_NextAttack <= Time.time)
        {
            // ...send shoot message
            float x, z, angley, anglew;
            
            x = m_Rigidbody.position.x;
            z = m_Rigidbody.position.z;
            angley = m_Rigidbody.rotation.y;
            anglew = m_Rigidbody.rotation.w;

            PacketSender.TankShootSender(Client.instance.id, Client.instance.roomId, x, z, angley, anglew);
            m_NextAttack = Time.time + m_BaseAttackTime;
        }

        if (Input.GetButtonUp(m_SpecialButton) && m_NextSpecial <= Time.time)
        {
            // ...send shoot message
            float x, z, angley, anglew;

            x = m_Rigidbody.position.x;
            z = m_Rigidbody.position.z;
            angley = m_Rigidbody.rotation.y;
            anglew = m_Rigidbody.rotation.w;

            PacketSender.TankSpecialSender(Client.instance.id, Client.instance.roomId, x, z, angley, anglew);
            m_NextSpecial = Time.time + m_SpecialCooldown;
        }
    }
}