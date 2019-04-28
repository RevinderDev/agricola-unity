using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionType : Type
{
    public readonly int length;

    /* New action should be added in PerformAction (GameController), OnMouseDown (ActionController
    * and eventually in Farmland or other classes created (custom method)
    **/
    public static readonly ActionType walk = new ActionType("walk", null, 0);
    public static readonly ActionType plant = new ActionType("plant", "Sprites/planting", 500);
    public static readonly ActionType collectPlant = new ActionType("collect plant", "Sprites/plants", 1000);
    public static readonly ActionType buyCow = new ActionType("buy cow", "Sprites/cowIcon2", 5000); //TODO: ta liczba nie wiem jaka ma byc..
    public static readonly ActionType market = new ActionType("market", "Sprites/market", 12000);
    public static readonly ActionType gatherMilk = new ActionType("get milk", "Sprites/milkIcon", 500);

    public ActionType(string name, string spriteDirectory, int length) : base(name, spriteDirectory)
    {
        this.length = length;
    }
}
