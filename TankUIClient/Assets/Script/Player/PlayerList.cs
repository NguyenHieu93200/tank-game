using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerList : MonoBehaviour
{
    [SerializeField]
    public Text _username;
    [SerializeField]
    public Text _tankname;


    public void SetPlayerInfo(PlayerInfo player )
    {
        _username.text = player.username;
        _tankname.text = "Default";
    }

}   
