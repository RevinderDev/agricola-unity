using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalFactory
{

    private static AnimalFactory animalFactoryInstance = new AnimalFactory();
    private readonly string cowPrefab = "Assets/VertexColorFarmAnimals/Prefabs/CowBlW.prefab";
    private readonly string chickenPrefab = "Assets/VertexColorFarmAnimals/Prefabs/ChickenBrown.prefab";

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
        int milkDaysToSpoil = 2;
        string productName = "Milk";
        string milkPrefab = "Assets/01_3D/ETC/3D Bakery Object/Prefab/Milk.prefab";
        int milkNutritionValue = 10;
        GeneratedFoodProduct milk = new GeneratedFoodProduct(milkDaysToCollect, milkDaysToSpoil, milkPrefab, productName, milkNutritionValue);

 
        int dayOfCowDeath = 15;
        int cowDailyHungerLoss = 10;
        int cowMaxHungerLevel = 80;
        AnimalType cowType = new AnimalType("Cow", cowPrefab, dayOfCowDeath, milk, cowDailyHungerLoss, cowMaxHungerLevel);

        return new Animal(gameObject, cowType);
    }

    public string getChickenPrefab()
    {
        return chickenPrefab;
    }

    public string getCowPrefab()
    {
        return cowPrefab;
    }

    public Animal buildChicken(GameObject gameObject)
    {

        string eggPrefab = "Assets/FoodIcons/egg.FBX";

        GeneratedFoodProduct egg = new GeneratedFoodProduct(daysToCollect: 3,
            daysToBeSpoiled: 1, 
            prefabDirectory: eggPrefab, 
            productName: "egg",
            nutritionValue: 8);


        AnimalType chickenType = new AnimalType(name: "Chicken",
            prefabDirectory: chickenPrefab,
            dayOfDeath: 10,
            generatedResource: egg,
            dailyHungerLoss: 10,
            maxHungerLevel: 50);

        return new Animal(gameObject, chickenType);
    }
}
