using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManagment : MonoBehaviour
{

    public int lifes = 5;
    public int health = 100;

    public CheckPointFinder checkPointFinder;
    private FMOD.Studio.EventInstance takeDamage;

    void die()
    {
        if (lifes != 0)
        {
            lifes--;
            health = 100;
            Respawn();
        }
    }

    private void Respawn()
    {
        if (PhotonNetwork.IsConnected == true)
        {
            transform.position = GameObject.Find("NetworkInstantiate").transform.position;
            return;
        }
        if (checkPointFinder)
        {
            transform.position = checkPointFinder.LastCheckPoint+Vector3.up*0.5f;
        }
    }

    public void loseHealth(int ammount)
    {
        takeDamage.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        takeDamage = FMODUnity.RuntimeManager.CreateInstance("event:/dano 1");
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(takeDamage, gameObject.transform, gameObject.GetComponent<Rigidbody>());
        takeDamage.start();
        takeDamage.release();
        if (this.health - ammount < 0)
        {
            this.health = 0;
            die();
        }
        else
            this.health -= ammount;
    }

    void earnHealth(int ammount)
    {
        if (this.health + ammount > 100)
            this.health = 100;
        else
            this.health += ammount;
    }

    void loseLife()
    {
        this.lifes--;
    }

    void earnLife()
    {
        this.lifes++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Resp"))
        {
            die();
        }
    }




}
