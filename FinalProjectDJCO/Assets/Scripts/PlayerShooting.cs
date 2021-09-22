using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PlayerShooting : MonoBehaviour
{
    private bool Shooting = false;
    public Transform firePoint;
    public GameObject bulletPre;
    public float bulletForce;
    public int equiped = 2;
    public float maxSpread = 0.1f;
    private bool waitingRelease = false; 
    private int initialBullets = 60;

    public Transform ammoText;

    private const byte NORMAL_FIRE_EVENT = 0;
    private const byte SHOTGUN_FIRE_EVENT = 1;

    void FixedUpdate()
    {
        if (Shooting) {
            if(initialBullets > 0) {
            shooting();
            }
            
        }
        else
            waitingRelease = false;

        ammoText.GetComponent<Text>().text = initialBullets.ToString();
    }

    private void Awake()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    private void NetworkingClient_EventReceived(EventData obj)
    {
        if (obj.Code == NORMAL_FIRE_EVENT)
        {
            object[] data = (object[])obj.CustomData;
            Vector3 position = (Vector3)data[0];
            Quaternion rotation = (Quaternion)data[1];
            Vector3 forward = (Vector3)data[2];

            GameObject bullet = Instantiate(bulletPre, position, rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(forward * bulletForce);
            FMODUnity.RuntimeManager.PlayOneShotAttached("event:/tiro1", bullet);
        }
        else if (obj.Code == SHOTGUN_FIRE_EVENT)
        {
            bool soundPlayed = false;
            object[] data = (object[])obj.CustomData;
            for (int i = 0; i < data.Length - 2; i = i + 3)
            {
                Vector3 position = (Vector3)data[i];
                Quaternion rotation = (Quaternion)data[i + 1];
                Vector3 forward = (Vector3)data[i + 2];

                GameObject bullet = Instantiate(bulletPre, position, rotation);
                if (soundPlayed == false)
                {
                    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/shotgun1", bullet);
                    soundPlayed = true;
                }
                Rigidbody rb1 = bullet.GetComponent<Rigidbody>();
                rb1.AddForce(forward * bulletForce);
            }
        }
    }

    void shooting()
    {
        switch (equiped)
        {
            case 1:
            if (!waitingRelease) {
                GameObject bullet = Instantiate(bulletPre, firePoint.position, firePoint.rotation);
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                rb.AddForce(firePoint.forward * bulletForce);
                if (PhotonNetwork.InRoom)
                {
                    object[] data = new object[] { firePoint.position, firePoint.rotation, firePoint.forward };
                    PhotonNetwork.RaiseEvent(NORMAL_FIRE_EVENT, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);
                }
                initialBullets = initialBullets-1;
                waitingRelease = true;
                    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/tiro1", gameObject);
                }
                break;
            case 2:
                if (!waitingRelease)
                {
                    object[] data = new object[90];
                    int index = 0;
                    for (int i = 0; i < 30; i++)
                    {
                        GameObject bullet1 = Instantiate(bulletPre, firePoint.position, firePoint.rotation);
                        data[index] = firePoint.position;
                        data[index + 1] = firePoint.rotation;
                        Rigidbody rb1 = bullet1.GetComponent<Rigidbody>();

                        //Add randomness to every bullet direction
                        Vector3 dir = firePoint.forward + new Vector3(Random.Range(-maxSpread, maxSpread), Random.Range(-maxSpread, maxSpread), Random.Range(-maxSpread, maxSpread));
                        data[index + 2] = dir;
                        index = index + 3;
                        rb1.AddForce(dir * bulletForce);
                    }
                    initialBullets = initialBullets-5;
                    if (initialBullets < 0)
                        initialBullets = 0;
                    waitingRelease = true;
                    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/shotgun1", gameObject);
                    if (PhotonNetwork.InRoom)
                    {
                        PhotonNetwork.RaiseEvent(SHOTGUN_FIRE_EVENT, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);
                    }
                }
                break;
            default:
                break;
        }
    }

    private void OnShoot(InputValue value)
    {
        Shooting = value.isPressed;
    }

    private void OnChange_Weapon(InputValue value)
    {
        if (equiped == 1)
            equiped = 2;
        else
            equiped = 1;
    }
}
