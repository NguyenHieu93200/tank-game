using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Rigidbody m_Rigidbody;              // Reference used to move the tank.
    // Start is called before the first frame update
    public Color m_PlayerColor;                             // This is the color this tank will be tinted.
    [HideInInspector] public int m_PlayerNumber;            // This specifies which player this the manager for.
    
    [HideInInspector] public GameObject m_Instance;         // A reference to the instance of the tank when it is created.
    [HideInInspector] public int m_Wins;                    // The number of wins this player has so far.

   
    private GameObject m_CanvasGameObject;                  // Used to disable the world space UI during the Starting and Ending phases of each round.
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.maxDepenetrationVelocity = float.PositiveInfinity;
    }
    public void Setup()
    {

        // Get all of the renderers of the tank.
        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            // ... set their material color to the color specific to this tank.
            renderers[i].material.color = m_PlayerColor;
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
}
