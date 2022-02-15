using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;
    public InputField usernameField;
    public GameObject IpField;
    public GameObject IpHideButtonImage;
    public GameObject ErrorText;

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

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    public void ShowHideIpField()
    {
        if (IpField.activeInHierarchy)
        {
            IpField.SetActive(false);
        } else
        {
            IpField.SetActive(true);
        }
        IpHideButtonImage.transform.Rotate(180, 0, 0, Space.Self);
    }

    /// <summary>Attempts to connect to the server.</summary>
    public void ConnectToServer()
    {
        string ip = "127.0.0.1";
        if (IpField.activeInHierarchy)
        {
            ip = IpField.GetComponent<InputField>().text;
        }
        Debug.Log(Client.instance.username);
        string username = usernameField.text;
        startMenu.SetActive(false);
        try
        {
            Client.instance.Connect(username, ip);
            SceneManager.LoadScene(1);
        }
        catch (Exception _ex)
        {
            Debug.Log(_ex);
            startMenu.SetActive(true);
            ErrorText.SetActive(true);
        }
    }
}