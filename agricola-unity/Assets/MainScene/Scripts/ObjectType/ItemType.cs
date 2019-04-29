using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemType : Type
{
    public readonly int priceSell;
    public readonly int priceBuy;
    public readonly bool canBeSold;
    public readonly bool canBeBought;
    public readonly int nutritionValue;
    public static IReadOnlyCollection<ItemType> list;
    //seeds
    public static readonly ItemType carrotSeeds = new ItemType("carrot seeds", "Sprites/seeds_carrot", 0, 1, true, true);
    public static readonly ItemType tomatoSeeds = new ItemType("tomato seeds", "Sprites/seeds_tomato", 0, 1, true, true);
    public static readonly ItemType pumpkinSeeds = new ItemType("pumpkin seeds", "Sprites/seeds_pumpkin", 0, 1, true, true);
    //plants
    public static readonly ItemType carrot = new ItemType("carrot", "Sprites/carrot", 2, 5, true, false, 1);
    public static readonly ItemType tomato = new ItemType("tomato", "Sprites/tomato", 3, 7, true, false, 2);
    public static readonly ItemType pumpkin = new ItemType("pumpkin", "Sprites/pumpkin", 5, 10, true, false, 5);
    //animals
    public static readonly ItemType cow = new ItemType("cow", "Sprites/cow", 10, 20, true, true);
    public static readonly ItemType michal = new ItemType("Michal", "Sprites/michal", 100, 200, true, true, 50);
    public static readonly ItemType milk = new ItemType("milk", "Sprites/milk", 4, 6, true, true, 5);

    public ItemType(string name, string spriteDirectory, int priceSell, int priceBuy, 
        bool canBeSold, bool canBeBought, int nutritionValue = 0) 
        : base(name, spriteDirectory)
    {
        this.priceSell = priceSell;
        this.priceBuy = priceBuy;
        this.canBeSold = canBeSold;
        this.canBeBought = canBeBought;
        this.nutritionValue = nutritionValue;
    }

    public static void Initialize()
    {
        list = new List<ItemType>
            {
                carrotSeeds,
                tomatoSeeds,
                pumpkinSeeds,
                carrot,
                tomato,
                pumpkin,
                cow,
                michal,
                milk

            }.AsReadOnly();
    }
}
