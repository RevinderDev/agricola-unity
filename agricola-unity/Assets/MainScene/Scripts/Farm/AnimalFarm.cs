using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AnimalFarm
{

    GameController gameController;

    private List<AnimalSlot> cowSlotsList;
    private Vector3 newestMilkPosition;
    private Vector3 milkAreaInitPosition;
    public List<GeneratedFoodProduct> milksList { get; }
    private GameObject milkArea { get; }
    private readonly Vector3 MilkScale;
    private int milkDaysSpoilage;
    private int cowFood;


    private List<AnimalSlot> chickenSlotsList;
    private GameObject eggArea { get; }
    public List<GeneratedFoodProduct> eggsList { get; }
    private Vector3 newestEggPosition;
    private readonly Vector3 EggScale;
    private Vector3 eggAreaInitPosition;
    private int eggDaysSpoilage;
    private int chickenFood;



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
        cowFood = 0;


        eggArea = GameObject.FindGameObjectWithTag("EggArea");
        eggsList = new List<GeneratedFoodProduct>();
        newestEggPosition = eggArea.transform.position + new Vector3(0, 0.1f, 0); //Correction for egg to be above ground
        eggAreaInitPosition = newestEggPosition;
        EggScale = new Vector3(0.75f, 0.75f, 0.75f);
        eggDaysSpoilage = -1;
        chickenFood = 0;
        

        initStartingAnimals();
        initFarmSlots();
    }


    public void addAnimalFood(string animalName, int totalFood)
    {
        switch(animalName)
        {
            case "cows":
                cowFood += totalFood;
                return;
            case "chickens":
                chickenFood += totalFood;
                return;
        }
    }


    public void animalsEat()
    {
        bool hungryAnimalFound = false;
        while (cowFood > 0)
        {
            hungryAnimalFound = false;
            foreach (var cowSlot in cowSlotsList)
            {
                if (cowSlot.animal != null)
                {
                    if (cowSlot.animal.isHungry() && cowFood > 0)
                    {
                        cowSlot.animal.eat(ref cowFood);
                        hungryAnimalFound = true;
                    }
                }
            }
            if (hungryAnimalFound == false)
                break;
        }

        while (chickenFood > 0)
        {
            hungryAnimalFound = false;
            foreach (var chickenSlot in chickenSlotsList)
            {
                if (chickenSlot.animal != null)
                {
                    if (chickenSlot.animal.isHungry() && chickenFood > 0) { 
                        chickenSlot.animal.eat(ref chickenFood);
                        hungryAnimalFound = true;
                    }
                }
            }
            if (hungryAnimalFound == false)
                break;
        }
    }


    public int? getAnimalFood(string animalName)
    {
        switch (animalName)
        {
            case "cows":
                return cowFood;
            case "chickens":
                return chickenFood;
        }

        return null;
    }

    public void ageAnimals()
    {
        foreach(AnimalSlot cowSlot in cowSlotsList)
        {
            if (cowSlot.animal != null)
            {
                cowSlot.animal.AddDayInExistance();
                if (cowSlot.animal.isDead)
                {
                    gameController.DisplayInfo("One of your cows died :(");
                    cowSlot.removeAnimal();
                }
            }
        }

        foreach (AnimalSlot chickenSlot in chickenSlotsList)
        {
            if (chickenSlot.animal != null)
            {
                chickenSlot.animal.AddDayInExistance();
                if (chickenSlot.animal.isDead)
                {
                    gameController.DisplayInfo("One of your chickens died :(");
                    chickenSlot.removeAnimal();
                }
            }
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


        GameObject[] chickenSlotsObjects = GameObject.FindGameObjectsWithTag("ChickenSlot");
        chickenSlotsList = new List<AnimalSlot>();
        for(int i = 0; i < chickenSlotsObjects.Length; i++)
        {
            AnimalSlot animalSlot = new AnimalSlot(chickenSlotsObjects[i]);
            chickenSlotsList.Add(animalSlot);
        }

    }


    public AnimalSlot getSelectedAnimalSlot(GameObject animalSlotObject, string animalName)
    {

        int slotIndex;
        switch(animalName)
        {
            case "cow":
                slotIndex = getSlotIndex(animalSlotObject, cowSlotsList);
                return cowSlotsList[slotIndex];
            case "chicken":
                slotIndex = getSlotIndex(animalSlotObject, chickenSlotsList);
                return chickenSlotsList[slotIndex];
        }


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

       foreach(var chickenSlot in chickenSlotsList)
        {
            if(chickenSlot.animal != null)
            {
                GeneratedFoodProduct egg = chickenSlot.animal.getAnimalType().generatedResource;
                eggsList.Add(egg);
                Object prefab = AssetDatabase.LoadAssetAtPath(egg.prefabDirectory, typeof(GameObject));
                GameObject eggClone = gameController.InstantiatePrefab(prefab, Vector3.zero, Quaternion.identity) as GameObject;
                eggClone.tag = "Egg";
                eggClone.transform.position = newestEggPosition;
                eggClone.transform.localScale = EggScale;
                recalculateEggPosition();
                if (eggDaysSpoilage <= 0)
                    eggDaysSpoilage = 1;
            }
        }
    }


    public void recalculateEggPosition()
    {
        if (newestEggPosition.z < eggAreaInitPosition.z + 4.0f)
        {
            newestEggPosition.z += 0.4f;
            int MaxEggCapacity = 11;
            if (eggsList.Count <= MaxEggCapacity)
            {
                eggArea.transform.localScale = new Vector3(eggArea.transform.localScale.x, eggArea.transform.localScale.y, eggArea.transform.localScale.z + 0.05f);
                eggArea.transform.position = new Vector3(eggArea.transform.position.x, eggArea.transform.position.y, eggArea.transform.position.z + 0.175f);
            }
        }
        else
        {
            newestEggPosition.z = 0.7f;
            newestEggPosition.x -= 0.4f;
            eggArea.transform.localScale = new Vector3(eggArea.transform.localScale.x + 0.01f, eggArea.transform.localScale.y, eggArea.transform.localScale.z);
        }
    }

    public int getMilkSpoilage()
    {
        if (milksList.Count > 0)
            return milkDaysSpoilage;
        else
            return ErrorCode;
    }

    public int getEggSpoilage()
    {
        if (eggsList.Count > 0)
            return eggDaysSpoilage;
        else
            return ErrorCode;
    }

    public int getMilkCount()
    {
        return GameObject.FindGameObjectsWithTag("Milk").Length;
    }

    public int getEggCount()
    {
        return GameObject.FindGameObjectsWithTag("Egg").Length;
    }

    public void gatherEgg()
    {
        if (eggsList.Count > 0)
        {
            GameObject[] eggs = GameObject.FindGameObjectsWithTag("Egg");
            gameController.inventory.AddItem(ItemType.egg, eggs.Length);
            eggsList.Clear();
            foreach (var egg in eggs)
            {
                GameController.RemoveGameObject(egg);
            }

            GameObject[] spoiledEggs = GameObject.FindGameObjectsWithTag("SpoiledEgg");
            foreach (var egg in spoiledEggs)
            {
                GameController.RemoveGameObject(egg);
            }

            newestEggPosition = eggAreaInitPosition;
            eggArea.transform.position = eggAreaInitPosition;
            eggArea.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            eggDaysSpoilage = 1;
        }
    }

    public void gatherMilk()
    {
        if(milksList.Count > 0)
        {
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
        int slotIndex;
        switch (slotObject.tag)
        {
            case "CowSlots":
                slotIndex = getSlotIndex(slotObject, cowSlotsList);
                if(slotIndex != ErrorCode)
                {
                    return !cowSlotsList[slotIndex].isSlotTaken();
                }
                break;
            case "ChickenSlot":
                slotIndex = getSlotIndex(slotObject, chickenSlotsList);
                if (slotIndex != ErrorCode)
                    return !chickenSlotsList[slotIndex].isSlotTaken();
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

        if (eggsList.Count > 0)
        {
            eggDaysSpoilage -= 1;
            if (eggDaysSpoilage <= 0)
            {
                GameObject[] eggsToBeSpoiled = GameObject.FindGameObjectsWithTag("Egg");
                foreach (var egg in eggsToBeSpoiled)
                {
                    egg.tag = "SpoiledEgg";
                    egg.GetComponent<Renderer>().material = Resources.Load("Milk_Spoiled_Test", typeof(Material)) as Material;
                }
            }
        }
    }

    public void addChicken(GameObject slotGameObject)
    {
        Object prefab = AssetDatabase.LoadAssetAtPath(animalFactory.getChickenPrefab(), typeof(GameObject));
        GameObject chickenCloneObject = gameController.InstantiatePrefab(prefab, Vector3.zero, Quaternion.identity) as GameObject;
        Animal newChicken = animalFactory.buildChicken(chickenCloneObject);

        int slotIndex = getSlotIndex(slotGameObject, chickenSlotsList);

        if (slotIndex != ErrorCode)
        {
            chickenSlotsList[slotIndex].takeSlot(newChicken);
            chickenSlotsList[slotIndex].animalGameObject = chickenCloneObject;

            // Scale?
            chickenCloneObject.transform.position = chickenSlotsList[slotIndex].slotGameObject.transform.position;
            chickenCloneObject.transform.eulerAngles = AnimalRotation;
            chickenCloneObject.tag = newChicken.getAnimalType().name;
        }
        else
            Debug.Log("Error adding chicken (addChicken) error code: " + slotIndex);

    }

    public void addCow(GameObject slotGameObject)
    {
        Object prefab = AssetDatabase.LoadAssetAtPath(animalFactory.getCowPrefab(), typeof(GameObject));
        GameObject cloneCowGameObject = gameController.InstantiatePrefab(prefab, Vector3.zero, Quaternion.identity) as GameObject;
        Animal newCow = animalFactory.buildCow(cloneCowGameObject);

        int slotIndex = getSlotIndex(slotGameObject, cowSlotsList);

        if (slotIndex != ErrorCode)
        {
            cowSlotsList[slotIndex].takeSlot(newCow);
            cowSlotsList[slotIndex].animalGameObject = cloneCowGameObject;

            cloneCowGameObject.transform.localScale = AnimalScale;
            cloneCowGameObject.transform.position = cowSlotsList[slotIndex].slotGameObject.transform.position;
            cloneCowGameObject.transform.eulerAngles = AnimalRotation;
            cloneCowGameObject.tag = newCow.getAnimalType().name;
        }
        else
            Debug.Log("Error adding cow (addCow) error code: " + slotIndex);
    }
}


public class AnimalSlot
{
    public Animal animal { get; set; }
    private bool isTaken = false;
    public GameObject slotGameObject { get; }
    public GameObject animalGameObject { get; set; }


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
            switch(animal.getAnimalType().name)
            {
                case "Cow":
                    slotGameObject.tag = "TakenCowSlot";
                    break;
                case "Chicken":
                    slotGameObject.tag = "TakenChickenSlot";
                    break;
            }
        }
    }

    public void removeAnimal()
    {
        if(animal != null && isTaken)
        {
            switch (animal.getAnimalType().name)
            {
                case "Cow":
                    slotGameObject.tag = "CowSlots";
                    break;
                case "Chicken":
                    slotGameObject.tag = "ChickenSlot";
                    break;
            }
            animal = null;
            isTaken = false;
            GameController.RemoveGameObject(animalGameObject);
            animalGameObject = null;
        }
    }

}