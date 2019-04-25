using UnityEngine;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{
    public Image[] itemImages = new Image[numItemSlots];
    public Image[] quantityBackgrounds = new Image[numItemSlots];
    public Item[] items = new Item[numItemSlots];
    public Text[] quantities = new Text[numItemSlots];
    public Item.ItemType[] types = new Item.ItemType[numItemSlots];

    public const int numItemSlots = 6;

    public void AddItem(Item.ItemType type, int quantityToAdd = 1)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                if (items[i].type == type)
                {
                    items[i].quantity+= quantityToAdd;
                    quantities[i].text = items[i].quantity.ToString();
                    return;
                }
            }
            else
            {
                types[i] = type;
                items[i] = new Item(type, quantityToAdd);
                itemImages[i].sprite = Resources.Load<Sprite>(type.spriteDirectory);
                itemImages[i].enabled = true;
                quantityBackgrounds[i].enabled = true;
                quantities[i].text = items[i].quantity.ToString();
                return;
            }
        }
    }
    public void RemoveItem(Item.ItemType type, int quantityToRemove = 1)
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
}