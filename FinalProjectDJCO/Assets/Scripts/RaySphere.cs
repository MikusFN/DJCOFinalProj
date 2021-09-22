using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RaySphere : MonoBehaviour, IComparer<RaySphere>
{
    private SphereCollider sphereCollider;

    private bool isColliding;

    public bool IsColliding { get => isColliding; }
    public Collider CurrentCollider { get => currentCollider; }
    public float CurrentRadius { get => currentRadius; }
    public float Maxradius { get => maxradius; set => maxradius = value; }
    public int RadiusStep { get => radiusStep; set => radiusStep = value; }
    public float SightAngle { get => sightAngle; set => sightAngle = value; }
    public float AngleFromViewer { get => angleFromViewer; set => angleFromViewer = value; }
    public float DistanceFromViewer { get => distanceFromViewer; set => distanceFromViewer = value; }

    private float currentRadius;
    private float startRadius;

    [SerializeField] float sightAngle = 45;

    private List<Collider> currentColliders;

    [SerializeField] float maxradius = 50f;
    [SerializeField] int radiusStep = 3;
    [SerializeField] float detectableHeight = 1f;


    private float angleFromViewer;
    private float distanceFromViewer;
    private Collider currentCollider;

    public GrappleLook grappleLook;

    public int CompareAngle(RaySphere x, RaySphere y)
    {

        return x.AngleFromViewer < y.AngleFromViewer ? -1 : 1;
    }

    public int CompareDistance(RaySphere x, RaySphere y)
    {

        return x.DistanceFromViewer < y.DistanceFromViewer ? -1 : 1;
    }


    public RaySphere(Vector3 startPos)
    {
        transform.position = startPos;
    }

    // Start is called before the first frame update
    void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        currentRadius = sphereCollider.radius;
        startRadius = currentRadius;
        currentColliders = new List<Collider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SphereGrow();
        if (currentColliders.Count == 0)
        {
            isColliding = false;
        }
    }

    private void SphereGrow()
    {
        if (isColliding == false)
        {
            if (sphereCollider.radius < maxradius)
            {
                sphereCollider.radius += radiusStep * Time.fixedDeltaTime;
            }
            else
            {
                sphereCollider.radius = startRadius;
            }
        }
        //    currentRadius = sphereCollider.radius;
        //    sphereCollider.radius = startRadius;
        //    isColliding = false;
        //    //sphereCollider.center = Vector3.Distance(Vector3.zero, newPos) < maxSightDistance ? newPos : Vector3.zero;
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Grapple") &&
            Vector3.Angle(transform.forward,
            (other.transform.position - transform.position)) < sightAngle &&
           other.transform.position.y > (transform.position.y * detectableHeight))
        {
            //Debug.Log(Vector3.Angle(transform.forward,
            //(other.transform.position - transform.position)) + "is the angle");

            RaycastHit raycastHit;
            if (Physics.Raycast(transform.position, (other.transform.position - transform.position).normalized, out raycastHit, sphereCollider.radius))
            {
                if (raycastHit.collider.tag.Equals("Grapple") && !currentColliders.Contains(other))
                {
                    isColliding = true;
                    currentRadius = sphereCollider.radius;
                    currentCollider = other;
                    currentColliders.Add(other);

                    angleFromViewer = Vector3.Angle(transform.forward,
                    (other.transform.position - transform.position));
                    distanceFromViewer = Vector3.Distance(other.transform.position, GetComponentInParent<Transform>().position);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other == CurrentCollider)
        {
            //if (Vector3.Distance(other.bounds.ClosestPoint(transform.position), transform.position) < currentRadius)
            //{
            //    Debug.Log("stay distance " + Vector3.Distance(other.bounds.ClosestPoint(transform.position), transform.position));
            //    currentRadius = Vector3.Distance(other.bounds.ClosestPoint(transform.position), transform.position);
            //    sphereCollider.radius = currentRadius;
            //}
            //else
            //{
            //    currentRadius = Vector3.Distance(other.bounds.ClosestPoint(transform.position), transform.position);
            //    sphereCollider.radius = currentRadius;
            //}

            //if (Vector3.Distance(other.bounds.ClosestPoint(transform.position), transform.position) < currentRadius)
            //{
            //    currentRadius = Vector3.Distance(other.bounds.ClosestPoint(transform.position), transform.position);
            //    sphereCollider.radius = currentRadius;
            //}

            if (sphereCollider.radius < maxradius)
            {
                sphereCollider.radius += radiusStep * Time.fixedDeltaTime;
            }
            //else
            //{
            //    sphereCollider.radius = startRadius;
            //}

            // give back all the collisions while getting bigger until it reaches max size then gets smaller
            // when exists removes from colliders
        }
        //else
        //{
        if (other.gameObject.tag.Equals("Grapple"))
        {

            if (Vector3.Angle(transform.forward,
            (other.transform.position - transform.position)) < sightAngle &&
           other.transform.position.y > (transform.position.y * detectableHeight))
            {
                RaycastHit raycastHit;
                if (Physics.Raycast(transform.position, (other.transform.position - transform.position).normalized, out raycastHit, sphereCollider.radius))
                {
                    if (raycastHit.collider.tag.Equals("Grapple") && !currentColliders.Contains(other))
                    {
                        isColliding = true;
                        currentRadius = sphereCollider.radius;

                        currentColliders.Add(other);

                        angleFromViewer = Vector3.Angle(transform.forward,
                        (other.transform.position - transform.position));
                        distanceFromViewer = Vector3.Distance(other.transform.position, GetComponentInParent<Transform>().position);
                    }
                }
            }
            else
            {
                if (currentColliders.Contains(other))
                {
                    if (grappleLook && grappleLook.HasGrapplePoint(other))
                    {
                        grappleLook.RemoveGrapPoint(other);
                    }
                    currentColliders.Remove(other);
                }
            }
            // //if (other.gameObject.tag.Equals("Grapple") &&
            // Vector3.Angle(transform.forward,
            // (other.transform.position - transform.position)) < sightAngle &&
            //other.transform.position.y > (transform.position.y * detectableHeight) &&
            //!currentColliders.Contains(other))
            // {
            //     //Debug.Log(Vector3.Angle(transform.forward,
            //     //(other.transform.position - transform.position)) + "is the angle");
            //     isColliding = true;
            //     currentRadius = sphereCollider.radius;

            //     currentColliders.Add(other);

            //     angleFromViewer = Vector3.Angle(transform.forward,
            //     (other.transform.position - transform.position));
            //     distanceFromViewer = Vector3.Distance(other.transform.position, GetComponentInParent<Transform>().position);
            // }
            // //else
            // //{
            // //    if (currentColliders.Contains(other) && grappleLook && grappleLook.HasGrapplePoint(other))
            // //    {
            // //        grappleLook.RemoveGrapPoint(other);
            // //    }
            // //    currentColliders.Remove(other);
            // //}
            // //}
        }
    }

    public int Compare(RaySphere x, RaySphere y)
    {
        throw new System.NotImplementedException();
    }


    private void OnTriggerExit(Collider other)
    {
        //if (other == CurrentCollider)
        //{
        //    isColliding = false;
        //    //sphereCollider.radius = startRadius;
        //    //currentRadius = sphereCollider.radius;
        //    //currentCollider = null;
        //}

        if (currentColliders.Count > 0 && currentColliders.Contains(other))
        {
            isColliding = false;
            //sphereCollider.radius = startRadius;
            //currentRadius = sphereCollider.radius;
            //currentCollider = null;
            if (grappleLook && grappleLook.HasGrapplePoint(other))
            {
                grappleLook.RemoveGrapPoint(other);
            }
            currentColliders.Remove(other);
        }
    }

    public List<Collider> GrapplingColliders()
    {
        return currentColliders;
    }
}

