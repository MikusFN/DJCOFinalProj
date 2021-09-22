using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform cameraPosition = null;
    [SerializeField] Camera cam = null;
    [SerializeField] GameObject checkpointArrow = null;

    private float targetFov;
    private float currentFov;
    private Players playerNames;

    public float TargetFov { get => targetFov; set => targetFov = value; }

    private void Awake()
    {
        targetFov = cam.fieldOfView;
        currentFov = targetFov;
        playerNames = UnityEngine.Object.FindObjectOfType<Players>();
        if (playerNames && playerNames.CanAddPlayer)
            playerNames.AddPlayer("Player " + (playerNames.PlayerCount + 1), this.gameObject);
    }
    private void Start()
    {
        if (cam && playerNames)
        cam.cullingMask = cam.cullingMask ^ (1 << (playerNames.PlayerCount+7));
        if(checkpointArrow && playerNames)
        checkpointArrow.layer = playerNames.PlayerCount + 7;
    }
    void Update()
    {
        transform.position = cameraPosition.position;
        float fovSpeed = 4f;
        currentFov = Mathf.Lerp(currentFov, targetFov, fovSpeed);
        cam.fieldOfView = currentFov;
    }


}
