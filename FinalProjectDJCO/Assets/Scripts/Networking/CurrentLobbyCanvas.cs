using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentLobbyCanvas : MonoBehaviour
{
    [SerializeField]
    private PlayerListMenu _playerListMenu;
    [SerializeField]
    private LeaveLobbyMenu _leaveLobbyMenu;
    public LeaveLobbyMenu LeaveLobbyMenu { get { return _leaveLobbyMenu; } }

    private LobbyCanvases _lobbyCanvases;

    public void FirstInitialize(LobbyCanvases canvases)
    {
        _lobbyCanvases = canvases;
        _playerListMenu.FirstInitialize(canvases);
        _leaveLobbyMenu.FirstInitialize(canvases);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
