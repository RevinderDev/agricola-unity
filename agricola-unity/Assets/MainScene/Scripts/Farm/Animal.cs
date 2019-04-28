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