using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemType : Type
{
    public readonly int priceSell;
    public readonly int priceBuy;
    public readonly bool canBeSold;
    public readonly bool canBeBought;
    public static IReadOnlyCollection<ItemType> list;
    //seeds
    public static readonly ItemType carrotSeeds = new ItemType("carrot seeds", "Sprites/seeds_carrot", 0, 1, true, true);
    public static readonly ItemType tomatoSeeds = new ItemType("tomato seeds", "Sprites/seeds_tomato", 0, 1, true, true);
    //plants
    public static readonly ItemType carrot = new ItemType("carrot", "Sprites/carrot", 2, 5, true, false);
    public static readonly ItemType tomato = new ItemType("tomato", "Sprites/tomato", 3, 7, true, false);
    //animals
    public static readonly ItemType cow = new ItemType("cow", "Sprites/cow", 10, 20, true, true);
    public static readonly ItemType michal = new ItemType("Michal", "Sprites/michal", 100, 200, true, true);
    public static readonly ItemType milk = new ItemType("milk", "Sprites/milkIcon", 4, 6, true, true);

    public ItemType(string name, string spriteDirectory, int priceSell, int priceBuy, bool canBeSold, bool canBeBought) 
        : base(name, spriteDirectory)
    {
        this.priceSell = priceSell;
        this.priceBuy = priceBuy;
        this.canBeSold = canBeSold;
        this.canBeBought = canBeBought;
    }

    public static void Initialize()
    {
        list = new List<ItemType>
            {
                carrotSeeds,
                tomatoSeeds,
                carrot,
                tomato,
                cow,
                michal,
                milk

            }.AsReadOnly();
    }
}
