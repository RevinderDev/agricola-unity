using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
