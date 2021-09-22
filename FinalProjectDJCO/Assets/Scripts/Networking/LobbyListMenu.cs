using Photon.Realtime;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyListMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private Lobby _lobby;
    [SerializeField]
    private Text _lobbyCount;

    private List<Lobby> _lobbies = new List<Lobby>();
    private LobbyCanvases _lobbyCanvases;

    public void FirstInitialize(LobbyCanvases canvases)
    {
        _lobbyCanvases = canvases;
    }

    public override void OnJoinedRoom()
    {
        _lobbyCanvases.CurrentLobbyCanvas.Show();
        _content.DestroyChildren();
        _lobbies.Clear();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList)
            {
                int index = _lobbies.FindIndex(x => x.RoomInfo.Name == info.Name);
                if (index != -1)
                {
                    Destroy(_lobbies[index].gameObject);
                    _lobbies.RemoveAt(index);
                }
            }
            else
            {
                int index = _lobbies.FindIndex(x => x.RoomInfo.Name == info.Name);
                if (index == -1)
                {
                    Lobby lobby = Instantiate(_lobby, _content);
                    if (lobby != null)
                    {
                        lobby.SetRoomInfo(info);
                        _lobbies.Add(lobby);
                    }
                }
                else
                    _lobbies[index].SetRoomInfo(info);
            }
        }
        if (_lobbies.Count == 1)
            _lobbyCount.text = _lobbies.Count + " Lobby";
        else
            _lobbyCount.text = _lobbies.Count + " Lobbies";

    }
}
