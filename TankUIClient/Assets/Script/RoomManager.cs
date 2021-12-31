using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TankClient;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;

    public InputField CreateNameField;
 
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

    public void CreateRoom()
    {
        Client.instance.roomName = CreateNameField.text;
        PacketSender.CreateRoomSender(Client.instance.id, Client.instance.roomName);
    }

    public void JoinRoom()
    {
        PacketSender.RoomListSender();
    }
}
