using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    GameObject[] SceneObjs;
    SortedList<string, GameObject> CheckPointsList;

    public bool HasCheckPoints { get => CheckPointsList != null && CheckPointsList.Count > 0; }

    // Start is called before the first frame update
    void Awake()
    {

        CheckPointsList = new SortedList<string, GameObject>();

        SceneObjs = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach (GameObject go in SceneObjs)
        {
            if (go.activeInHierarchy && go.tag.Equals("CheckPoint"))
            {
                CheckPointsList.Add(go.name, go);
            }
        }

    }

    public GameObject GetNextCheckPoint(int currentIndex)
    {
        if (currentIndex < CheckPointsList.Count)
            return CheckPointsList.Values[currentIndex];
        else
        {
            return null;
        }
    }

    public int GetCheckPointIndex(GameObject checkpoint)
    {
        int index = -1;
        if (checkpoint)
        {
            index = CheckPointsList.IndexOfValue(checkpoint);
        }
        return index;
    }
}
