using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Defines the behaviour of object with which player can interact.
 */
public class ActionController : MonoBehaviour
{
    private Color materialBasicColor;
    private Vector3 position;
    GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        materialBasicColor = GetComponent<Renderer>().material.color;
        position = transform.position;
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
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
        gameController.AddAction(position, 1000, materialBasicColor);
    }
}
