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
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        foreach (PlayerManager _pm in gameManager.m_Tanks.Values)
        {
            if (_pm.teamid == Client.instance.team && _pm.m_PlayerNumber != Client.instance.id)
            {
                FriendName.text = _pm.Name;
                FriendHealth.maxValue = _pm.MAX_HEALTH;
                FriendHealth.value = _pm.HEALTH;
            }
        }
    }
}
