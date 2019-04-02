using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/*
* Creates areas for plants. It is to manage planting plants, allows to add new areas.
* Stores areas positions (to plant over them), cubes (represents areas) and (in future) plants.
*/
public class Farmland
{
    private List<Vector3> positions;
    private List<GameObject> areaCubes;
    private List<GameObject> plants; //Not used yet for accesing plants (deleting, moving etc.)
    private float interspace;
    private Vector3 scale;
    private int count;
    private Vector3 position;
    GameController gameController;

    public Farmland()
    {
        scale = new Vector3(3, 0.25f, 3);
        position = new Vector3(-5, 0.5f, -10);
        interspace = 3.5f;
        count = 6;
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
    }

    public void AddPlant(Vector3 position)
    {
        Object prefab = AssetDatabase.LoadAssetAtPath("Assets/simple_low_poly_village_buildings/models/carrot2.prefab", typeof(GameObject));
        GameObject clone = gameController.InstantiatePrefab(prefab, Vector3.zero, Quaternion.identity) as GameObject;
        clone.transform.localScale = new Vector3(15, 15, 15);
        clone.transform.position = new Vector3(position.x, 0.25f, position.z);
    }
}

