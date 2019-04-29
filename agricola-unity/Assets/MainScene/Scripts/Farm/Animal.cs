using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal
{
    private readonly AnimalType animalType;
    private GameObject gameObject;
    private bool isHungry;
    public int daysInExistance { get; private set; }
    public int currentHungerLevel { get; }
    public int maxHungerLevel { get; }
    public int dayOfDeath { get; }

    public AnimalType getAnimalType()
    {
        return animalType;
    }



    public Animal(GameObject gameObject, AnimalType animalType)
    {
        isHungry = false;
        daysInExistance = 0;
        dayOfDeath = 10;
        this.gameObject = gameObject;
        this.animalType = animalType;
        this.currentHungerLevel = 50;
        this.maxHungerLevel = 50;
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