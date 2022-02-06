using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Rigidbody m_Rigidbody;              // Reference used to move the tank.
    // Start is called before the first frame update
    public Color m_Team1Color;                             // This is the color this tank will be tinted.
    public Color m_Team2Color;
    public float MAX_HEALTH = 100f;
    public float HEALTH;
    public int m_PlayerNumber;            // This specifies which player this the manager for.
    public byte teamid;
    
    [HideInInspector] public GameObject m_Instance;         // A reference to the instance of the tank when it is created.
    [HideInInspector] public int m_Wins;                    // The number of wins this player has so far.
    public Rigidbody m_Shell;                   // Prefab of the shell.
    public Transform m_FireTransform;           // A child of the tank where the shells are spawned.

    public float m_LaunchForce = 35f;         // The force that will be given to the shell when the fire button is released.


    private GameObject m_CanvasGameObject;                  // Used to disable the world space UI during the Starting and Ending phases of each round.
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.maxDepenetrationVelocity = float.PositiveInfinity;
        this.HEALTH = MAX_HEALTH;
    }
    public void Setup()
    {

        // Get all of the renderers of the tank.
        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            // ... set their material color to the color specific to this tank.
            if (teamid == 0)
            {
                renderers[i].material.color = m_Team1Color;
            } else
            {
                renderers[i].material.color = m_Team2Color;
            }
            
        }
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
