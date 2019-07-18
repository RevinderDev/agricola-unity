using UnityEngine;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{
    public Image[] itemImages = new Image[numItemSlots];
    public Image[] quantityBackgrounds = new Image[numItemSlots];
    public Item[] items = new Item[numItemSlots];
    public Text[] quantities = new Text[numItemSlots];
    public ItemType[] types = new ItemType[numItemSlots];

    public const int numItemSlots = 14;

    public void AddItem(ItemType type, int quantityToAdd = 1)
    {
        int j = -1;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                if (items[i].type == type)
                {
                    items[i].quantity += quantityToAdd;
                    quantities[i].text = items[i].quantity.ToString();
                    return;
                }
            }
            else if(j == -1)
                j = i;
        }
        types[j] = type;
        items[j] = new Item(type, quantityToAdd);
        itemImages[j].sprite = Resources.Load<Sprite>(type.directory);
        itemImages[j].enabled = true;
        quantityBackgrounds[j].enabled = true;
        quantities[j].text = items[j].quantity.ToString();
        return; 
    }
    public void RemoveItem(ItemType type, int quantityToRemove = 1)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null && items[i].type == type)
            {
                if (items[i].quantity > quantityToRemove)
                {
                    items[i].quantity -= quantityToRemove;
                    quantities[i].text = items[i].quantity.ToString();
                    return;
                }
                else
                {
                    types[i] = null;
                    items[i] = null;
                    itemImages[i].sprite = null;
                    itemImages[i].enabled = false;
                    quantityBackgrounds[i].enabled = false;
                    quantities[i].text = "";
                    return;
                }
            }
        }
    }
    public void RemoveItem(string name, int quantityToRemove = 1)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null && items[i].type.name == name)
            {
                if (items[i].quantity > quantityToRemove)
                {
                    items[i].quantity -= quantityToRemove;
                    quantities[i].text = items[i].quantity.ToString();
                    return;
                }
                else
                {
                    types[i] = null;
                    items[i] = null;
                    itemImages[i].sprite = null;
                    itemImages[i].enabled = false;
                    quantityBackgrounds[i].enabled = false;
                    quantities[i].text = "";
                    return;
                }
            }
        }
    }

    public void Clear()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                items[i].quantity = 0;
                types[i] = null;
                items[i] = null;
                itemImages[i].sprite = null;
                itemImages[i].enabled = false;
                quantityBackgrounds[i].enabled = false;
                quantities[i].text = "";
            }
        }
    }

    public bool DoesContain(ItemType type)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null && items[i].type == type)
            {
                return true;
            }
        }
        return false;
    }
    public bool DoesContain(string name)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null && items[i].type.name == name)
            {
                return true;
            }
        }
        return false;
    }


    public int GetInventoryValue()
    {
        int value = 0;
        for(int i = 0; i<items.Length; i++)
        {
            if(items[i] != null)
            {
                int itemQuantity = items[i].quantity;
                int itemPrice = types[i].priceSell;
                value += itemPrice * itemQuantity;
            }
        }

        return value;
    }
}