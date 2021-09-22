using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrappleLook : MonoBehaviour
{
    // Declare and initialize a new List of GameObjects called grappleblePoints.
    SortedList<float, Collider> grappleblePoints = new SortedList<float, Collider>();

    [SerializeField] Transform playerTransform;
    [SerializeField] int MaxDistance;
    [SerializeField] LayerMask grappLayer;
    [SerializeField] GameObject highlightSight;
    [SerializeField] Camera Cam;


    private Vector3 beginPos;
    private Vector3 beginScale;

    public bool HasObjects { get => grappleblePoints.Count > 0; }

    private GrapplingObjects gp;
    public RaySphere raySphere;

    private float maxDist = 100;
    private float maxAngle = 45;
    private float currentBestDist ;
    private float currentBestAngle ;

    //public SphereMarch sphereMarch;

    private void Awake()
    {
        gp = UnityEngine.Object.FindObjectOfType<GrapplingObjects>();
    }

    // Start is called before the first frame update
    void Start()
    {
        beginPos = transform.position;
        beginScale = transform.localScale;
        currentBestAngle = maxAngle;
        currentBestDist = maxDist;
    }

    // Update is called once per frame
    void Update()
    {
        DebugObjectsInLine();
        //ProbeGrapPoints();
        ShowBestGrappPoint();
    }

    private void ShowBestGrappPoint()
    {
        Collider bestPointCollider = GetBestGrapplingCollider();
        //Vector3 bestPointinView = Camera.main.WorldToViewportPoint(bestPoint);
        if (bestPointCollider && CheckIfInScreen(bestPointCollider))
        {
            highlightSight.SetActive(true);
            //put aim sight on it
            highlightSight.transform.position =
                 Cam.WorldToScreenPoint(bestPointCollider.transform.position);
        }
        else
        {
            highlightSight.SetActive(false);
        }
    }

    private bool CheckIfInScreen(Collider bestPointinView)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Cam);
        return GeometryUtility.TestPlanesAABB(planes, bestPointinView.bounds);
    }

    public Collider GetBestGrapplingCollider()
    {
        Collider currentBestPoint = new Collider();
        //grappleblePoints = sphereMarch.GrapplingColliders();

        //        grappleblePoints = raySphere.GrapplingColliders();

        UpdateGrapplingPoints();

        if (grappleblePoints.Count > 0)
        {
            //currentBestPoint = grappleblePoints.ElementAt<Collider>(UnityEngine.Random.Range(0, (int)grappleblePoints.Count / 4));

            currentBestPoint = grappleblePoints.ToList().Find(x => (x.Key <= currentBestAngle && Vector3.Distance(x.Value.transform.position, playerTransform.position) <= currentBestDist)).Value;
            if (!currentBestPoint)
            {
                if(currentBestAngle<maxDist)
                currentBestDist += 10;
                if(currentBestAngle<maxAngle)
                currentBestAngle += 5;
                currentBestPoint = grappleblePoints.First().Value;
            }
        }
        //if (grappleblePoints.Exists(x => Vector3.Distance(playerTransform.position, x.transform.position) < MaxDistance))
        //{
        //    currentBestPoint = grappleblePoints.Find(x => Vector3.Distance(playerTransform.position, x.transform.position) < MaxDistance);
        //    if (!CheckIfGrappExists(currentBestPoint.gameObject))
        //    {
        //        currentBestPoint = null;
        //    }
        //}

        if (currentBestPoint && !CheckIfGrappExists(currentBestPoint.gameObject))
        {
            currentBestPoint = null;
        }

        return currentBestPoint;
    }


    private void UpdateGrapplingPoints()
    {
        List<Collider> aux = raySphere.GrapplingColliders();
        foreach (var item in aux)
        {
            if (item && !grappleblePoints.ContainsValue(item) && CheckIfGrappExists(item.gameObject))
            {
                grappleblePoints.Add(Vector3.Angle(playerTransform.forward,
            (item.transform.position - transform.position)), item);
            }
        }
    }

    public void RemoveGrapPoint(Collider col)
    {
        if (grappleblePoints.ContainsValue(col))
        {
            grappleblePoints.RemoveAt(grappleblePoints.IndexOfValue(col));
        }
    }

    void DebugObjectsInLine()
    {
        for (int i = 0; i < grappleblePoints.Values.Count; i++)
        {
            if (playerTransform && CheckIfGrappExists(grappleblePoints.Values[i].gameObject))
            {
                Debug.DrawLine(playerTransform.position, grappleblePoints.Values[i].transform.position, Color.red);
            }
        }
    }

    public bool CheckIfGrappExists(GameObject go)
    {
        if (gp && go && !gp.ExistsGrappObject(go))
        {
            RemoveGrapPoint(go.GetComponent<Collider>());
            return false;
        }
        return true;
    }

    public bool HasGrapplePoint(Collider grappCollider)
    {
        return grappleblePoints.ContainsValue(grappCollider);
    }

}
