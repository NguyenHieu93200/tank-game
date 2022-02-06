using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // team1 spawn
    public Transform m_SpawnPoint1;
    public Transform m_SpawnPoint2;

    //team2 spawn
    public Transform m_SpawnPoint3;
    public Transform m_SpawnPoint4;

    public static GameManager instance;
    public int m_NumRoundsToWin = 3;
    public float m_StartDelay = 3f;
    public float m_EndDelay = 3f;
    public GameObject m_LocalTankPrefab;
    public GameObject m_OtherTankPrefab;
    public Dictionary<int, PlayerManager> m_Tanks = new Dictionary<int, PlayerManager>();
    public Text m_MessageText;



    private int m_RoundNumber;
    private WaitForSeconds m_StartWait;
    private WaitForSeconds m_EndWait;
    private int m_RoundWinner;
    private int m_GameWinner;
    private Transform m_SpawmPoint1;




    // Start is called before the first frame update
    private void Start()
    {
        // Create the delays so they only have to be made once.
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        SpawnAllTanks();

        // Once the tanks have been created and the camera is using them as targets, start the game.
    }

    private void SpawnAllTanks()
    {
        // For all the tanks...

        int team1Count = 0;
        int team2Count = 0;
        foreach (PlayerInfo player in Client.instance.players)
        {
            Debug.Log("Create OTher Tank! ");
            GameObject tank;
            Transform SpawnPoint;
            if(player.team == 0)
            {
                if (team1Count == 0)
                {
                    SpawnPoint = m_SpawnPoint1;
                } else
                {
                    SpawnPoint = m_SpawnPoint2;
                }
                team1Count++;
            } else{
                if (team2Count == 0)
                {
                    SpawnPoint = m_SpawnPoint3;
                }
                else
                {
                    SpawnPoint = m_SpawnPoint4;
                }
                team2Count++;
            }
            if (player.playerId == Client.instance.id)
            {
                tank = Instantiate(m_LocalTankPrefab, SpawnPoint.position, SpawnPoint.rotation);
            }
            else
            {
                tank = Instantiate(m_OtherTankPrefab, SpawnPoint.position, SpawnPoint.rotation);
                
            }
            PlayerManager tankManager = tank.GetComponent<PlayerManager>();
            tankManager.m_Instance = tank;
            tankManager.m_PlayerNumber = player.playerId;
            tankManager.teamid = player.team;
            m_Tanks.Add(player.playerId, tankManager);
            m_Tanks[player.playerId].Setup();
        }
    }


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
}