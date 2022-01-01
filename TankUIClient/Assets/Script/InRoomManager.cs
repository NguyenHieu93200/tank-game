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
        SceneManager.LoadScene(1);
        PacketSender.LeaveRoomSender(Client.instance.id, Client.instance.roomId);
    }

    public void Swap()
    {
        if (Client.instance.team == 0)
        {
            if (Client.instance.count2 < 2)
            {
                PacketSender.InfoChangeSender(Client.instance.id, Client.instance.roomId, 1, 1);
                return;
            }
        } 
        else
        {
            if ( Client.instance.count1 < 2 )
            {
                PacketSender.InfoChangeSender(Client.instance.id, Client.instance.roomId, 0, 1);
                return;
            }
        }
        //TODO: error handler
    } 

    public void StartGame()
    {
        PacketSender.StartGameSender(Client.instance.roomId);
    }
}
