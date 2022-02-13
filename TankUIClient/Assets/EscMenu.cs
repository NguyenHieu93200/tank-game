using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EscMenu : MonoBehaviour
{
    public static bool IsPaused = false;
    // Start is called before the first frame update

    public GameObject EscMenuUI;
    void Start()
    {
        EscMenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)
            {
                Resume();
            } else
            {
                Paused();
            }
        }
    }


     public void Paused()
    {
        EscMenuUI.SetActive(true);
        IsPaused = true;
    }
    
    public void Resume()
    {
        EscMenuUI.SetActive(false);
        IsPaused = false;
    }

    public void Quit()
    {
        IsPaused = false;
        SceneManager.LoadScene(1);
    }
 
}
