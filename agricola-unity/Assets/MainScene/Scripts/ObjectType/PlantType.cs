using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantType : Type
{
    public readonly float growthPerDay;
    public readonly float startPosision;
    public readonly int daysToCollect;
    public readonly int daysToBeSpoiled;
    public readonly ItemType itemType;

    public readonly IReadOnlyCollection<ActionEvent> associatedEventsPlant;
    public readonly IReadOnlyCollection<ActionEvent> associatedEventsCollect;

    // Name of PlantType must exist in project tags!
    public static readonly PlantType carrot = new PlantType("Carrot", 0.1f, 0.1f, 2, 4,"Assets/simple_low_poly_village_buildings/models/carrot2.prefab", 
        ItemType.carrot, new List<ActionEvent>() { ActionEvent.carrotFound }, new List<ActionEvent>() { ActionEvent.carrotSeedsFound });
    public static readonly PlantType tomato = new PlantType("Tomato", 0.08f, 0.3f, 4, 4, "Assets/simple_low_poly_village_buildings/models/tomato2.prefab", 
        ItemType.tomato, new List<ActionEvent>() { ActionEvent.tomatoFound }, new List<ActionEvent>() { ActionEvent.tomatoSeedsFound });
    public static readonly PlantType pumpkin = new PlantType("Pumpkin", 0.05f, 0.5f, 6, 10, "Assets/FoodIcons/pumpkin2.prefab", 
        ItemType.pumpkin, new List<ActionEvent>() { ActionEvent.pumpkinFound }, new List<ActionEvent>() { ActionEvent.pumpkinSeedsFound});

    public PlantType(string name, float growthPerDay, float startPosision, int daysToCollect, int daysToBeSpoiled, string directory, 
        ItemType itemType, List<ActionEvent> associatedEventsPlant, List<ActionEvent> associatedEventsCollect) : base(name, directory)
    {
        this.growthPerDay = growthPerDay;
        this.startPosision = startPosision;
        this.daysToBeSpoiled = daysToBeSpoiled;
        this.daysToCollect = daysToCollect;
        this.itemType = itemType;
        this.associatedEventsCollect = associatedEventsCollect.AsReadOnly();
        this.associatedEventsPlant = associatedEventsPlant.AsReadOnly();
    }
}