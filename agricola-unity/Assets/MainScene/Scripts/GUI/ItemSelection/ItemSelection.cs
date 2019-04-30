using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ItemSelection : MonoBehaviour
{
    public enum Mode
    {
        market,
        eating,
        animalEating
    }

    private Mode mode = Mode.market;
    private GameController gameController;
    private GameObject windowObject;
    private Text title;
    private Button buttonFinish;
    private Button buttonAccept;
    private GameObject transactionType;
    private Slider slider;
    public const int numItemSlots = 14;
    public GameObject[] items = new GameObject[numItemSlots];
    public ItemType[] itemTypes = new ItemType[numItemSlots];
    public Image[] itemImages = new Image[numItemSlots];
    public Text[] itemsNames = new Text[numItemSlots];
    public Text[] itemsPricesOrNutritions = new Text[numItemSlots];
    public Text[] itemsPricesOrNutritionsLabels = new Text[numItemSlots];
    public Image[] coinImages = new Image[numItemSlots];
    public InputField[] quantities = new InputField[numItemSlots];
    public Image coinImage;
    public int totalPriceOrNutritions = 0;
    public static readonly int itemWidth = 130;
    public string animalName { set; get; }

    public void SetMode(Mode mode)
    {
        if (this.mode != mode)
        {
            if (mode == Mode.eating)
            {
                title.text = "Eating";
                transactionType.SetActive(false);

            }
            else if(mode == Mode.market)
            {
                title.text = "Market";
                transactionType.SetActive(true);
            }
            else if(mode == Mode.animalEating)
            {
                title.text = "Animal food";
                transactionType.SetActive(false);
            }
        }
        this.mode = mode;
    }

    public void Initialize()
    {
        int i = 0;
        foreach (ItemType value in ItemType.list)
        {
            if (mode == Mode.market)
            {
                //Buy and can be bought or sell and can be sold
                if (slider.value == 0 && value.canBeBought || slider.value == 1 && value.canBeSold)
                {
                    items[i].SetActive(true);
                    itemTypes[i] = value;
                    itemsNames[i].text = value.name;
                    //Buy
                    if (slider.value == 0)
                        itemsPricesOrNutritions[i].text = "" + value.priceBuy;
                    //Sell
                    else
                        itemsPricesOrNutritions[i].text = "" + value.priceSell;
                    quantities[i].text = "0";
                    quantities[i].onValueChanged.AddListener(delegate { ActualizeTotal(); });
                    itemImages[i].sprite = Resources.Load<Sprite>(value.directory);
                    itemImages[i].enabled = true;
                    coinImages[i].sprite = Resources.Load<Sprite>("Sprites/coin");
                    coinImage.sprite = Resources.Load<Sprite>("Sprites/coin");
                    itemsPricesOrNutritionsLabels[i].text = "Price";
                    i++;
                }
            }
            else if(mode == Mode.eating)
            {
                if(value.nutritionValue != 0)
                {
                    Debug.Log(value.name);
                    items[i].SetActive(true);
                    itemTypes[i] = value;
                    itemsNames[i].text = value.name;
                    quantities[i].text = "0";
                    itemsPricesOrNutritions[i].text = "" + value.nutritionValue;
                    quantities[i].onValueChanged.AddListener(delegate { ActualizeTotal(); });
                    itemImages[i].sprite = Resources.Load<Sprite>(value.directory);
                    itemImages[i].enabled = true;
                    coinImages[i].sprite = Resources.Load<Sprite>("Sprites/eat");
                    coinImage.sprite = Resources.Load<Sprite>("Sprites/eat");
                    itemsPricesOrNutritionsLabels[i].text = "Nutritions";
                    i++;
                }
            }
            else if(mode == Mode.animalEating)
            {
                if (value.nutritionValue != 0)
                {
                    //TODO 
                    items[i].SetActive(true);
                    coinImages[i].sprite = Resources.Load<Sprite>("Sprites/animalFood");
                    coinImage.sprite = Resources.Load<Sprite>("Sprites/animalFood");
                    itemsPricesOrNutritionsLabels[i].text = "Ratio";
                    i++;
                }
            }
        }
        for (int j = i; j < numItemSlots; j++)
            items[j].SetActive(false);
    }

    public void SetMarket()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        windowObject = GameObject.Find("ItemSelectionObject");
        title = GameObject.Find("Title").GetComponent<Text>();
        buttonFinish = GameObject.Find("ButtonFinish").GetComponent<Button>();
        buttonAccept = GameObject.Find("ButtonAccept").GetComponent<Button>();
        transactionType = GameObject.Find("TransactionType");
        slider = GameObject.Find("Slider").GetComponent<Slider>();
        slider.onValueChanged.AddListener(delegate { Initialize(); });
        buttonFinish.onClick.AddListener(delegate { Hide(); });
        buttonAccept.onClick.AddListener(delegate 
        {
            if (mode == Mode.market)
                AcceptTransaction();
            else if (mode == Mode.eating || mode == Mode.animalEating)
                Eat();
        });
        coinImage = GameObject.Find("CoinImageTotal").GetComponent<Image>();

        items = GameObject.FindGameObjectsWithTag("Item");
        //itemTypes = new ItemType[numItemSlots];
        int i = 0;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ItemImage"))
        {
            itemImages[i] = obj.GetComponent<Image>();
            i++;
        }
        i = 0;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ItemName"))
        {
            itemsNames[i] = obj.GetComponent<Text>();
            i++;
        }
        i = 0;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ItemPriceOrNutritions"))
        {
            itemsPricesOrNutritions[i] = obj.GetComponent<Text>();
            i++;
        }
        i = 0;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ItemPriceOrNutritionsLabels"))
        {
            itemsPricesOrNutritionsLabels[i] = obj.GetComponent<Text>();
            i++;
        }
        i = 0;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("CoinImage"))
        {
            coinImages[i] = obj.GetComponent<Image>();
            i++;
        }
        i = 0;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Quantity"))
        {
            quantities[i] = obj.GetComponent<InputField>();
            i++;
        }

        Initialize();
    }

    public void Display()
    {
        windowObject.SetActive(true);
        ActionController.isActive = false;
    }

    public void Hide()
    {
        windowObject.SetActive(false);
        ActionController.isActive = true;
    }

    public void ActualizeTotal()
    {
        totalPriceOrNutritions = 0;
        for(int i = 0; i<numItemSlots; i++)
        {
            totalPriceOrNutritions += Int32.Parse(quantities[i].text) * Int32.Parse(itemsPricesOrNutritions[i].text);
        }
        GameObject.Find("TotalPrice").GetComponent<Text>().text = totalPriceOrNutritions.ToString();
    }
    public void AcceptTransaction()
    {
        //Buy
        if(slider.value == 0)
            if(gameController.GetMoney() >= totalPriceOrNutritions)
            {
                for (int i = 0; i < numItemSlots; i++)
                {
                    try
                    {
                        try
                        {
                            if (Int32.Parse(quantities[i].text) != 0)
                                gameController.inventory.AddItem(itemTypes[i], Int32.Parse(quantities[i].text));
                        }
                        catch (FormatException e) { }
                    }
                    catch (FormatException e)
                    {
                        gameController.DisplayInfo("Invalid quantity.");
                        break;
                    }
                }
                gameController.MoneyTransaction(-totalPriceOrNutritions);
                for (int i = 0; i < numItemSlots; i++)
                    quantities[i].text = "0";
            }
            else
            {
                gameController.DisplayInfo("You do not have enough money.");
            }
        //Sell
        else
            {
            int isValid = 0;
            for (int i = 0; i < numItemSlots; i++)
            {
                if (Int32.Parse(quantities[i].text) == 0)
                    isValid++;
                else
                    for (int j = 0; j<Inventory.numItemSlots; j++)
                    {
                        if (gameController.inventory.types[j] != null && itemTypes[i].Equals(gameController.inventory.types[j]))
                        {
                            try
                            {
                                if (gameController.inventory.quantities[j].text == null)
                                    ;
                                else if (Int32.Parse(quantities[i].text) > Int32.Parse(gameController.inventory.quantities[j].text))
                                    ;
                                else
                                    isValid++;
                            }
                            catch (FormatException e)
                            {
                                gameController.DisplayInfo("Invalid quantity.");
                                return;
                            }
                        }
                    }
            }
            if(isValid == numItemSlots)
            {
                for (int i = 0; i < numItemSlots; i++)
                {
                    if (Int32.Parse(quantities[i].text) != 0)
                        gameController.inventory.RemoveItem(itemTypes[i], Int32.Parse(quantities[i].text));
                }
                gameController.MoneyTransaction(+totalPriceOrNutritions);
                for (int i = 0; i < numItemSlots; i++)
                    quantities[i].text = "0";
            }
            else
            {
                gameController.DisplayInfo("You do not have enough items.");
            }
        }
    }

    private void Eat()
    {
        int isValid = 0;
        for (int i = 0; i < numItemSlots; i++)
        {
            if (Int32.Parse(quantities[i].text) == 0)
                isValid++;
            else
                for (int j = 0; j < Inventory.numItemSlots; j++)
                {
                    if (gameController.inventory.types[j] != null && itemTypes[i].Equals(gameController.inventory.types[j]))
                    {
                        try
                        {
                            if (gameController.inventory.quantities[j].text == null)
                                ;
                            else if (Int32.Parse(quantities[i].text) > Int32.Parse(gameController.inventory.quantities[j].text))
                                ;
                            else
                                isValid++;
                        }
                        catch (FormatException e)
                        {
                            gameController.DisplayInfo("Invalid quantity.");
                            return;
                        }
                    }
                }
        }
        if (isValid == numItemSlots)
        {
            for (int i = 0; i < numItemSlots; i++)
            {
                if (Int32.Parse(quantities[i].text) != 0)
                {
                    gameController.inventory.RemoveItem(itemTypes[i], Int32.Parse(quantities[i].text));
                    if(mode == Mode.animalEating)
                    {
                        if (animalName != null)
                        {
                            //TODO : Ratio ?
                            gameController.animalFarm.addAnimalFood(animalName, totalPriceOrNutritions);
                        }
                    }
                    else
                        gameController.player.ChangeHunger(itemTypes[i].nutritionValue * Int32.Parse(quantities[i].text));
                }
            }
            for (int i = 0; i < numItemSlots; i++)
                quantities[i].text = "0";
        }
        else
        {
            gameController.DisplayInfo("You do not have enough items.");
        }
    }
}

