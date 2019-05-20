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

    public void OnMouseEnter()
    {
        lastColor = GetComponent<Renderer>().material.color;
        if (!isActive || tag == "Player")
            return;
        if (tag == "Pumpkin")
            for (int i = 0; i < 4; i++)
                transform.GetChild(i).GetComponent<Renderer>().material.color = Color.gray;
        GetComponent<Renderer>().material.color = Color.gray;
        showTooltip = true;
    }
    public void OnMouseExit()
    {
        if (!isActive || tag == "Player")
            return;
        if (tag == "Pumpkin")
            for (int i = 0; i < 4; i++)
                transform.GetChild(i).GetComponent<Renderer>().material.color = materialBasicColor;
        GetComponent<Renderer>().material.color = materialBasicColor;
        showTooltip = false;
        Tooltip.Hidetooltip_Static();
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
                gameController.AddAction(gameObject, ActionType.placeCow);
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
            case "EggArea":
                gameController.AddAction(gameObject, ActionType.gatherEgg);
                break;
            case "ChickenSlot":
                gameController.AddAction(gameObject, ActionType.placeChicken);
                break;
            case "TakenChickenSlot":
                gameController.AddAction(gameObject, ActionType.checkChickenStatus);
                break;
            case "FoodHouseChickens":
                gameController.AddAction(gameObject, ActionType.feedChicken);
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
            //var x = Event.current.mousePosition.x;
            //var y = Event.current.mousePosition.y;

            var x = Input.mousePosition.x + 60;
            var y = Input.mousePosition.y - 10;
            Vector2 newTooltipPosition = new Vector2(x, y);
            Tooltip.ChangePosition(newTooltipPosition);


            if (tag == "PlantingArea")
                Tooltip.ShowTooltip_Static("Action: " + ActionType.plant.name +
                        "\nTime: " + (double)ActionType.plant.length / 1000 + " h");
           else if (tag == "Tomato")
            {
                int ripe = (PlantType.tomato.daysToCollect - age) <= 0 ? 0 : (PlantType.tomato.daysToCollect - age);
                int spoiled = (PlantType.tomato.daysToBeSpoiled - age) <= 0 ? 0 : (PlantType.tomato.daysToBeSpoiled - age);
                Tooltip.ShowTooltip_Static("Action: " + ActionType.collectPlant.name +
                        "\nTime: " + (double)ActionType.collectPlant.length / 1000 + " h" +
                        "\nRipe in: " + ripe + " days" +
                        "\nSpoiled in: " + spoiled + " days");
            }
            else if (tag == "Carrot")
            {
                int ripe = (PlantType.carrot.daysToCollect - age) <= 0 ? 0 : (PlantType.carrot.daysToCollect - age);
                int spoiled = (PlantType.carrot.daysToBeSpoiled - age) <= 0 ? 0 : (PlantType.carrot.daysToBeSpoiled - age);
                Tooltip.ShowTooltip_Static("Action: " + ActionType.collectPlant.name +
                        "\nTime: " + (double)ActionType.collectPlant.length / 1000 + " h" +
                        "\nRipe in: " + ripe + " days" +
                        "\nSpoiled in: " + spoiled + " days");
            }

            else if (tag == "Pumpkin")
            {
                int ripe = (PlantType.pumpkin.daysToCollect - age) <= 0 ? 0 : (PlantType.pumpkin.daysToCollect - age);
                int spoiled = (PlantType.pumpkin.daysToBeSpoiled - age) <= 0 ? 0 : (PlantType.pumpkin.daysToBeSpoiled - age);
                Tooltip.ShowTooltip_Static("Action: " + ActionType.collectPlant.name +
                        "\nTime: " + (double)ActionType.collectPlant.length / 1000 + " h" +
                        "\nRipe in: " + ripe + " days" +
                        "\nSpoiled in: " + spoiled + " days");
            }
            else if (tag == "CowSlots")
                Tooltip.ShowTooltip_Static("Action: " + ActionType.placeCow.name +
                        "\nTime: " + (double)ActionType.placeCow.length / 1000 + " h");
            else if (tag == "Market")
                Tooltip.ShowTooltip_Static("Action: " + ActionType.market.name +
                        "\nTime: " + (double)ActionType.market.length / 1000 + " h");
            else if (tag == "MilkArea")
            {

                int milkSpoilage = gameController.GetMilkSpoilage();
                string milkSpoilageString = "";
                if (milkSpoilage > 0)
                {
                    milkSpoilageString = "Spoiled in: " + milkSpoilage + " days \n";
                }
                Tooltip.ShowTooltip_Static("Action: " + ActionType.gatherMilk.name + "\nTime: " + (double)ActionType.gatherMilk.length / 1000 + " h\n"
                    + milkSpoilageString + "Count: " + gameController.GetMilkCount());
            }
            else if(tag == "TakenCowSlot")
            {
                AnimalSlot cowSlot = gameController.animalFarm.getSelectedAnimalSlot(gameObject, "cow");
                if(cowSlot != null) {
                    Animal cow = cowSlot.animal;
                    Tooltip.ShowTooltip_Static("Animal: " + cow.getAnimalType().name
                        + "\nHunger: " + cow.currentHungerLevel + "/" + cow.maxHungerLevel
                        + "\nAge: " + cow.daysInExistance + "/" + cow.dayOfDeath + " days");
                }
            }
            else if(tag == "FoodHouseCows")
            {
                Tooltip.ShowTooltip_Static("Action:" + ActionType.feedCow.name +
                    "\nTime:" + (double)ActionType.feedCow.length / 1000 + " h"
                    + "\nFood amount:" + gameController.animalFarm.getAnimalFood("cows"));
            }
            else if(tag == "House")
                Tooltip.ShowTooltip_Static("Action: " + ActionType.eat.name +
                    "\nTime: " + (double)ActionType.eat.length / 1000 + " h");
            else if (tag == "ChickenSlot")
                Tooltip.ShowTooltip_Static("Action: " + ActionType.placeChicken.name +
                        "\nTime: " + (double)ActionType.placeChicken.length / 1000 + " h");
            else if (tag == "EggArea")
            {
                int eggSpoilage = gameController.GetEggSpoilage();
                string eggSpoilageString = "";
                if (eggSpoilage > 0)
                {
                    eggSpoilageString = "Spoiled in: " + eggSpoilage + " days \n";
                }
                Tooltip.ShowTooltip_Static("Action: " + ActionType.gatherEgg.name + "\nTime: " + (double)ActionType.gatherEgg.length / 1000 + " h\n"
                        + eggSpoilageString + "Count: " + gameController.GetEggCount());

            }
            else if (tag == "TakenChickenSlot")
            {
                AnimalSlot chickenSlot = gameController.animalFarm.getSelectedAnimalSlot(gameObject, "chicken");
                if (chickenSlot != null)
                {
                    Animal chicken = chickenSlot.animal;
                    Tooltip.ShowTooltip_Static("Animal: " + chicken.getAnimalType().name
                        + "\nHunger: " + chicken.currentHungerLevel + "/" + chicken.maxHungerLevel
                        + "\nAge: " + chicken.daysInExistance + "/" + chicken.dayOfDeath + " days");
                }
            }
            else if (tag == "FoodHouseChickens")
            {
                Tooltip.ShowTooltip_Static("Action:" + ActionType.feedChicken.name
                    + "\nTime:" + (double)ActionType.feedChicken.length / 1000 + " h"
                    + "\nFood amount:" + gameController.animalFarm.getAnimalFood("chickens"));
            }
        }
    }

}
