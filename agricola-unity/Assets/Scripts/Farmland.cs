using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmland
{
    private List<Vector3> positions;
    private List<GameObject> areaCubes;
    private float interspace;
    private Vector3 scale;
    private int count;
    private Vector3 position;

    public Farmland()
    {
        scale = new Vector3(3, 0.25f, 3);
        position = new Vector3(-5, 0.5f, -10);
        interspace = 3.5f;
        count = 6;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                createArea(i, j);
            }
        }
    }

    private void createArea(int x, int z)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = scale;
        cube.transform.position = new Vector3(position.x + x * interspace, position.y, position.z + z * interspace);
        cube.GetComponent<Renderer>().material = Resources.Load("terrain", typeof(Material)) as Material;
        cube.AddComponent<ActionController>();
    }
}

