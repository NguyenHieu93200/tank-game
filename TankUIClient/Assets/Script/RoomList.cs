using System.Collections;
using System.Collections.Generic;
using TankClient;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomList : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    public Text nameText;
    [SerializeField]
    public Text _members;
    [SerializeField]
    public Text _roomId;

    public void SetRoomInfo(Room roominfo)
    {
        nameText.text = roominfo.Name;
        _members.text = roominfo.NumberOfMembers.ToString() + "/4";
        _roomId.text = roominfo.Id.ToString() ;
    }

    public void OnPointerDown(PointerEventData data)
    {
        Client.instance.roomId = int.Parse(_roomId.text);
        SceneManager.LoadScene(3);
        PacketSender.JoinRoomSender(Client.instance.id, Client.instance.roomId);
        Debug.Log($"You press {_roomId.text} button.");
    }

}
