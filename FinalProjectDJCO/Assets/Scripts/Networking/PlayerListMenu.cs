using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private PlayerListing _playerListing;
    [SerializeField]
    private GameObject _readyUpButton;
    [SerializeField]
    private GameObject _readyIcon;
    [SerializeField]
    private GameObject _notReadyIcon;
    [SerializeField]
    private GameObject _startGameButton;
    [SerializeField]
    private GameObject _changeLevelButton;
    [SerializeField]
    private LevelSelect _levelSelection;

    private List<PlayerListing> _players = new List<PlayerListing>();
    private LobbyCanvases _lobbyCanvases;
    private bool _ready = false;

    public override void OnEnable()
    {
        base.OnEnable();
        if (PhotonNetwork.IsMasterClient)
        {
            _readyUpButton.SetActive(false);
            _startGameButton.SetActive(true);
            _changeLevelButton.SetActive(true);
            SetReadyUp(true);
        }
        else
        {
            _startGameButton.SetActive(false);
            _readyUpButton.SetActive(true);
            _changeLevelButton.SetActive(false);
            SetReadyUp(false);
        }
        GetCurrentRoomPlayers();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        for (int i = 0; i < _players.Count; i++)
            Destroy(_players[i].gameObject);

        _players.Clear();
    }

    public void FirstInitialize(LobbyCanvases canvases)
    {
        _lobbyCanvases = canvases;
    }

    private void SetReadyUp(bool state)
    {
        _ready = state;
        if (_ready)
        {
            _readyIcon.SetActive(true);
            _notReadyIcon.SetActive(false);
        }
        else
        {
            _readyIcon.SetActive(false);
            _notReadyIcon.SetActive(true);
        }
    }

    private void GetCurrentRoomPlayers()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        if (PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null)
            return;

        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerListing(playerInfo.Value);
        }
    }

    private void AddPlayerListing(Player player)
    {
        int index = _players.FindIndex(x => x.Player == player);
        if (index != -1)
        {
            _players[index].SetPlayerInfo(player);
        }
        else
        {
            PlayerListing listing = Instantiate(_playerListing, _content);
            if (listing != null)
            {
                listing.SetPlayerInfo(player);
                _players.Add(listing);
            }
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        _lobbyCanvases.CurrentLobbyCanvas.LeaveLobbyMenu.OnClick_LeaveRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerListing(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = _players.FindIndex(x => x.Player == otherPlayer);
        if (index != -1)
        {
            Destroy(_players[index].gameObject);
            _players.RemoveAt(index);
        }
    }

    public void OnClick_StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < _players.Count; i++)
            {
                if (_players[i].Player != PhotonNetwork.LocalPlayer)
                {
                    if (!_players[i].Ready)
                        return;
                }
            }
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            int level = _levelSelection.level;
            GameObject.Find("BackgroundMusic").GetComponent<BackgroundMusic>().ChangeMusic();
            base.photonView.RPC("RPC_ChangeMusic", RpcTarget.Others);
            PhotonNetwork.LoadLevel(level);
        }
    }

    public void OnClick_ReadyUp()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            SetReadyUp(!_ready);
            int index = _players.FindIndex(x => x.Player == PhotonNetwork.LocalPlayer);
            if (index != -1)
            {
                _players[index].Ready = _ready;
                _players[index].UpdateReadyIcon();
            }
            base.photonView.RPC("RPC_ChangeReadyState", RpcTarget.Others, PhotonNetwork.LocalPlayer, _ready);
        }
    }

    [PunRPC]
    private void RPC_ChangeReadyState(Player player, bool ready)
    {
        int index = _players.FindIndex(x => x.Player == player);
        if (index != -1)
        {
            _players[index].Ready = ready;
            _players[index].UpdateReadyIcon();
        }
    }

    [PunRPC]
    private void RPC_ChangeMusic()
    {
        GameObject.Find("BackgroundMusic").GetComponent<BackgroundMusic>().ChangeMusic();
    }
}
