using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AnimalFarm
{
    private List<AnimalSlot> cowSlotsList;
    GameController gameController;

    private Vector3 newestMilkPosition;
    private Vector3 milkAreaInitPosition;
    public List<GeneratedFoodProduct> milksList { get; }
    private GameObject milkArea { get; }
    private readonly Vector3 MilkScale;
    private int milkDaysSpoilage;

    private readonly Vector3 AnimalScale;
    private readonly Vector3 AnimalRotation;
    private readonly int ErrorCode = -1;

    private AnimalFactory animalFactory = AnimalFactory.getInstance();

    public AnimalFarm()
    {
        AnimalScale = new Vector3(0.8f, 0.8f, 0.8f);
        AnimalRotation = new Vector3(0, -90, 0);



        gameController = GameObject.Find("GameController").GetComponent<GameController>();


        MilkScale = new Vector3(1.5f, 1.5f, 1.5f);
        milksList = new List<GeneratedFoodProduct>();
        milkArea = GameObject.FindGameObjectWithTag("MilkArea");
        newestMilkPosition = milkArea.transform.position;
        milkAreaInitPosition = milkArea.transform.position;
        milkDaysSpoilage = -1;

        initStartingAnimals();
        initFarmSlots();
    }

    public void ageAnimals()
    {
        foreach(AnimalSlot cowSlot in cowSlotsList)
        {
            if(cowSlot.animal != null)
                cowSlot.animal.AddDayInExistance();
        }
    }

    private void initFarmSlots()
    {
        GameObject[] cowSlotsObjects = GameObject.FindGameObjectsWithTag("CowSlots");
        cowSlotsList = new List<AnimalSlot>();
        for(int i = 0; i < cowSlotsObjects.Length; i++)
        {
            AnimalSlot animalSlot = new AnimalSlot(cowSlotsObjects[i]);
            cowSlotsList.Add(animalSlot);
        }
    }


    public AnimalSlot getSelectedAnimalSlot(GameObject cowSlotObject)
    {
        int slotIndex = getSlotIndex(cowSlotObject, cowSlotsList);
        if (slotIndex != ErrorCode)
            return cowSlotsList[slotIndex];


        return null;
    }
    

    public void generateFoodProducts()
    {
       foreach(var cowSlot in cowSlotsList)
        {
            if(cowSlot.animal != null)
            {
                GeneratedFoodProduct milk = cowSlot.animal.getAnimalType().generatedResource;
                milksList.Add(milk);
                Object prefab = AssetDatabase.LoadAssetAtPath(milk.prefabDirectory, typeof(GameObject));
                GameObject milkClone = gameController.InstantiatePrefab(prefab, Vector3.zero, Quaternion.identity) as GameObject;
                milkClone.transform.position = newestMilkPosition;
                milkClone.transform.localScale = MilkScale;
                milkClone.tag = "Milk";
                recalculateMilkPosition();
                if(milkDaysSpoilage <= 0)
                    milkDaysSpoilage = 1;
            }
        }
    }


    public int getMilkSpoilage()
    {
        if (milksList.Count > 0)
            return milkDaysSpoilage;
        else
            return ErrorCode;
    }


    public int getMilkCount()
    {
        return GameObject.FindGameObjectsWithTag("Milk").Length;
    }

    public void gatherMilk()
    {
        if(milksList.Count > 0)
        {
            //gameController.inventory.AddItem(plants[i].GetPlantType().itemType);
            GameObject[] milks = GameObject.FindGameObjectsWithTag("Milk");
            gameController.inventory.AddItem(ItemType.milk, milks.Length);
            milksList.Clear();
            foreach (var milk in milks)
            {
                GameController.RemoveGameObject(milk);
            }

            GameObject[] spoiledMilks = GameObject.FindGameObjectsWithTag("SpoiledMilk");
            foreach(var milk in spoiledMilks)
            {
                GameController.RemoveGameObject(milk);
            }

            newestMilkPosition = milkAreaInitPosition;
            milkArea.transform.position = milkAreaInitPosition;
            milkArea.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            milkDaysSpoilage = 1;
        }
    }


    private void recalculateMilkPosition()
    {
        if (newestMilkPosition.z < milkAreaInitPosition.z + 4.0f)
        {
            newestMilkPosition.z += 0.4f;
            int MaxMilkCapacity = 11;
            if(milksList.Count <= MaxMilkCapacity) {
                milkArea.transform.localScale = new Vector3(milkArea.transform.localScale.x, milkArea.transform.localScale.y, milkArea.transform.localScale.z + 0.05f);
                milkArea.transform.position = new Vector3(milkArea.transform.position.x, milkArea.transform.position.y, milkArea.transform.position.z + 0.175f);
            }
        }
        else
        {
            newestMilkPosition.z = 0.7f;
            newestMilkPosition.x -= 0.4f;
            milkArea.transform.localScale = new Vector3(milkArea.transform.localScale.x + 0.01f, milkArea.transform.localScale.y, milkArea.transform.localScale.z);
        }
    }

    private void initStartingAnimals()
    {
        //addCow();
    }

    public bool isSlotAvailable(GameObject slotObject)
    {
        switch(slotObject.tag)
        {
            case "CowSlots":
                int slotIndex = getSlotIndex(slotObject, cowSlotsList);
                if(slotIndex != ErrorCode)
                {
                    return !cowSlotsList[slotIndex].isSlotTaken();
                }
                break;
        }

       return false;
    }

    public int getSlotIndex(GameObject slotObject, List<AnimalSlot> slotsList)
    {
        foreach(var animalSlot in slotsList)
        {
            if(animalSlot.slotGameObject == slotObject)
            {
                return slotsList.IndexOf(animalSlot);
            }
        }

        return -1;
    }



    public void spoilFood()
    {
        if(milksList.Count > 0)
        {
            milkDaysSpoilage -= 1;
            if (milkDaysSpoilage <= 0)
            {
                GameObject[] milksToBeSpoiled = GameObject.FindGameObjectsWithTag("Milk");
                foreach (var milk in milksToBeSpoiled)
                {
                    milk.tag = "SpoiledMilk";
                    milk.GetComponent<Renderer>().material = Resources.Load("Milk_Spoiled_Test", typeof(Material)) as Material;
                }
            }
        }
    }

    public void addCow(GameObject slotGameObject)
    {
        Object prefab = AssetDatabase.LoadAssetAtPath(animalFactory.getCowPrefab(), typeof(GameObject));
        GameObject cloneCow = gameController.InstantiatePrefab(prefab, Vector3.zero, Quaternion.identity) as GameObject;
        Animal newCow = animalFactory.buildCow(cloneCow);

        int slotIndex = getSlotIndex(slotGameObject, cowSlotsList);

        if (slotIndex != ErrorCode)
        {
            cowSlotsList[slotIndex].takeSlot(newCow);

            cloneCow.transform.localScale = AnimalScale;
            cloneCow.transform.position = cowSlotsList[slotIndex].slotGameObject.transform.position;
            cloneCow.transform.eulerAngles = AnimalRotation;
            //cloneCow.AddComponent<ActionController>(); TODO: Problem z kolorami w teksturze.
            cloneCow.tag = newCow.getAnimalType().name;
            slotGameObject.tag = "TakenCowSlot";
        }
        else
            Debug.Log("Error adding cow (addCow) error code: " + slotIndex);
    }

    public void feedCow(GameObject cowSlot)
    {
        //
    }

}


public class AnimalSlot
{
    public Animal animal { get; set; }
    private bool isTaken = false;
    public GameObject slotGameObject { get; }


    public AnimalSlot(GameObject slotGameObject)
    {
        this.slotGameObject = slotGameObject;
    }


    public bool isSlotTaken()
    {
        return isTaken;
    }

    public void takeSlot(Animal animal)
    {
        if (!isTaken)
        {
            this.animal = animal;
            isTaken = true;
        }
    }

    public void removeAnimal()
    {
        if(animal != null && isTaken)
        {
            animal = null;
            isTaken = false;
        }
    }

}