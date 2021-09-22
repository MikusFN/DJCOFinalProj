using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GrapplingObjects : MonoBehaviour
{

    GameObject[] SceneObjs;
    List<GameObject> GrapObjsList;

    // Start is called before the first frame update
    void Awake()
    {

        GrapObjsList = new List<GameObject>();

        SceneObjs = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach (GameObject go in SceneObjs)
        {
            if (go.activeInHierarchy && go.tag.Equals("Grapple"))
            {
                GrapObjsList.Add(go);
            }
        }

    }

    //Methods to manage this list

    public void RemoveGrappObject(GameObject gp)
    {
        if (gp && GrapObjsList.Exists(x => x == gp))
        {
            GrapObjsList.Remove(gp);
            Debug.Log(gp + "has been removed");
        }
    }

    public bool ExistsGrappObject(GameObject gp)
    {
        if (gp && GrapObjsList.Count > 0)
        {
            return GrapObjsList.Contains(gp);
        }
        return false;
    }

    public void AddGrappObjects ( GameObject gp)
    {
        if(gp && gp.tag.Equals("Grapple") && GrapObjsList != null)
        {
            GrapObjsList.Add(gp);
        }
    }

}
