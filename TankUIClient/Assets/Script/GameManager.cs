using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TankClient;
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
    public Text Score;
    public Text team1;
    public Text team2; 
    public int team1Count = 0;
    public int team2Count = 0;
    public int team1Score = 0;
    public int team2Score = 0;
    private int m_RoundNumber;
    public bool isRoundEnd = false;
    private int m_StartWait = 3;
    private int m_RoundScore = 0;
    public GameObject DeathCamera;
    public  int isEnd = 0;


    // Start is called before the first frame update
    private void Start()
    {   
        m_MessageText.gameObject.SetActive(false);
        DeathCamera.SetActive(true);
        // Create the delays so they only have to be made once.
        StartCoroutine(DelaySpawn(m_StartWait));
        // Once the tanks have been created and the camera is using them as targets, start the game.
    }

    private void SpawnAllTanks()
    {
        // For all the tanks...
        m_RoundScore++;
        team1Count = 0;
        team2Count = 0;
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
            tankManager.m_Dead = false;
            tankManager.SpawnPoint = SpawnPoint; // add spawn point
            tankManager.Name = player.username;

            tankManager.TankType = player.tank;

            TankType.instance.InitializeSpecialInfo(tankManager);

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

    public bool CheckTeamWinRound(out int team)
    {
        StartCoroutine(Wait(0.1f));
        int team1Remain = 0, team2Remain = 0;

        foreach (PlayerManager tank in m_Tanks.Values)
        {
            if (tank.teamid == 0 && tank.m_Dead==false)
            {
                team1Remain++;
            } else if (tank.teamid == 1 && tank.m_Dead==false)
            {
                team2Remain++;
            }
        }

        team = -1;
        if (team1Remain == 0)
        {
            team = 1;
            return true;
        }
        if (team2Remain == 0)
        {
            team = 0;
            return true;
        }
        return false;
    }
    public void WinRoundHandler(int team)
    {
        if ( team1Score < 3 || team2Score < 3) { 
            m_MessageText.text = "TEAM " + (team+1) +  " WIN ROUND!";
            m_MessageText.gameObject.SetActive(true);
        }
    }

    public void HostOut()
    {
        m_MessageText.text = "Host out!!";
        m_MessageText.gameObject.SetActive(true);
        StartCoroutine(ExitGame(m_StartDelay));
    }
    public void CheckWinGame()
    {
        if(team1Score == 3 || Client.instance.count2 == 0)
        {
            isEnd = 1;
            PacketSender.WinGameSender(Client.instance.roomId, 0);
        }
        else if(team2Score == 3 || Client.instance.count1 == 0)
        {
            isEnd = 1;
            PacketSender.WinGameSender(Client.instance.roomId, 1);
        }

    }
    public void WinGame(int team)
    {
        m_MessageText.text = "TEAM " + (team + 1) + " WIN GAME!";
        m_MessageText.gameObject.SetActive(true);
        StartCoroutine(ExitGame(m_StartDelay));
    }

    public void Reset()
    {
        foreach (PlayerManager tank in m_Tanks.Values)
        {
           Destroy(tank.m_Instance);
        }
        m_Tanks.Clear();
        DeathCamera.SetActive(true);
        StartCoroutine(DelaySpawn(m_StartWait));
    }

    IEnumerator DelaySpawn(float delayTime)
    {
        if(m_RoundScore == 0 )
        {
            m_MessageText.gameObject.SetActive(true);
            m_MessageText.text = "START GAME!";
        }
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);

        SpawnAllTanks();
        isRoundEnd = false;
        m_MessageText.gameObject.SetActive(false);
        DeathCamera.SetActive(false);
    }

    public void LateUpdate()
    {
        Score.text = m_RoundScore +"";
        team1.text =  "" + team1Score;
        team2.text = "" + team2Score;
    }

    IEnumerator ExitGame(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        
        SceneManager.LoadScene(1);
    }
    // check
}
    
