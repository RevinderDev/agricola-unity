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
    public int age = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (tag == "Pumpkin")
            materialBasicColor = transform.GetChild(0).GetComponent<Renderer>().material.color;
        else
            materialBasicColor = GetComponent<Renderer>().material.color;
        lastColor = materialBasicColor;
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameController.AddControlledObject(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        gameController.RemoveControlledObject(this);
    }

    void OnMouseEnter()
    {
        if (!isActive || tag == "Player")
            return;
        if (tag == "Pumpkin")
            for (int i = 0; i < 4; i++)
                transform.GetChild(i).GetComponent<Renderer>().material.color = Color.gray;
        lastColor = GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = Color.gray;
        showTooltip = true;
    }
    void OnMouseExit()
    {
        if (!isActive || tag == "Player")
            return;
        if (tag == "Pumpkin")
            for (int i = 0; i < 4; i++)
                transform.GetChild(i).GetComponent<Renderer>().material.color = lastColor;
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
            case "Pumpkin":
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
            case "TakenCowSlot":
                gameController.AddAction(gameObject, ActionType.checkCowStatus);
                break;
            case "FoodHouseCows":
                gameController.AddAction(gameObject, ActionType.feedCow);
                break;
            case "House":
                gameController.AddAction(gameObject, ActionType.eat);
                break;
        }
    }

    void OnGUI()
    {
        if (showTooltip)
        {
            var x = Event.current.mousePosition.x;
            var y = Event.current.mousePosition.y;


            if(tag == "PlantingArea")
                EditorGUI.TextField(new Rect(x -50, y + 20, 100, 35),
                        "Action: " + ActionType.plant.name + 
                        "\nTime: " + (double)ActionType.plant.length / 1000 + " h");
           else if (tag == "Tomato")
            {
                int ripe = (PlantType.tomato.daysToCollect - age) <= 0 ? 0 : (PlantType.tomato.daysToCollect - age);
                int spoiled = (PlantType.tomato.daysToBeSpoiled - age) <= 0 ? 0 : (PlantType.tomato.daysToBeSpoiled - age);
                EditorGUI.TextField(new Rect(x - 50, y + 20, 130, 62),
                        "Action: " + ActionType.collectPlant.name +
                        "\nTime: " + (double)ActionType.collectPlant.length / 1000 + " h" +
                        "\nRipe in: " + ripe + " days" +
                        "\nSpoiled in: " + spoiled + " days");
            }
            else if (tag == "Carrot")
            {
                int ripe = (PlantType.carrot.daysToCollect - age) <= 0 ? 0 : (PlantType.carrot.daysToCollect - age);
                int spoiled = (PlantType.carrot.daysToBeSpoiled - age) <= 0 ? 0 : (PlantType.carrot.daysToBeSpoiled - age);
                EditorGUI.TextField(new Rect(x - 50, y + 20, 130, 62),
                        "Action: " + ActionType.collectPlant.name +
                        "\nTime: " + (double)ActionType.collectPlant.length / 1000 + " h" +
                        "\nRipe in: " + ripe + " days" +
                        "\nSpoiled in: " + spoiled + " days");
            }

            else if (tag == "Pumpkin")
            {
                int ripe = (PlantType.pumpkin.daysToCollect - age) <= 0 ? 0 : (PlantType.pumpkin.daysToCollect - age);
                int spoiled = (PlantType.pumpkin.daysToBeSpoiled - age) <= 0 ? 0 : (PlantType.pumpkin.daysToBeSpoiled - age);
                EditorGUI.TextField(new Rect(x - 50, y + 20, 130, 62),
                        "Action: " + ActionType.collectPlant.name +
                        "\nTime: " + (double)ActionType.collectPlant.length / 1000 + " h" +
                        "\nRipe in: " + ripe + " days" +
                        "\nSpoiled in: " + spoiled + " days");
            }
            else if (tag == "CowSlots")
                EditorGUI.TextField(new Rect(x -50, y + 20, 110, 35),
                        "Action: " + ActionType.buyCow.name + 
                        "\nTime: " + (double)ActionType.buyCow.length / 1000 + " h");
            else if (tag == "Market")
                EditorGUI.TextField(new Rect(x -50, y + 20, 100, 35),
                        "Action: " + ActionType.market.name + 
                        "\nTime: " + (double)ActionType.market.length / 1000 + " h");
            else if (tag == "MilkArea")
            {
                int milkSpoilage = gameController.GetMilkSpoilage();
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
                        + milkSpoilageString + "Count: " + gameController.GetMilkCount());

            }
            else if(tag == "TakenCowSlot")
            {
                AnimalSlot cowSlot = gameController.animalFarm.getSelectedAnimalSlot(gameObject);
                if(cowSlot != null) {
                    Animal cow = cowSlot.animal;
                    EditorGUI.TextField(new Rect(x - 50, y + 20, 100, 50), "Animal: " + cow.getAnimalType().name 
                        + "\nHunger: " + cow.currentHungerLevel + "/" + cow.maxHungerLevel 
                        + "\nAge: " + cow.daysInExistance + "/" + cow.dayOfDeath + " days");
                }
            }
            else if(tag == "FoodHouseCows")
            {
                //TODO: dodac food amount
                EditorGUI.TextField(new Rect(x - 50, y + 20, 100, 35),
                    "Action:" + ActionType.feedCow.name +
                    "\nTime:" + (double)ActionType.feedCow.length / 1000 + " h");
            }
            else if(tag == "House")
                EditorGUI.TextField(new Rect(x - 50, y + 20, 100, 35),
                    "Action: " + ActionType.eat.name +
                    "\nTime: " + (double)ActionType.eat.length / 1000 + " h");
        }
    }

}
