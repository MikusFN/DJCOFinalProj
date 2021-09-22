using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{

    private Camera cam;
    private PlayerMovementPast player;

    public Camera Cam { get => cam; }

    private void Awake()
    {
        cam = GetComponent<Camera>();
        player = GetComponentInParent<PlayerBehaviour>().GetComponentInChildren<PlayerMovementPast>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + new Vector3(0,1.5f,0);

        Vector3 currentRot = player.transform.localEulerAngles;
        currentRot.x = player.CameraPitch1;
        transform.localEulerAngles = currentRot;        
    }
}
