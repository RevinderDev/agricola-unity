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
    public readonly int ratioValue;
    public static IReadOnlyCollection<ItemType> list;
    //seeds
    public static readonly ItemType carrotSeeds = new ItemType("carrot seeds", "Sprites/seeds_carrot", 0, 11, false, true, 0, 1);
    public static readonly ItemType tomatoSeeds = new ItemType("tomato seeds", "Sprites/seeds_tomato", 0, 30, false, true, 0, 1);
    public static readonly ItemType pumpkinSeeds = new ItemType("pumpkin seeds", "Sprites/seeds_pumpkin", 0, 45, false, true, 0, 1);
    //plants
    public static readonly ItemType carrot = new ItemType("carrot", "Sprites/carrot", 3, 5, true, false, 10, 2);
    public static readonly ItemType tomato = new ItemType("tomato", "Sprites/tomato", 6, 7, true, false, 15, 4);
    public static readonly ItemType pumpkin = new ItemType("pumpkin", "Sprites/pumpkin", 5, 10, true, false, 25, 10);
    //animals
    public static readonly ItemType cow = new ItemType("cow", "Sprites/cow", 20, 30, true, true);
    public static readonly ItemType chicken = new ItemType("chicken", "Sprites/chicken", priceSell: 10, priceBuy: 12, canBeSold: true, canBeBought: true);
    public static readonly ItemType michal = new ItemType("Michal", "Sprites/michal", 100, 200, false, false, 50);
    public static readonly ItemType milk = new ItemType("milk", "Sprites/milk", 4, 6, true, false, 5);
    public static readonly ItemType egg = new ItemType("egg", "Sprites/egg", priceSell: 3, priceBuy: 5, canBeSold: true, canBeBought: false, nutritionValue:5);

    public ItemType(string name, string spriteDirectory, int priceSell, int priceBuy, 
        bool canBeSold, bool canBeBought, int nutritionValue = 0, int ratioValue = 0) 
        : base(name, spriteDirectory)
    {
        this.priceSell = priceSell;
        this.priceBuy = priceBuy;
        this.canBeSold = canBeSold;
        this.canBeBought = canBeBought;
        this.nutritionValue = nutritionValue;
        this.ratioValue = ratioValue;
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
                milk,
                chicken,
                egg

            }.AsReadOnly();
    }
}
