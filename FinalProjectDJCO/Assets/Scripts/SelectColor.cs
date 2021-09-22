using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SelectColor : MonoBehaviour
{
    
    float speed = 30.0f;

    public GameObject model;
    public Button redButton;
    public string color;
    // Start is called before the first frame update
    void Start()
    {
        redButton.Select();
    }

    // Update is called once per frame
    void Update()
    {
        model.transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }

    public void setRed() {
        model.GetComponent<Renderer>().material.color = new Color32(255, 0, 0, 200);
        color = "red";
    }

    public void setBlue() {
        model.GetComponent<Renderer>().material.color = new Color32(0, 0, 255, 200);
        color = "blue";
    }

    public void setPurple() {
        model.GetComponent<Renderer>().material.color = new Color32(255, 0, 200, 200);
        color="purple";
    }

    public void setGreen() {
        model.GetComponent<Renderer>().material.color = new Color32(0, 255, 0, 200);
        color = "green";
    }
}
