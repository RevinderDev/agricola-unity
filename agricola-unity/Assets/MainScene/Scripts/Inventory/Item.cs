using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public ItemType type;
    public int quantity;

    public Item(ItemType type, int quantity)
    {
        this.type = type;
        this.quantity = quantity;
    }
}
