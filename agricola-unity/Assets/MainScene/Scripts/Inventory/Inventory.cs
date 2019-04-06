using UnityEngine;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{
    public Image[] itemImages = new Image[numItemSlots];
    public Item[] items = new Item[numItemSlots];
    public Text[] quantities = new Text[numItemSlots];

    public const int numItemSlots = 6;

    public void AddItem(string spriteDirectory, int quantityToAdd = 1)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                if (items[i].spriteDirectory == spriteDirectory)
                {
                    items[i].quantity+= quantityToAdd;
                    quantities[i].text = items[i].quantity.ToString();
                    return;
                }
            }
            else
            {
                items[i] = new Item(spriteDirectory, 1);
                itemImages[i].sprite = Resources.Load<Sprite>(spriteDirectory);
                itemImages[i].enabled = true;
                quantities[i].text = items[i].quantity.ToString();
                return;
            }
        }
    }
    public void RemoveItem(string spriteDirectory, int quantityToRemove = 1)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].spriteDirectory == spriteDirectory)
            {
                if (items[i].quantity > quantityToRemove)
                {
                    items[i].quantity -= quantityToRemove;
                    quantities[i].text = items[i].quantity.ToString();
                    return;
                }
                else
                {
                    items[i] = null;
                    itemImages[i].sprite = null;
                    itemImages[i].enabled = false;
                    quantities[i].text = "";
                    return;
                }
            }
        }
    }
}