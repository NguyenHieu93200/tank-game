using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FriendInfo : MonoBehaviour
{
    [SerializeField]
    public Text FriendName;
    [SerializeField]
    public Slider FriendHealth;
    private GameManager gameManager;
    private int count;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Client.instance.team == 0)
        {
            count = gameManager.team1Count;
        }
        else count = gameManager.team2Count;
        if (count >= 2) {
            foreach (PlayerManager _pm in gameManager.m_Tanks.Values)
            {
                if (_pm.teamid == Client.instance.team && _pm.m_PlayerNumber != Client.instance.id)
                {
                FriendName.text = _pm.Name;
                FriendHealth.maxValue = _pm.MAX_HEALTH;
                FriendHealth.value = _pm.HEALTH;
                FriendHealth.gameObject.SetActive(true);
                }
            }
        }else
        {
            FriendName.text = "";
            FriendHealth.gameObject.SetActive(false);
        }
    }
}
