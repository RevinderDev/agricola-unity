using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public ItemType type;
    public int quantity;
    
    public class ItemType
    {
        
        public string name;
        public string spriteDirectory;
        public int price;
        public static IReadOnlyCollection<ItemType> list;
        //seeds
        public static readonly ItemType carrotSeeds = new ItemType("carrot seeds", "Sprites/seeds_carrot", 1);
        public static readonly ItemType tomatoSeeds = new ItemType("tomato seeds", "Sprites/seeds_tomato", 1);
        //plants
        public static readonly ItemType carrot = new ItemType("carrot", "Sprites/carrot", 2);
        public static readonly ItemType tomato = new ItemType("tomato", "Sprites/tomato", 3);
        //animals
        public static readonly ItemType cow = new ItemType("cow", "Sprites/cow", 10);
        public static readonly ItemType michal = new ItemType("Michal", "Sprites/michal", 100);
        
       

        public ItemType(string name, string spriteDirectory, int price)
        {
            this.name = name;
            this.spriteDirectory = spriteDirectory;
            this.price = price;
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
                michal

            }.AsReadOnly();
        }
    }

    public Item(ItemType type, int quantity)
    {
        this.type = type;
        this.quantity = quantity;
    }
}
