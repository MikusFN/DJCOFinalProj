using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class PlayerListing : MonoBehaviour
{
    [SerializeField]
    private Text _text;

    [SerializeField]
    private GameObject _readyIcon;
    [SerializeField]
    private GameObject _notReadyIcon;
    [SerializeField]
    private GameObject _hostIcon;

    public Player Player { get; private set; }
    public bool Ready = false;
    public bool Host = false;

    public void SetPlayerInfo(Player player)
    {
        Player = player;
        _text.text = player.NickName;
        if (player.IsMasterClient)
        {
            SetHost();
        }
    }

    public void SetHost()
    {
        _readyIcon.SetActive(false);
        _notReadyIcon.SetActive(false);
        _hostIcon.SetActive(true);
        Host = true;
    }

    public void UpdateReadyIcon()
    {
        if (Host)
            return;
        if (Ready)
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
}
