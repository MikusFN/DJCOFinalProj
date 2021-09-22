using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class SphereMarch : MonoBehaviour
{

    private List<Collider> grappleblePoints;
    private SphereCollider sphereCollider;

    private float startRadius;
    [SerializeField] float maxradius = 50f;
    [SerializeField] int radiusStep = 3;

    private bool Hascollided;
    private float currentRadius = 0;
    [SerializeField] float sightAngle = 45;

    private Vector3 currentSpherePos;
    [SerializeField] int maxStep = 20;
    [SerializeField] int maxSightDistance = 100;

    public Grappling grappling;
    public GameObject raySphere;

    private List<RaySphere> raySpheres;

    private int currentStep;
    private float currentDistance;
    private Collider rayCastColl;

    /* 
     * launch sphere on player and grow until collision or max check distance
     * (collision needs to be on front -> angle between foward direction and point less then 45)
     * use the new sphere radius as step for ray march step
     * iterate until the ray lanched hit the same surface as the last sphere
     * gather all hitting points
     * sort by distance and angle from foward direction (heuristic) 
     * just used valid points in the scene ( check the grapplig objects )
     * USE A SET OF SPHERES ( ADD SPHERE AT EACH STEP EITH A COLLIDER) 
     */

    public

    // Start is called before the first frame update
    void Awake()
    {
        grappleblePoints = new List<Collider>();
        raySpheres = new List<RaySphere>();
        //sphereCollider = GetComponent<SphereCollider>();

        //startRadius = sphereCollider.radius;

        currentStep = 0;
        currentDistance = 0;

        currentSpherePos = transform.position;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (grappling && grappling.IsGrappling)
        //{
        //}
        //SphereMarchLoop();

    }

    private void SphereMarchLoop()
    {
        //Use current radius to spwan another one until max distance and for a fixed number os steps
        currentStep = 0;
        while (currentStep < maxStep && currentDistance < maxSightDistance)
        {
            March();
            currentDistance = Vector3.Distance(currentSpherePos, transform.position);
            currentStep++;
        }
    }

    private void March()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxSightDistance))
        {
            ////get the point
            //Debug.DrawLine(currentSpherePos, currentSpherePos + transform.forward, Color.black);
            //Debug.Log("got obj " + hit.collider.name);
            rayCastColl = hit.collider;
        }
        else
        {
            SearchCollision();
            rayCastColl = null;
        }
    }

    private void SphereSpawn(Vector3 newPos)
    {
        //if (sphereCollider.radius < maxradius && Hascollided == false)
        //{
        //    sphereCollider.radius += radiusStep * Time.fixedDeltaTime;
        //}
        //else
        //{
        //    currentRadius = sphereCollider.radius;
        //    sphereCollider.radius = startRadius;
        //    Hascollided = false;
        //    sphereCollider.center = Vector3.Distance(Vector3.zero, newPos) < maxSightDistance ? newPos : Vector3.zero;

        //}

        GameObject go = Instantiate<GameObject>(raySphere, this.transform);
        go.transform.position = newPos;

        RaySphere raySphereAux = go.GetComponent<RaySphere>();

        raySphereAux.RadiusStep = radiusStep;
        raySphereAux.Maxradius = maxradius;
        raySphereAux.SightAngle = sightAngle;

        currentSpherePos = raySphereAux.transform.position;

        raySpheres.Add(raySphereAux);
    }

    private void SearchCollision()
    {
        List<RaySphere> aux = raySpheres.FindAll(x => x.IsColliding == true);

        if (aux != null && aux.Count > 0)
        {
            RaySphere raySphere = aux.Find(x => Vector3.Distance(x.transform.position, transform.position) < currentDistance);
            if (raySphere)
            {
                currentRadius = raySphere.CurrentRadius;
                SphereSpawn(currentSpherePos + (transform.forward * currentRadius));
            }
        }
        else
        {
            SphereSpawn(currentSpherePos + (-transform.right * currentRadius));
        }
    }

    public List<Collider> GrapplingColliders()
    {
        grappleblePoints.Clear();
        raySpheres.Clear();


        SphereMarchLoop();

        List<RaySphere> aux = raySpheres.FindAll(x => x.IsColliding == true);

        foreach (var item in aux)
        {
            grappleblePoints.Add(item.CurrentCollider);
        }
        if (rayCastColl)
        {
            grappleblePoints.Add(rayCastColl);
        }
        grappleblePoints.OrderBy(x => Vector3.Distance(x.transform.position, transform.position));


        return grappleblePoints;

    }

}



