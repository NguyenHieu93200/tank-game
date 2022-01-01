using System.Collections;
using System.Collections.Generic;
using TankClient;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomListing : MonoBehaviour
{
    [SerializeField]
    public Transform _content;
    [SerializeField]
    public RoomList _roomListing;
    


    public static RoomListing instance;

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
        PacketSender.RoomListSender();
    }
    public void ListingRoom(List<Room> roomlist)
    {
        foreach (Room info in roomlist)
        {
            RoomList listing = (RoomList)Instantiate(_roomListing, _content);
            if (listing != null) listing.SetRoomInfo(info);
        }
    }

    public void Refresh()
    {
        SceneManager.LoadScene(2);
    }

    public void Back()
    {
        SceneManager.LoadScene(1);
    }

}
