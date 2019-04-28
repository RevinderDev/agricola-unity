using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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

    public Farmland()
    {
        scale = new Vector3(2f, 0.25f, 2f);
        position = new Vector3(-14f, 0.5f, 2f);
        interspace = 2.3f;
        count = 20;
        plantsToAreaMap = new Dictionary<GameObject, GameObject>();
        plants = new List<Plant>();
        // Create planting areas
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 5; j++)
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
        Object prefab = AssetDatabase.LoadAssetAtPath(type.directory, typeof(GameObject));
        GameObject clone = gameController.InstantiatePrefab(prefab, Vector3.zero, Quaternion.identity) as GameObject;
        clone.transform.localScale = new Vector3(10, 10, 10);
        clone.transform.position = new Vector3(areaObject.transform.position.x, type.startPosision, areaObject.transform.position.z);
        clone.AddComponent<ActionController>();
        clone.AddComponent<BoxCollider>();
        clone.tag = type.name;
        plants.Add(new Plant(clone, type));
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

