using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateOrJoinLobbyCanvas : MonoBehaviour
{
    [SerializeField]
    private CreateLobbyMenu _createLobbyMenu;
    [SerializeField]
    private LobbyListMenu _lobbyListMenu;

    private LobbyCanvases _lobbyCanvases;

    public void FirstInitialize(LobbyCanvases canvases)
    {
        _lobbyCanvases = canvases;
        _createLobbyMenu.FirstInitialize(canvases);
        _lobbyListMenu.FirstInitialize(canvases);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
