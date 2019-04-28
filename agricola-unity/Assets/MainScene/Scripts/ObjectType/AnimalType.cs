using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalType : Type
{
    public readonly GeneratedFoodProduct generatedResource;
    public readonly int dailyHungerLoss;
    public readonly int dayOfDeath;
    public readonly int maxHungerLevel;

    public AnimalType(string name, string prefabDirectory, int dayOfDeath, GeneratedFoodProduct generatedResource, int dailyHungerLoss, int maxHungerLevel):
        base(name, prefabDirectory)
    {
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