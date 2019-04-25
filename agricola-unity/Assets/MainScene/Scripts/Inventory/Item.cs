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
        public static readonly ItemType carrot = new ItemType("carrot", "Sprites/carrot", 1);
        public static readonly ItemType cow = new ItemType("cow", "Sprites/cow", 10);

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
                carrot,
                cow
            }.AsReadOnly();
        }
    }

    public Item(ItemType type, int quantity)
    {
        this.type = type;
        this.quantity = quantity;
    }
}
