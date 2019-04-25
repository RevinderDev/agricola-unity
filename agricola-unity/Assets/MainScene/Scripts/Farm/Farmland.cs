using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class PlantType
{
    public readonly string name;
    public readonly float growthPerDay;
    public readonly int daysToCollect;
    public readonly int daysToBeSpoiled;
    public readonly string prefabDirectory;
    public readonly Item.ItemType itemType;

    public PlantType(string name, float growthPerDay, int daysToCollect, int daysToBeSpoiled, string prefabDirectory, Item.ItemType itemType)
    {
        this.name = name;
        this.growthPerDay = growthPerDay;
        this.daysToBeSpoiled = daysToBeSpoiled;
        this.daysToCollect = daysToCollect;
        this.prefabDirectory = prefabDirectory;
        this.itemType = itemType;
    }
}

public class Plant
{
    private GameObject gameObject;
    private readonly PlantType plantType;
    private int daysOfExistence;
    private bool isSpoiled;

    public Plant(GameObject gameObject, PlantType plantType)
    {
        this.gameObject = gameObject;
        this.plantType = plantType;
        daysOfExistence = 0;
        isSpoiled = false;
    }

    public PlantType GetPlantType()
    {
        return plantType;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public int GetDaysOfExistence()
    {
        return daysOfExistence;
    }

    public void AddDayOfExsistence()
    {
        daysOfExistence++;
    }

    public bool IsSpoiled()
    {
        return isSpoiled;
    }

    public void Spoil()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.black;
        isSpoiled = true;
    }
}

/*
* Creates areas for plants. It is to manage planting plants, allows to add new areas.
*/
public class Farmland
{
    private Dictionary<GameObject, GameObject> plantsToAreaMap; //Plant, Area
    private List<Plant> plants;
    private readonly float interspace;
    private Vector3 scale;
    private int count;
    private Vector3 position;
    GameController gameController;

    // Name of PlantType must exist in project tags!
    public readonly PlantType carrot = new PlantType("Carrot", 0.1f, 2, 4, 
        "Assets/simple_low_poly_village_buildings/models/carrot2.prefab", Item.ItemType.carrot);

    public Farmland()
    {
        scale = new Vector3(3, 0.25f, 3);
        position = new Vector3(-5, 0.5f, -10);
        interspace = 3.5f;
        count = 6;
        plantsToAreaMap = new Dictionary<GameObject, GameObject>();
        plants = new List<Plant>();
        // Create planting areas
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                CreateArea(i, j);
            }
        }
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    private void CreateArea(int x, int z)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = scale;
        cube.transform.position = new Vector3(position.x + x * interspace, position.y, position.z + z * interspace);
        cube.GetComponent<Renderer>().material = Resources.Load("terrain", typeof(Material)) as Material;
        cube.AddComponent<ActionController>();
        cube.tag = "PlantingArea";
    }

    public void AddPlant(GameObject areaObject, PlantType type)
    {
        Object prefab = AssetDatabase.LoadAssetAtPath(type.prefabDirectory, typeof(GameObject));
        GameObject clone = gameController.InstantiatePrefab(prefab, Vector3.zero, Quaternion.identity) as GameObject;
        clone.transform.localScale = new Vector3(15, 15, 15);
        clone.transform.position = new Vector3(areaObject.transform.position.x, -0.1f, areaObject.transform.position.z);
        clone.AddComponent<ActionController>();
        clone.AddComponent<BoxCollider>();
        clone.tag = type.name;
        plants.Add(new Plant(clone, carrot));
        // Take the area
        plantsToAreaMap.Add(clone, areaObject);
    }

    public bool IsAreaTaken(GameObject gameObject)
    {
        if (plantsToAreaMap.ContainsValue(gameObject))
            return true;
        return false; 
    }

    public bool CanPlantBeCollected(GameObject gameObject)
    {
        foreach(Plant plant in plants)
        {
            if (plant.GetGameObject().Equals(gameObject))
            {
                return plant.GetDaysOfExistence() >= plant.GetPlantType().daysToCollect && !plant.IsSpoiled();
            }
        }
        return false;
    }

    public void CollectPlant(GameObject plantObject)
    {
        for (int i = 0; i < plants.Count; i++)
        {
            if (plants[i].GetGameObject().Equals(plantObject))
            {
                gameController.inventory.AddItem(plants[i].GetPlantType().itemType);
                plants.RemoveAt(i);
                
                break;
            }
        }
       
        GameController.RemoveGameObject(plantObject);
        // Free the area
        plantsToAreaMap.Remove(plantObject);
    }

    public void GrowPlants()
    {
        for (int i = 0; i<plants.Count; i++)
        {
            if (plants[i].GetGameObject() == null)
                plants.RemoveAt(i);
            else
            {
                Transform t = plants[i].GetGameObject().transform;
                //Grow
                if (plants[i].GetDaysOfExistence() < plants[i].GetPlantType().daysToCollect)
                    t.position = new Vector3(t.position.x, t.position.y + plants[i].GetPlantType().growthPerDay, t.position.z);
                //Spoiled
                else if (plants[i].GetDaysOfExistence() == plants[i].GetPlantType().daysToBeSpoiled)
                    plants[i].Spoil();
                //Plant is getting older :(
                plants[i].AddDayOfExsistence();
            }
        }

    }
}

