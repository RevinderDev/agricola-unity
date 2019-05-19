using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEvent
{
    public readonly string name;
    public readonly float probability;
    public readonly string description;
    public readonly int healthChange;
    public readonly int hungerChange;
    public readonly Dictionary<ItemType, int> itemsChange;
    
    // New animal
    public static readonly ActionEvent eggHatch = new ActionEvent("Egg hatch", 0.1f, "Egg htched! You got chicken instead of one egg.",
        0, 0, new Dictionary<ItemType, int> {{ ItemType.chicken, 1}, {ItemType.egg, -1}});

    // Health loose (animals)
    public static readonly ActionEvent cowKick = new ActionEvent("Cow kick", 0.1f, "Cow kicked you. You lost 5 hp.",
        -5, 0, new Dictionary<ItemType, int> {});
    public static readonly ActionEvent chickenPeack = new ActionEvent("Chicken peck", 0.2f, "Chicken pecked you. You lost 1 hp.",
        -1, 0, new Dictionary<ItemType, int> {});

    // Additionary plants
    public static readonly ActionEvent carrotFound = new ActionEvent("Carrot found", 0.2f, "Poking in the ground you found a carrot.",
        0, 0, new Dictionary<ItemType, int> { { ItemType.carrot, 1 } });
    public static readonly ActionEvent tomatoFound = new ActionEvent("Tomato found", 0.05f, "Poking in the ground you found a tomato.",
        0, 0, new Dictionary<ItemType, int> { { ItemType.tomato, 1 } });
    public static readonly ActionEvent pumpkinFound = new ActionEvent("Pumpkin found", 0.01f, "Poking in the ground you found a pumpkin.",
        0, 0, new Dictionary<ItemType, int> { { ItemType.pumpkin, 1 } });

    // Additionary seeds
    public static readonly ActionEvent carrotSeedsFound = new ActionEvent("Carrot seeds found", 0.2f, "Collecting carrot you found some carrot seeds.",
        0, 0, new Dictionary<ItemType, int> { { ItemType.carrotSeeds, 1 } });
    public static readonly ActionEvent tomatoSeedsFound = new ActionEvent("Tomato seeds found", 0.05f, "Collecting tomato you found some tomato seeds.",
        0, 0, new Dictionary<ItemType, int> { { ItemType.tomatoSeeds, 1 } });
    public static readonly ActionEvent pumpkinSeedsFound = new ActionEvent("Pumpkin seeds found", 0.01f, "Collecting pumpkin you found some pumpkin seeds.",
        0, 0, new Dictionary<ItemType, int> { { ItemType.pumpkinSeeds, 1 } });

    public ActionEvent(string name, float probability, string description, int healthChange, int hungerChange, 
        Dictionary<ItemType, int> itemsChange)
        {
            this.name = name;
            this.probability = probability;
            this.description = description;
            this.healthChange = healthChange;
            this.hungerChange = hungerChange;
            this.itemsChange = itemsChange;
    }
}
