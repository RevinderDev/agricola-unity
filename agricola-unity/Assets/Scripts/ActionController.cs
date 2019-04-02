using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    private Color materialBasicColor;

    // Start is called before the first frame update
    void Start()
    {
        materialBasicColor = GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseEnter()
    {
        GetComponent<Renderer>().material.color = Color.gray;
    }
    void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = materialBasicColor;
    }

    private void OnMouseDown()
    {
        
    }
}
