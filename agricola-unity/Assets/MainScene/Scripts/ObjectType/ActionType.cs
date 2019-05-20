using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionType : Type
{
    public readonly int length;
    public readonly IReadOnlyCollection<ActionEvent> associatedEvents;

    /* New action should be added in PerformAction (GameController), OnMouseDown (ActionController
    * and eventually in Farmland or other classes created (custom method)
    **/
    public static readonly ActionType walk = new ActionType("walk", null, 0, 
        new List<ActionEvent>());
    public static readonly ActionType plant = new ActionType("plant", "Sprites/planting", 6000, 
        new List<ActionEvent>() { ActionEvent.pumpkinFound, ActionEvent.tomatoFound, ActionEvent.carrotFound });
    public static readonly ActionType collectPlant = new ActionType("collect plant", "Sprites/plants", 6000, 
        new List<ActionEvent>());
    public static readonly ActionType placeCow = new ActionType("place cow", "Sprites/cow", 6000, 
        new List<ActionEvent>() { ActionEvent.cowKick });
    public static readonly ActionType placeChicken = new ActionType("add chicken", "Sprites/chicken", 6000, 
        new List<ActionEvent>() { ActionEvent.chickenPeck });
    public static readonly ActionType market = new ActionType("market", "Sprites/market", 12000, 
        new List<ActionEvent>());
    public static readonly ActionType gatherMilk = new ActionType("get milk", "Sprites/milk", 3000, 
        new List<ActionEvent>() { ActionEvent.cowKick });
    public static readonly ActionType gatherEgg = new ActionType("get egg", "Sprites/egg", 3000, 
        new List<ActionEvent>() { ActionEvent.eggHatch });
    public static readonly ActionType feedCow = new ActionType("feed cow", "Sprites/animalFood", 8000, 
        new List<ActionEvent>() { ActionEvent.cowKick });
    public static readonly ActionType feedChicken = new ActionType("feed chicken", "Sprites/animalFood", 8000, 
        new List<ActionEvent>() { ActionEvent.chickenPeck });
    public static readonly ActionType eat = new ActionType("eat", "Sprites/eat", 3000, 
        new List<ActionEvent>());
    public static readonly ActionType checkCowStatus = new ActionType("-", null, 0, 
        new List<ActionEvent>());
    public static readonly ActionType checkChickenStatus = new ActionType("-", null, 0, 
        new List<ActionEvent>());

    public ActionType(string name, string spriteDirectory, int length, List<ActionEvent> events) : base(name, spriteDirectory)
    {
        this.length = length;
        this.associatedEvents = events.AsReadOnly();
    }
}
