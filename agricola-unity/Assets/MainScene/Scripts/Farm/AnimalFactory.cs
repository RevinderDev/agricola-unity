using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalFactory
{

    private static AnimalFactory animalFactoryInstance = new AnimalFactory();
    private readonly string cowPrefab = "Assets/VertexColorFarmAnimals/Prefabs/CowBlW.prefab";

    private AnimalFactory()
    {

    }


    public static AnimalFactory getInstance()
    {
        return animalFactoryInstance;
    }


    public Animal buildCow(GameObject gameObject)
    {
        int milkDaysToCollect = 5;
        int milkDaysToSpoil = 3;
        string productName = "Milk";
        string milkPrefab = ""; // TODO: add milk prefab
        int milkNutritionValue = 10;
        GeneratedFoodProduct milk = new GeneratedFoodProduct(milkDaysToCollect, milkDaysToSpoil, milkPrefab, productName, milkNutritionValue);

 
        int dayOfCowDeath = 15;
        int cowDailyHungerLoss = 10;
        int cowMaxHungerLevel = 100;
        AnimalType cowType = new AnimalType("Cow", cowPrefab, dayOfCowDeath, milk, cowDailyHungerLoss, cowMaxHungerLevel);

        return new Animal(gameObject, cowType);
    }

    public string getCowPrefab()
    {
        return cowPrefab;
    }


    public Animal buildSheep(GameObject gameObject)
    {
        // TODO: implement build Sheep
        throw new System.NotImplementedException();
    }

    public Animal buildChicken(GameObject gameObject)
    {
        // TODO: implement build Chicken
        throw new System.NotImplementedException();
    }
}
