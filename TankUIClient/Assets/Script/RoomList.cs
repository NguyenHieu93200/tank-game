using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomList : MonoBehaviour
{
    [SerializeField]
    public Text nameText;
    [SerializeField]
    public Text _members;
    [SerializeField]
    public Text _roomid;

    public void SetRoomInfo(Room roominfo)
    {
        nameText.text = roominfo.Name;
        _members.text = roominfo.NumberOfMembers.ToString();
        _roomid.text = roominfo.Id.ToString() ;
    }

}
