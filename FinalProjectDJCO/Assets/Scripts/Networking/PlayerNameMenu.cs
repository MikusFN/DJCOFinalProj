using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerNameMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text _playerName;

    [SerializeField]
    private GameObject _messageBox;

    private LobbyCanvases _lobbyCanvases;

    public void FirstInitialize(LobbyCanvases canvases)
    {
        _lobbyCanvases = canvases;
    }

    public void OnClick_ConnectServer()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = _playerName.text;
        PhotonNetwork.GameVersion = "0.3.0";
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "eu";
        PhotonNetwork.ConnectUsingSettings();
        _messageBox.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
        _lobbyCanvases.CreateOrJoinLobbyCanvas.Show();
        GameObject.Find("BackgroundMusic").GetComponent<BackgroundMusic>().UseAmbient();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnected, reason: " + cause.ToString());
    }
}
