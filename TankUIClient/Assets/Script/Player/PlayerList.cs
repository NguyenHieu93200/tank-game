using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TankClient;
using UnityEngine;
using UnityEngine.UI;

public class PlayerList : MonoBehaviour
{
    [SerializeField]
    public Text _username;
    [SerializeField]
    public Text _tankname;
    public Dropdown _dropdown;

    private void Start()
    {
        _dropdown.onValueChanged.AddListener(delegate {
            PacketSender.InfoChangeSender(Client.instance.id, Client.instance.roomId, Client.instance.team, (byte)_dropdown.value);
        });
    }

    public void SetPlayerInfo(PlayerInfo player )
    {
        _username.text = player.username;
        if (player.playerId == Client.instance.id)
        {
            _tankname.gameObject.SetActive(false);
            _dropdown.gameObject.SetActive(true);
            _dropdown.ClearOptions();
            _dropdown.AddOptions(TankType.TankTypes.ToList());
            _dropdown.value = Client.instance.tank;
            return;
        }
        _tankname.gameObject.SetActive(true);
        _dropdown.gameObject.SetActive(false);
        _tankname.text = TankType.TankTypes[player.tank];
    }

}   
