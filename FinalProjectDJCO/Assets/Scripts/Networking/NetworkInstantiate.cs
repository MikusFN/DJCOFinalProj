using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkInstantiate : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefab;

    private void Awake()
    {
        if (PhotonNetwork.IsConnected == true)
            PhotonNetwork.Instantiate(_prefab.name, new Vector3(0f, 0f, 0f), Quaternion.identity);
    }
}
