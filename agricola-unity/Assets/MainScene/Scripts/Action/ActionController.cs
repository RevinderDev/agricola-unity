using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/*
 * Defines the behaviour of object with which player can interact.
 */
public class ActionController : MonoBehaviour
{
    private Color materialBasicColor;
    private Color lastColor;
    GameController gameController;
    
    // Start is called before the first frame update
    void Start()
    {
        materialBasicColor = GetComponent<Renderer>().material.color;
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseEnter()
    {
        lastColor = GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = Color.gray;
    }
    void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = lastColor;
    }

    private void OnMouseDown()
    {
        switch (tag)
        {
            case "PlantingArea":
                gameController.AddAction(gameObject, PlayerActionList.ActionType.plant, 500, materialBasicColor);
                break;
            case "Carrot":
                gameController.AddAction(gameObject, PlayerActionList.ActionType.collectPlant, 1000, materialBasicColor);
                break;
        }
    }
    
}
