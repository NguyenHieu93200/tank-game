using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;
    public InputField usernameField;
    public GameObject InputName;

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

    /// <summary>Attempts to connect to the server.</summary>
    public void ConnectToServer()
    {

        
        Debug.Log("Hello");
        Debug.Log(Client.instance.username);
        startMenu.SetActive(false);
        usernameField.interactable = false;
        Debug.Log("Name");
        Client.instance.Connect();
    }
}