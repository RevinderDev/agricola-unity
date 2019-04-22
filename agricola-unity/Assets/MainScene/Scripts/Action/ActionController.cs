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
    public static bool isActive = true;
    private bool showTooltip = false;

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
        if (!isActive)
            return;
        lastColor = GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = Color.gray;
        showTooltip = true;
    }
    void OnMouseExit()
    {
        if (!isActive)
            return;
        GetComponent<Renderer>().material.color = lastColor;
        showTooltip = false;
    }

    private void OnMouseDown()
    {
        if (!isActive)
            return;
        switch (tag)
        {
            case "PlantingArea":
                gameController.AddAction(gameObject, ActionList.plant);
                break;
            case "Carrot":
                gameController.AddAction(gameObject, ActionList.collectPlant);
                break;
        }
    }

    void OnGUI()
    {
        if (showTooltip)
        {
            var x = Event.current.mousePosition.x;
            var y = Event.current.mousePosition.y;

            GUIStyle TextFieldStyles = new GUIStyle(EditorStyles.textField);
            GUI.contentColor = Color.white;
            GUI.color = Color.white;

            //Value Color
            TextFieldStyles.normal.textColor = Color.white;

            //Label Color
            EditorStyles.label.normal.textColor = Color.yellow;

          

            switch (tag)
            {
                // TODO maybe do it in some "clever" way (universal call)
                case "PlantingArea":
                    //GUI.Label(new Rect(x + 20, y, 200, 30), (double)ActionList.plant.length/1000 + " h");
                   EditorGUI.TextField(new Rect(x + 20, y, 100, 35),
                        "Action: " + ActionList.plant.name + "\nTime: " + (double)ActionList.plant.length / 1000 + " h");
                    break;
                case "Carrot":
                    EditorGUI.TextField(new Rect(x + 20, y, 100, 35),
                         "Action: " + ActionList.plant.name + "\nTime:" + (double)ActionList.collectPlant.length / 1000 + " h");
                    break;
            }
        }
    }

}
