using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AnimalFarm
{
    private Dictionary<string, int> animalCount;  //here we store how many animals of each type we have
    private List<Animal> listOfAnimals;
    private Dictionary<string, GameObject> farmsGameObjects;
    GameController gameController;

    private readonly Vector3 COW_FARM_POSITION;
    private readonly Vector3 PIG_FARM_POSITION;
    private readonly Vector3 SHEEP_FARM_POSITION;
    private readonly Vector3 ANIMAL_SCALE;
    private readonly Vector3 ANIMAL_ROTATION;

    private AnimalFactory animalFactory = AnimalFactory.getInstance();

    public AnimalFarm()
    {
        ANIMAL_SCALE = new Vector3(1, 1, 1);
        COW_FARM_POSITION = new Vector3(10, 0.5f, -10);
        PIG_FARM_POSITION = new Vector3(13, -0.8f, -16);
        SHEEP_FARM_POSITION = new Vector3(13, -0.8f, 0);
        ANIMAL_ROTATION = new Vector3(0, -90, 0);

        farmsGameObjects = new Dictionary<string, GameObject>();
        animalCount = new Dictionary<string, int>();
        listOfAnimals = new List<Animal>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();

        initStartingAnimals();
        initFarms();
    }


    private void initFarms()
    {
        // TODO: Is broken, find a way to get reference to farm objects.
        //farmsGameObjects.Add("CowFarm", GameObject.Find("CowFarm").GetComponent<GameObject>());
        //farmsGameObjects.Add("SheepFarm", GameObject.Find("SheepFarm").GetComponent<GameObject>());
        //farmsGameObjects.Add("PigFarm", GameObject.Find("PigFarm").GetComponent<GameObject>());
        //foreach (KeyValuePair<string, GameObject> entry in farmsGameObjects)
        //{
        //    entry.Value.AddComponent<ActionController>();
        //}
    }

    private void initStartingAnimals()
    {
        addCow();
    }

    public void addCow()
    {
        
        Object prefab = AssetDatabase.LoadAssetAtPath(animalFactory.getCowPrefab(), typeof(GameObject));
        GameObject cloneCow = gameController.InstantiatePrefab(prefab, Vector3.zero, Quaternion.identity) as GameObject;
        Animal newCow = animalFactory.buildCow(cloneCow);

        cloneCow.transform.localScale = ANIMAL_SCALE;
        cloneCow.transform.position = COW_FARM_POSITION;
        cloneCow.transform.eulerAngles = ANIMAL_ROTATION;
        cloneCow.tag = newCow.getAnimalType().name;
    }

}
