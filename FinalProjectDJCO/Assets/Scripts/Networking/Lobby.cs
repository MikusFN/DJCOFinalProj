using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class Lobby : MonoBehaviour
{
    [SerializeField]
    private Text _lobbyName;

    [SerializeField]
    private Text _playerCount;

    public RoomInfo RoomInfo { get; private set; }

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        _lobbyName.text = roomInfo.Name;
        _playerCount.text = roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers;
    }

    public void OnClick_Button()
    {
        PhotonNetwork.JoinRoom(RoomInfo.Name);
    }
}
