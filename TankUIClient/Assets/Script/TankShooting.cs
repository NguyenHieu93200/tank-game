using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;              // Used to identify the different players.
    public Rigidbody m_Shell;                   // Prefab of the shell.
    public Transform m_FireTransform;           // A child of the tank where the shells are spawned.
    
    public float m_LaunchForce = 35f;         // The force that will be given to the shell when the fire button is released.

    private string m_FireButton;                // The input axis that is used for launching shells.
    private float m_BaseAttackTime = 1.0f;      // Base Attack Time (BAT)
    private float m_NextAttack = 0.0f;          // Time for the next attack


    private void Start()
    {
        // The fire axis is based on the player number.
        m_FireButton = "Fire1";
    }


    private void Update()
    {
        if (Input.GetButtonUp(m_FireButton) && m_NextAttack <= Time.time)
        {
            // ... launch the shell.
            Fire();
        }
    }


    private void Fire()
    {
        // Set the fired flag so only Fire is only called once.
        m_NextAttack = Time.time + m_BaseAttackTime;
        // Create an instance of the shell and store a reference to it's rigidbody.
        Rigidbody shellInstance =
            Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        // Set the shell's velocity to the launch force in the fire position's forward direction.
        shellInstance.velocity = m_LaunchForce * m_FireTransform.forward;

        //// Change the clip to the firing clip and play it.
        //m_ShootingAudio.clip = m_FireClip;
        //m_ShootingAudio.Play();

        //// Reset the launch force.  This is a precaution in case of missing button events.
        //m_CurrentLaunchForce = m_MinLaunchForce;
    }
}