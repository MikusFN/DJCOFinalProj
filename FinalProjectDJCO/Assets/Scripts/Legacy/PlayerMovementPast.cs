using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementPast : MonoBehaviour
{
    float h;
    float v;
    public float rx = 0;
    public float ry = 0;

    public float moveSpeed;


    public PlayerControls controls;
    public Vector2 move;
    public Vector2 rotate;

    private Rigidbody rb;
    private Camera cam;
    private float CameraPitch;
    public Transform firePoint;
    public GameObject bulletPre;

    float rotationY = 0f;
    float rotationX = 0f;

    float minimumY = -60f;
    float maximumY = 60f;

    public float sensivityX = 1f;
    public float sensivityY = 1f;
    public Vector3 offset;
    private bool Jumping = false;
    private bool Shooting = false;
    private bool Grounded = false;
    private float jumpForce = 200f;
    public float bulletForce = 1f;


    public float CameraPitch1 { get => CameraPitch; }

    private void Awake()
    {
        cam = GetComponentInParent<PlayerBehaviour>().GetComponentInChildren<CameraPlayer>().Cam;
        rb = GetComponent<Rigidbody>();
    }

    private void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }
    private void OnLook(InputValue value)
    {
        rotate = value.Get<Vector2>();
    }
    
    private void OnJump(InputValue value)
    {
        Jumping = value.isPressed;
    }

    private void onChange_Weapon(InputValue value)
    {
        //TODO
    }

    private void onReload(InputValue value)
    {
        //TODO
    }

    private void onSlide(InputValue value)
    {
        //TODO
    }

    private void onMelee(InputValue value)
    {
        //TODO
    }

    private void onRun(InputValue value)
    {
        //TODO
    }

    private void onOptions(InputValue value)
    {
        //TODO
    }

    private void onLook_Right(InputValue value)
    {
        //TODO
    }

    private void onLook_Left(InputValue value)
    {
        //TODO
    }

    private void onAim(InputValue value)
    {
        //TODO
    }

    private void OnShoot(InputValue value)
    {
        Shooting = value.isPressed;
    }

    private void onLook_Behind(InputValue value)
    {
        //TODO
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        movement();

        if(Shooting)
            shooting();
    }

    void movement()
    {

        v = move.y;
        h = move.x;

        Vector3 velocityaux = Vector3.zero;
        velocityaux = transform.forward * moveSpeed * v;
        velocityaux += transform.right * moveSpeed * h;

        if (Grounded)
        {
            rb.drag = 5;

            if (Jumping)
            {
                Grounded = false;
                velocityaux += transform.up * jumpForce;
            }
        }

        if (rb.velocity.y < 0f)
        {
            rb.AddForce((-transform.up * rb.mass * 9.8f));
        }

        rb.AddForce(velocityaux * rb.mass, ForceMode.Acceleration);


        rx = rotate.x;
        ry = rotate.y;
        rotationX += rx * sensivityX;
        rotationY += ry * sensivityY;

        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);


        transform.localEulerAngles = new Vector3(0, rotationX, 0);
        //cam.transform.position = new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, transform.position.z + offset.z); // Camera follows the player with specified offset position 
        //cam.transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);

        CameraPitch = -rotationY;
        Jumping = false;
    }

    void shooting()
    {
        GameObject bullet = Instantiate(bulletPre, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * bulletForce);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Grounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Grounded = false;
        }
    }
}
