using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointFinder : MonoBehaviour
{
    private Vector3 lastCheckPoint;
    private Vector3 nextCheckpoint;
    private int currentCheckPointIndex;

    private CheckPoints CheckPoints;

    public GameObject model;

    public Vector3 LastCheckPoint { get => lastCheckPoint;}



    // Start is called before the first frame update
    void Start()
    {
        CheckPoints = UnityEngine.Object.FindObjectOfType<CheckPoints>();

        currentCheckPointIndex = -1;
        if (CheckPoints && CheckPoints.HasCheckPoints && CheckPoints.GetNextCheckPoint(currentCheckPointIndex + 1))
        {
            lastCheckPoint = transform.position;
            nextCheckpoint = CheckPoints.GetNextCheckPoint(currentCheckPointIndex + 1).transform.position;

            currentCheckPointIndex++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 relativePos = nextCheckpoint - transform.position;

        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.Cross(transform.right, relativePos));
        if (model)
            model.transform.rotation = rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("CheckPoint"))
            if (CheckPoints.GetCheckPointIndex(other.gameObject) >= currentCheckPointIndex)
            {
                if (CheckPoints && CheckPoints.HasCheckPoints && CheckPoints.GetNextCheckPoint(currentCheckPointIndex + 1))
                {
                    lastCheckPoint = nextCheckpoint;
                    nextCheckpoint = CheckPoints.GetNextCheckPoint(currentCheckPointIndex + 1).transform.position;

                    currentCheckPointIndex++;
                }
            }
    }
}
