using UnityEngine;
using UnityEngine.UI;
using TankClient;

public class TankShooting : MonoBehaviour
{
    private string m_FireButton;                // The input axis that is used for launching shells.
    public float m_BaseAttackTime = 1.0f;      // Base Attack Time (BAT)
    private float m_NextAttack = 0.0f;          // Time for the next attack
    private Rigidbody m_Rigidbody;              // Reference used to move the tank.

    private void Start()
    {
        // The fire axis is based on the player number.
        m_FireButton = "Fire1";
    }

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
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
    }
}