using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCanvases : MonoBehaviour
{
    [SerializeField]
    private NameAndConnectCanvas _nameAndConnectCanvas;

    public NameAndConnectCanvas NameAndConnectCanvas { get { return _nameAndConnectCanvas; } }

    [SerializeField]
    private CreateOrJoinLobbyCanvas _createOrJoinLobbyCanvas;

    public CreateOrJoinLobbyCanvas CreateOrJoinLobbyCanvas { get { return _createOrJoinLobbyCanvas; } }

    [SerializeField]
    private CurrentLobbyCanvas _currentLobbyCanvas;

    public CurrentLobbyCanvas CurrentLobbyCanvas { get { return _currentLobbyCanvas; } }

    private void Awake()
    {
        FirstInitialize();
    }

    private void FirstInitialize()
    {
        NameAndConnectCanvas.FirstInitialize(this);
        CreateOrJoinLobbyCanvas.FirstInitialize(this);
        CurrentLobbyCanvas.FirstInitialize(this);
    }
}
