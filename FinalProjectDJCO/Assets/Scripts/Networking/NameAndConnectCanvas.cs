using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameAndConnectCanvas : MonoBehaviour
{
    [SerializeField]
    private PlayerNameMenu _playerNameMenu;

    private LobbyCanvases _lobbyCanvases;

    public void FirstInitialize(LobbyCanvases canvases)
    {
        _playerNameMenu.FirstInitialize(canvases);
        _lobbyCanvases = canvases;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
