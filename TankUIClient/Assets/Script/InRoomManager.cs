using System.Collections;
using System.Collections.Generic;
using TankClient;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InRoomManager : MonoBehaviour
{
    public static InRoomManager instance;
    [SerializeField]
    public Transform _panel1;
    [SerializeField]
    public Transform _panel2;
    [SerializeField]
    public PlayerList _playerListing;

    public Text RoomName;

    private void Start()
    {
        RoomName.text = Client.instance.roomName;
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

    public void ListingPlayer(List<PlayerInfo> players)
    {
        foreach (Transform child in _panel1)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in _panel2)
        {
            Destroy(child.gameObject);
        }
        foreach (PlayerInfo info in players)
        {

            PlayerList listing;
            if (info.team == 0)
            {
                listing = (PlayerList)Instantiate(_playerListing, _panel1);
            }
            else
            {
                listing = (PlayerList)Instantiate(_playerListing, _panel2);
            }
            if (listing != null) listing.SetPlayerInfo(info);
        }
    }

    public void Back()
    {
        //TODO: out room
        SceneManager.LoadScene(1);
    }
}
