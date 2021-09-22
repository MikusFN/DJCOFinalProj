using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class Grappling : MonoBehaviourPun
{

    [SerializeField] Transform Cam;
    [SerializeField] Transform debugHit;
    [SerializeField] Rigidbody PlayerRB;
    [SerializeField] float grapMinSpeed = 10f;
    [SerializeField] float grapMaxSpeed = 40f;
    [SerializeField] float grapSpeedMult = 5f;
    [SerializeField] LayerMask grappLayer;
    [SerializeField] GrappleLook grappleLook;
    [SerializeField] float spring, damper, massScale;
    [SerializeField] float maxGrappDistance = 100f;

    private const float GRAPPLING_FOV = 100f;
    private const float NORMAL_FOV = 60f;

    [SerializeField] CameraMovement cameraMovement;

    private Vector3 grappPosition;
    private RaycastHit ray;
    private bool IsAiming;
    private bool isGrappling;
    private SpringJoint joint;
    private LineRenderer lr;
    private bool grapplingButton;
    private Collider currentGrapCollider;

    private const byte GRAPPLE_TRANSVERSAL = 2;
    private const byte GRAPPLE_STOP = 3;
    private const byte GRAPPLE_CHECK = 4;

    public Vector3 GrappPosition { get => grappPosition; }
    public bool IsGrappling { get => isGrappling; }

    private FMOD.Studio.EventInstance ropeShoot;
    private FMOD.Studio.EventInstance ropeClick;
    private FMOD.Studio.EventInstance grappling;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GrappleFire();
        CheckIfCanStillGrapple(currentGrapCollider);

        if (joint)
        {
            Debug.DrawLine(this.transform.position, joint.connectedAnchor, Color.blue);
        }
        if (isGrappling)
        {
            FMOD.Studio.PLAYBACK_STATE state;
            grappling.getPlaybackState(out state);
            if (state == FMOD.Studio.PLAYBACK_STATE.STOPPED)
            {
                grappling = FMODUnity.RuntimeManager.CreateInstance("event:/Corda Tensa");
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(grappling, gameObject.transform, gameObject.GetComponent<Rigidbody>());
                grappling.start();
                grappling.release();
            }
        }
        else
        {
            grappling.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }

    private void NetworkingClient_EventReceived(EventData obj)
    {
        if (obj.Code == GRAPPLE_TRANSVERSAL)
        {
            int viewID = (int)obj.CustomData;
            PhotonView view = PhotonView.Find(viewID);
            view.gameObject.GetComponent<Grappling>().GrappleTransversal();
        }
        if (obj.Code == GRAPPLE_STOP)
        {
            int viewID = (int)obj.CustomData;
            PhotonView view = PhotonView.Find(viewID);
            view.gameObject.GetComponent<Grappling>().StopGrappleTransversal();
        }
        if (obj.Code == GRAPPLE_CHECK)
        {
            int viewID = (int)obj.CustomData;
            PhotonView view = PhotonView.Find(viewID);
            view.gameObject.GetComponent<Grappling>().GrappleCheck();
        }
    }

    private void ResetGrappPos()
    {
        if (!IsGrappling && !IsAiming)
        {
            grappPosition = Vector3.zero;
        }
    }

    private void GrappleCheck()
    {
        bool hasRB = (ray.rigidbody != null);
        Rigidbody currentRB = hasRB ? ray.rigidbody : PlayerRB;
        isGrappling = GrapplingMovement(grappPosition, ref currentRB, hasRB);
        if (isGrappling == true)
        {
            ropeShoot.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            ropeShoot = FMODUnity.RuntimeManager.CreateInstance("event:/Disparo Corda");
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(ropeShoot, gameObject.transform, gameObject.GetComponent<Rigidbody>());
            ropeShoot.start();
            ropeShoot.release();
        }
    }

    private bool GrapplingMovement(Vector3 grapplingPos, ref Rigidbody rb, bool hasRB)
    {
        bool hasgrap = false;
        if (grapplingPos != Vector3.zero)
        {
            int isPlayer = hasRB ? -1 : 1;

            Vector3 grapDir = isPlayer * (grapplingPos - transform.position).normalized;

            float grapSpeed = Mathf.Clamp(Vector3.Distance(transform.position, grapplingPos), grapMinSpeed, grapMaxSpeed);

            Vector3 vel = grapDir * grapSpeed * grapSpeedMult;

            rb.AddForce(vel, ForceMode.Impulse);
            hasgrap = true;
        }
        return hasgrap;
    }


    private void GrappleFire()
    {
        if (IsAiming &&
            Physics.Raycast(Cam.position + Cam.forward.normalized
            , Cam.forward, out ray, maxGrappDistance, grappLayer))
        {
            debugHit.position = ray.point;
            grappPosition = ray.point;
            debugHit.GetComponent<MeshRenderer>().enabled = true;
            //cameraMovement.TargetFov = GRAPPLING_FOV;
        }
        else
        {
            debugHit.position = Vector3.zero;
            //grappPosition = Vector3.zero;
            debugHit.GetComponent<MeshRenderer>().enabled = false;
            //cameraMovement.TargetFov = NORMAL_FOV;
        }
    }

    private void GrappleTransversal()
    {
        if (grappleLook.HasObjects)
        {
            StartGrapplingTransversal();
            ropeShoot.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            ropeShoot = FMODUnity.RuntimeManager.CreateInstance("event:/Disparo Corda");
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(ropeShoot, gameObject.transform, gameObject.GetComponent<Rigidbody>());
            ropeShoot.start();
            ropeShoot.release();
        }
        isGrappling = grappleLook.HasObjects;
    }

    private void StartGrapplingTransversal()
    {
        float distanceFromPoint = 0;
        if (joint != null)
            return;

        currentGrapCollider = grappleLook.GetBestGrapplingCollider();
        if (currentGrapCollider)
            grappPosition = currentGrapCollider.ClosestPoint(this.transform.position);

        if (grappPosition != null)
        {
            distanceFromPoint = Vector3.Distance(this.transform.position, grappPosition);
        }

        if (distanceFromPoint > 0)
        {
            joint = this.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grappPosition;

            //The distance grapple will try to keep from grapple point. 
            joint.maxDistance = distanceFromPoint * 0.6f;
            joint.minDistance = distanceFromPoint * 0.25f;

            //Adjust these values to fit your game.
            joint.spring = spring;
            joint.damper = damper;
            joint.massScale = massScale;
        }
    }

    void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!joint) return;

        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, joint.connectedAnchor);
    }

    public void StopGrappleTransversal()
    {
        //lr.positionCount = 0;
        Vector3 anchorpoint = Vector3.zero;
        if (joint)
            anchorpoint = joint.connectedAnchor;

        grappleLook.RemoveGrapPoint(currentGrapCollider);
        grappPosition = Vector3.zero;
        isGrappling = false;
        if (joint != null)
            Destroy(joint);
    }

    private void CheckIfCanStillGrapple(Collider grappCollider)
    {
        if (isGrappling && grappCollider != null)
        {
            if (!grappleLook.CheckIfGrappExists(grappCollider.gameObject))
            {
                StopGrappleTransversal();
            }
        }
    }

    private void OnLook_Right(InputValue value)
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        grapplingButton = value.isPressed ? true : false;

        if (grapplingButton)
        {
            if (IsAiming)
            {
                GrappleCheck();
                if (PhotonNetwork.InRoom)
                {
                    PhotonNetwork.RaiseEvent(GRAPPLE_CHECK, photonView.ViewID, RaiseEventOptions.Default, SendOptions.SendUnreliable);
                }
            }
            else
            {
                GrappleTransversal();
                if (PhotonNetwork.InRoom)
                {
                    PhotonNetwork.RaiseEvent(GRAPPLE_TRANSVERSAL, photonView.ViewID, RaiseEventOptions.Default, SendOptions.SendUnreliable);
                }
            }
        }
        else
        {
            StopGrappleTransversal();
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.RaiseEvent(GRAPPLE_STOP, photonView.ViewID, RaiseEventOptions.Default, SendOptions.SendUnreliable);
            }
        }
    }

    private void OnAim(InputValue value)
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        IsAiming = value.isPressed ? true : false;
        if (IsAiming == true)
        {
            ropeClick.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            ropeClick = FMODUnity.RuntimeManager.CreateInstance("event:/Click Corda");
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(ropeClick, gameObject.transform, gameObject.GetComponent<Rigidbody>());
            ropeClick.start();
            ropeClick.release();
        }
    }

}

