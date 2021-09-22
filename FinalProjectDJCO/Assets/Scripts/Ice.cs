using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Ice : MonoBehaviour
{
    public GameObject iceEffect;
    
    private GrapplingObjects gp;

    private void Awake()
    {
       gp = UnityEngine.Object.FindObjectOfType<GrapplingObjects>();
    }

    void OnCollisionEnter(Collision collision)
    {
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();

        if (bullet != null)
        {
            StartCoroutine(Die());

        }
        
    }

    IEnumerator Die(){
        GetComponent<Renderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;

        GameObject effect = Instantiate(iceEffect,transform.position,Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z));

        Destroy(effect,10f);

        gameObject.GetComponent<StudioEventEmitter>().Play();

        if(gp)
        gp.RemoveGrappObject(this.gameObject);

        yield return new WaitForSeconds(10f);
        Destroy(this.gameObject);

    }

}