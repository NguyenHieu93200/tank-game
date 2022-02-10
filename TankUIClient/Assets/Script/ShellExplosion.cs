using UnityEngine;
using Packages;
using System.Collections.Generic;
using TankClient;

public class ShellExplosion : MonoBehaviour
{
    public LayerMask m_TankMask;                        // Used to filter what the explosion affects, this should be set to "Players".
    public ParticleSystem m_ExplosionParticles;         // Reference to the particles that will play on explosion.
    public AudioSource m_ExplosionAudio;                // Reference to the audio that will play on explosion.
    public float m_Damage;
    public float m_ExplosionForce = 5f;              // The amount of force added to a tank at the centre of the explosion.
    public float m_MaxLifeTime = 10f;                    // The time in seconds before the shell is removed.
    public float m_ExplosionRadius = 5f;                // The maximum distance away from the explosion tanks can be and are still affected.
    private int hit = 1;

    private void Start()
    {
        // If it isn't destroyed by then, destroy the shell after it's lifetime.
        Destroy(gameObject, m_MaxLifeTime);
    }

    [System.Obsolete]
    private void OnTriggerEnter(Collider other)
    {
        if (hit == 0)
        {
            return;
        }
        hit--;
        // Collect all the colliders in a sphere from the shell's current position to a radius of the explosion radius.
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);

        // Go through all the colliders...
        if (Client.instance.id == Client.instance.hostId)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                // ... and find their rigidbody.
                Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

                // If they don't have a rigidbody, go on to the next collider.
                if (!targetRigidbody)
                    continue;

                // Add an explosion force.

                //// Find the TankHealth script associated with the rigidbody.
                //TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();

                PlayerManager player = targetRigidbody.GetComponent<PlayerManager>();
                if (!player)
                {
                    continue;
                }
                
                if( player.HEALTH <= 0) 
                {
                    continue;
                }
                //targetRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

                //// If there is no TankHealth script attached to the gameobject, go on to the next collider.
                //if (!targetHealth)
                //    continue;

                //// Calculate the amount of damage the target should take based on it's distance from the shell.
                float damage = CalculateDamage();
                player.HEALTH -= damage;
                Debug.Log($"Player {player.m_PlayerNumber} Take {damage}");
                if(player.HEALTH <= 0)
                {   
                    Debug.Log("A player died");

                    PacketSender.TankDeathSender(player.m_PlayerNumber,Client.instance.roomId);

                }
                PacketSender.TankHealthSender(player.m_PlayerNumber,Client.instance.roomId,player.HEALTH);   

                
                //// Deal this damage to the tank.
                //targetHealth.TakeDamage(damage);
            }
        }

        // Unparent the particles from the shell.
        m_ExplosionParticles.transform.parent = null;

        // Play the particle system.
        m_ExplosionParticles.Play();

        // Play the explosion sound effect.
        m_ExplosionAudio.Play();

        // Once the particles have finished, destroy the gameobject they are on.
        Destroy(m_ExplosionParticles.gameObject, m_ExplosionParticles.main.duration);

        // Destroy the shell.
        Destroy(gameObject);
    }


    private float CalculateDamage()
    {
        return m_Damage;
    }
}