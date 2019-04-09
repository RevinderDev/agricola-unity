using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal
{
    private readonly AnimalType animalType;
    private GameObject gameObject;
    private bool isHungry;
    private int daysInExistance { get; set; }
    private int currentHungerLevel;

    public AnimalType getAnimalType()
    {
        return animalType;
    }



    public Animal(GameObject gameObject, AnimalType animalType)
    {
        isHungry = false;
        daysInExistance = 0;
        this.gameObject = gameObject;
        this.animalType = animalType;
        this.currentHungerLevel = 0;
    }
    

    public bool eatHay()
    {
        /*TODO: eat hay, increase hungry level etc.*/
        return false;
    }

    public void AddDayInExistance()
    {
        daysInExistance++;
    }

}


public class AnimalType
{
    public readonly string name;
    public readonly string prefabDirectory;
    public readonly GeneratedFoodProduct generatedResource;
    public readonly int dailyHungerLoss;
    public readonly int dayOfDeath;
    public readonly int maxHungerLevel;

    public AnimalType(string name, string prefabDirectory, int dayOfDeath, GeneratedFoodProduct generatedResource, int dailyHungerLoss, int maxHungerLevel)
    {
        this.name = name;
        this.prefabDirectory = prefabDirectory;
        this.dayOfDeath = dayOfDeath;
        this.generatedResource = generatedResource;
        this.dailyHungerLoss = dailyHungerLoss;
        this.maxHungerLevel = maxHungerLevel;
    }
}


public class GeneratedFoodProduct
{
    public readonly int daysToCollect;
    public readonly int daysToBeSpoiled;
    public readonly string prefabDirectory;
    public readonly string productName;
    public readonly int nutritionValue;

    public GeneratedFoodProduct(int daysToCollect, int daysToBeSpoiled, string prefabDirectory, string productName, int nutritionValue)
    {
        this.daysToBeSpoiled = daysToBeSpoiled;
        this.daysToCollect = daysToCollect;
        this.prefabDirectory = prefabDirectory;
        this.productName = productName;
        this.nutritionValue = nutritionValue;
    }
}