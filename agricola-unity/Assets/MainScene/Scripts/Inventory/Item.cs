using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public string spriteDirectory;
    public int quantity;

    public Item(string spriteDirectory, int quantity)
    {
        this.spriteDirectory = spriteDirectory;
        this.quantity = quantity;
    }
}
