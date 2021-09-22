using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float time;
    public int damage=20;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Destroy(this.gameObject, time);
    }

    void OnCollisionEnter(Collision collision)
    {
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        GrappleLook gl = collision.gameObject.GetComponent<GrappleLook>();
        PlayerManagment pm = collision.gameObject.GetComponent<PlayerManagment>();
        if (pm != null)
        {
            pm.loseHealth(damage);
        }

        if (bullet == null && gl == null)
        {
            Destroy(this.gameObject);
        }

    }
}
