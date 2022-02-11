using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerManager : MonoBehaviour
{
    private Rigidbody m_Rigidbody;              // Reference used to move the tank.
    private TankShooting m_Shooting;
    // Start is called before the first frame update
    public Color m_Team1Color;                             // This is the color this tank will be tinted.
    public Color m_Team2Color;
    public Color m_TypeColor;

    public float MAX_HEALTH = 100f;
    public float HEALTH;
    public float Damage = 20f;

    public int m_PlayerNumber;            // This specifies which player this the manager for.
    public byte teamid;

    public Transform SpawnPoint;

    [HideInInspector] public GameObject m_Instance;         // A reference to the instance of the tank when it is created.
    [HideInInspector] public int m_Wins;                    // The number of wins this player has so far.

    public GameObject m_Shell;                   // Prefab of the shell.
    public Transform m_FireTransform;           // A child of the tank where the shells are spawned.

    public float m_LaunchForce = 50f;         // The force that will be given to the shell when the fire button is released.

    private GameObject m_CanvasGameObject;                  // Used to disable the world space UI during the Starting and Ending phases of each round.

    public bool m_Dead;
    private ParticleSystem m_ExplosionParticles;
    public GameObject m_ExplosionPrefab;
    private AudioSource m_ExplosionAudio;

    [HideInInspector] public SpecialFireDelegate SpecialFire;
    [HideInInspector] public int TankType;
    public Transform m_SelfEffectTransform;

    public float m_BaseAttackTime = 1.0f;
    public float m_SpecialCooldown = 10.0f;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Shooting = GetComponent<TankShooting>();
        m_Rigidbody.maxDepenetrationVelocity = float.PositiveInfinity;

        // Instantiate the explosion prefab and get a reference to the particle system on it.
        m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();

        // Get a reference to the audio source on the instantiated prefab.
        m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();

        // Disable the prefab so it can be activated when it's required.
        m_ExplosionParticles.gameObject.SetActive(false);
    }
    public void Setup()
    {
        this.HEALTH = MAX_HEALTH;
        // Get all of the renderers of the tank.
        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].name == "TankTurret")
            {
                renderers[i].material.color = m_TypeColor;
                continue;
            }
            // ... set their material color to the color specific to this tank.
            if (teamid == 0)
            {
                renderers[i].material.color = m_Team1Color;
            } else
            {
                renderers[i].material.color = m_Team2Color;
            }
        }
        if (m_Shooting) m_Shooting.UpdateAttackTime();
    }

    public void DisableControl()
    {

        m_CanvasGameObject.SetActive(false);
    }


    public void EnableControl()
    {

        m_CanvasGameObject.SetActive(true);
    }

    // Update is called once per frame
    //public void Reset()
    //{
    //    m_Instance.transform.position = m_SpawnPoint.position;
    //    m_Instance.transform.rotation = m_SpawnPoint.rotation;

    //    m_Instance.SetActive(false);
    //    m_Instance.SetActive(true);
    //}

    public void Fire()
    {
        // Create an instance of the shell and store a reference to it's rigidbody.
        GameObject shell = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation);
        shell.GetComponent<ShellExplosion>().m_Damage = Damage; 
        Rigidbody shellInstance = shell.GetComponent<Rigidbody>();
        // Set the shell's velocity to the launch force in the fire position's forward direction.
        shellInstance.velocity = m_LaunchForce * m_FireTransform.forward;

        //// Change the clip to the firing clip and play it.
        //m_ShootingAudio.clip = m_FireClip;
        //m_ShootingAudio.Play();

        //// Reset the launch force.  This is a precaution in case of missing button events.
        //m_CurrentLaunchForce = m_MinLaunchForce;
    }

    public delegate void SpecialFireDelegate();

    public void OnDeath()
    {
        // Set the flag so that this function is only called once.
        m_Dead = true;

        // Move the instantiated explosion prefab to the tank's position and turn it on.
        m_ExplosionParticles.transform.position = transform.position;
        m_ExplosionParticles.gameObject.SetActive(true);

        // Play the particle system of the tank exploding.
        m_ExplosionParticles.Play();

        // Play the tank explosion sound effect.
        m_ExplosionAudio.Play();

        // Turn the tank off.
        gameObject.SetActive(false);
    }

    public void Reset()
    {
        m_Instance.transform.position = SpawnPoint.position;
        m_Instance.transform.rotation = SpawnPoint.rotation;
        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }
}
