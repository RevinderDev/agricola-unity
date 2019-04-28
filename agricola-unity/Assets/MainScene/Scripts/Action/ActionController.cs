using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
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
        if(lastColor != new Color(0, 0, 0, 0))
            GetComponent<Renderer>().material.color = lastColor;
        showTooltip = false;
    }

    private void OnMouseDown()
    {
        if (!isActive)
            return;
        showTooltip = false;
        GetComponent<Renderer>().material.color = lastColor;
        switch (tag)
        {
            case "PlantingArea":
                isActive = false;
                gameController.dropdown.Display(gameObject);
                break;
            case "Carrot":
                gameController.AddAction(gameObject, ActionType.collectPlant);
                break;
            case "Tomato":
                gameController.AddAction(gameObject, ActionType.collectPlant);
                break;
            case "CowSlots":
                gameController.AddAction(gameObject, ActionType.buyCow);
                break;
            case "Market":
                gameController.AddAction(gameObject, ActionType.market);
                break;
            case "MilkArea":
                gameController.AddAction(gameObject, ActionType.gatherMilk);
                break;
        }
    }

    void OnGUI()
    {
        if (showTooltip)
        {
            var x = Event.current.mousePosition.x;
            var y = Event.current.mousePosition.y;

            switch (tag)
            {
                // TODO maybe do it in some "clever" way (universal call)
                case "PlantingArea":
                   EditorGUI.TextField(new Rect(x -50, y + 20, 100, 35),
                        "Action: " + ActionType.plant.name + "\nTime: " + (double)ActionType.plant.length / 1000 + " h");
                    break;
                case "Carrot":
                    EditorGUI.TextField(new Rect(x - 50, y + 20, 100, 35),
                         "Action: " + ActionType.plant.name + "\nTime: " + (double)ActionType.collectPlant.length / 1000 + " h");
                    break;
                case "CowSlots":
                    EditorGUI.TextField(new Rect(x -50, y + 20, 100, 35),
                         "Action: " + ActionType.buyCow.name + "\nTime: " + (double)ActionType.buyCow.length / 1000 + " h");
                    break;
                case "Market":
                    EditorGUI.TextField(new Rect(x -50, y + 20, 100, 35),
                         "Action: " + ActionType.market.name + "\nTime: " + (double)ActionType.market.length / 1000 + " h");
                    break;
                case "MilkArea":
                    int milkSpoilage = gameController.getMilkSpoilage();
                    string milkSpoilageString = "";
                    int windowResizeY = 48;
                    int windowResizeX = 110;
                    if (milkSpoilage > 0)
                    {
                        milkSpoilageString = "Spoiled in: " + milkSpoilage + " days \n";
                        windowResizeY = 65;
                    }
                    EditorGUI.TextField(new Rect(x - 50, y + 20, windowResizeX, windowResizeY),
                         "Action: " + ActionType.gatherMilk.name + "\nTime: " + (double)ActionType.gatherMilk.length / 1000 + " h\n"
                         + milkSpoilageString +"Count: " + gameController.getMilkCount());
                    break;
            }
        }
    }

}
