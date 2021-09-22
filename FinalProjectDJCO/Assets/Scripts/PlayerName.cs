using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerName : MonoBehaviourPun
{
    private Transform mainCameraTransform;

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        Player player = this.transform.parent.GetComponent<PhotonView>().Owner;
        if (PhotonNetwork.LocalPlayer == player)
            return;

        this.GetComponent<TextMesh>().text = player.NickName;
        mainCameraTransform = Camera.main.transform;
    }
    
    void LateUpdate()
    {
        if (mainCameraTransform == null)
            return;
        transform.LookAt(transform.position + mainCameraTransform.rotation * Vector3.forward, mainCameraTransform.rotation * Vector3.up);
    }
}
