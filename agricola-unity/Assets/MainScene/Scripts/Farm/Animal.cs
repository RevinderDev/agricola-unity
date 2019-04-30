using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal
{
    private readonly AnimalType animalType;
    private GameObject gameObject;
    public int daysInExistance { get; private set; }
    public int currentHungerLevel { get; private set; }
    public int maxHungerLevel { get; }
    public int dayOfDeath { get; }
    public bool isDead { get; set; }

    public AnimalType getAnimalType()
    {
        return animalType;
    }



    public Animal(GameObject gameObject, AnimalType animalType)
    {
        daysInExistance = 0;
        dayOfDeath = 10;
        this.gameObject = gameObject;
        this.animalType = animalType;
        this.currentHungerLevel = 50;
        this.maxHungerLevel = 50;
        this.isDead = false;
    }


    public bool isHungry()
    {
        return currentHungerLevel < maxHungerLevel;
    }
    

    public void eat(ref int totalFood)
    {
        totalFood -= 1;
        currentHungerLevel += 10;
    }

    public void AddDayInExistance()
    {
        currentHungerLevel -= 10;
        daysInExistance++;
        if (daysInExistance >= dayOfDeath || currentHungerLevel <= 0)
            isDead = true;
    }
}