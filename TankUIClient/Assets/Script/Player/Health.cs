using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Slider playerSlider2D;
    private PlayerManager m_playerManager;

    // Start is called before the first frame update
    private void Awake()
    {
        m_playerManager = GetComponent<PlayerManager>();
        playerSlider2D = GetComponent<Slider>();
    }
    void Start()
    {
            playerSlider2D.maxValue = m_playerManager.MAX_HEALTH;
            playerSlider2D.value = playerSlider2D.maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        HealthUpdate();
    }
    void HealthUpdate()
    {
        if (m_playerManager)
        {
            playerSlider2D.value = m_playerManager.HEALTH;
        }
    }

}
