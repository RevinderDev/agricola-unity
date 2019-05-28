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

    public class Tag
    {
        public static readonly string item = "Item";
        public static readonly string itemImage = "ItemImage";
        public static readonly string itemName = "ItemName";
        public static readonly string itemValue = "ItemValue";
        public static readonly string itemValueLabel = "ItemValueLabel";
        public static readonly string itemQuantity = "ItemQuantity";
        public static readonly string itemValueImage = "ItemValueImage";
    }

    public Mode mode = Mode.market;
    public bool isVisible;
    private GameController gameController;
    private GameObject windowObject;
    private Text title;
    private Button buttonFinish;
    private Button buttonAccept;
    private GameObject transactionType;
    private Text totalPriceText; 
    private Slider slider;
    public const int numItemSlots = 14;
    public Dictionary<string, GameObject[]> gameObjects = new Dictionary<string, GameObject[]>();
    //TODO create public arrays and inicialize in unity
    public GameObject[] images;
    public GameObject[] items;
    public GameObject[] names;
    public GameObject[] itemValues;
    public GameObject[] itemValueLabels;
    public GameObject[] itemValueImages;
    public GameObject[] itemQuantities;
    public ItemType[] itemTypes = new ItemType[numItemSlots];
    public Image totalValueImage;
    public int totalValue = 0;
    public static readonly int itemWidth = 130;
    public string animalName { set; get; }


    public void SetMode(Mode mode)
    {
        if (this.mode != mode)
        {
            
            if(mode == Mode.market)
            {
                title.text = "Market";
                transactionType.SetActive(true);
            }
            else if (mode == Mode.eating)
            {
                title.text = "Eating";
                transactionType.SetActive(false);
            }
            else if(mode == Mode.animalEating)
            {
                title.text = "Animal food";
                transactionType.SetActive(false);
            }
        }
        this.mode = mode;
    }

    public void SetCommon(int i, ItemType value)
    {
        gameObjects[Tag.item][i].SetActive(true);
        itemTypes[i] = value;
        gameObjects[Tag.itemName][i].GetComponent<Text>().text = value.name;
        gameObjects[Tag.itemQuantity][i].GetComponent<InputField>().text = "0";
        gameObjects[Tag.itemQuantity][i].GetComponent<InputField>().onValueChanged.AddListener(delegate { ActualizeTotal(); });
        gameObjects[Tag.itemImage][i].GetComponent<Image>().sprite = Resources.Load<Sprite>(value.directory);
        gameObjects[Tag.itemImage][i].GetComponent<Image>().enabled = true;
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
                    SetCommon(i, value);
                    //Buy
                    if (slider.value == 0)
                        gameObjects[Tag.itemValue][i].GetComponent<Text>().text = "" + value.priceBuy;
                    //Sell
                    else
                        gameObjects[Tag.itemValue][i].GetComponent<Text>().text = "" + value.priceSell;
                    gameObjects[Tag.itemValueImage][i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/coin");
                    totalValueImage.sprite = Resources.Load<Sprite>("Sprites/coin");
                    gameObjects[Tag.itemValueLabel][i].GetComponent<Text>().text = "Price";
                    i++;
                }
            }
            else if(mode == Mode.eating)
            {
                if(value.nutritionValue != 0)
                {
                    SetCommon(i, value);
                    gameObjects[Tag.itemValue][i].GetComponent<Text>().text = "" + value.nutritionValue;
                    gameObjects[Tag.itemValueImage][i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/eat");
                    totalValueImage.sprite = Resources.Load<Sprite>("Sprites/eat");
                    gameObjects[Tag.itemValueLabel][i].GetComponent<Text>().text = "Nutritions";
                    i++;
                }
            }
            else if(mode == Mode.animalEating)
            {
                if (value.ratioValue != 0)
                {
                    SetCommon(i, value);
                    gameObjects[Tag.itemValue][i].GetComponent<Text>().text = "" + value.ratioValue;
                    gameObjects[Tag.itemValueImage][i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/animalFood");
                    totalValueImage.sprite = Resources.Load<Sprite>("Sprites/animalFood");
                    gameObjects[Tag.itemValueLabel][i].GetComponent<Text>().text = "Ratio";
                    i++;
                }
            }
        }

        for (int j = i; j < numItemSlots; j++)
            gameObjects[Tag.item][j].SetActive(false);
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
        buttonFinish.onClick.AddListener(delegate {
            Hide();
            totalPriceText.text = "0";
            if (mode == Mode.market)
                SetNewPlayerQuestion();
        });
        buttonAccept.onClick.AddListener(delegate 
        {
            if (mode == Mode.market)
                AcceptTransaction();
            else if (mode == Mode.eating || mode == Mode.animalEating)
                Eat();
        });
        totalValueImage = GameObject.Find("CoinImageTotal").GetComponent<Image>();
        totalPriceText = GameObject.Find("TotalPrice").GetComponent<Text>();



        //TODO Remove dynamic adding for all arrays (like below)


        gameObjects.Add(Tag.item, items);
        gameObjects.Add(Tag.itemImage, images);
        gameObjects.Add(Tag.itemName, names);
        gameObjects.Add(Tag.itemValue, itemValues);
        gameObjects.Add(Tag.itemValueLabel, itemValueLabels);
        gameObjects.Add(Tag.itemValueImage, itemValueImages);
        gameObjects.Add(Tag.itemQuantity, itemQuantities);
        //gameObjects.Add(Tag.item, GameObject.FindGameObjectsWithTag(Tag.item));
        //gameObjects.Add(Tag.itemName, GameObject.FindGameObjectsWithTag(Tag.itemName));
        //gameObjects.Add(Tag.itemValue, GameObject.FindGameObjectsWithTag(Tag.itemValue));
        //gameObjects.Add(Tag.itemValueLabel, GameObject.FindGameObjectsWithTag(Tag.itemValueLabel));
        //gameObjects.Add(Tag.itemValueImage, GameObject.FindGameObjectsWithTag(Tag.itemValueImage));
        //gameObjects.Add(Tag.itemQuantity, GameObject.FindGameObjectsWithTag(Tag.itemQuantity)); //TUTAJ DODAC INPUT
      
        Initialize();
    }

    int CompareObNames(GameObject x, GameObject y)
    {
        return x.name.CompareTo(y.name);
    }

    public void SetNewPlayerQuestion()
    {
        gameController.questionWindow.DisplayQuestion("You can have new family member, but it will cost " 
            + gameController.newPlayerCost +" gold. Do you accept?", "Family member");
    }

    public void Display()
    {
        gameController.PerformMouseExit();
        isVisible = true;
        windowObject.SetActive(true);
        ActionController.isActive = false;
    }

    public void Hide()
    {
        isVisible = false;
        windowObject.SetActive(false);
        ActionController.isActive = true;
    }

    public void ActualizeTotal()
    {
        totalValue = 0;
        for(int i = 0; i<numItemSlots; i++)
        {
            try
            {
                totalValue += Int32.Parse(gameObjects[Tag.itemQuantity][i].GetComponent<InputField>().text) * 
                    Int32.Parse(gameObjects[Tag.itemValue][i].GetComponent<Text>().text);
            }
            catch (FormatException e)
            {
                gameController.DisplayInfo("Invalid quantity.");
                break;
            }
        }
        totalPriceText.text = totalValue.ToString();
    }

    public void AcceptTransaction()
    {
        int total = totalValue;
        try
        { 
            //Buy
            if (slider.value == 0)
                if (gameController.GetMoney() >= totalValue)
                {
                    for (int i = 0; i < numItemSlots; i++)
                    {
                        InputField quantity = gameObjects[Tag.itemQuantity][i].GetComponent<InputField>();
                        if (Int32.Parse(quantity.text) != 0)
                            gameController.inventory.AddItem(itemTypes[i], Int32.Parse(quantity.text));
                        quantity.text = "0";
                    }
                    gameController.MoneyTransaction(-total);
                }
                else
                    gameController.DisplayInfo("You do not have enough money.");
            //Sell
            else
            {
                int isValid = 0;
                for (int i = 0; i < numItemSlots; i++)
                {
                    InputField quantity = gameObjects[Tag.itemQuantity][i].GetComponent<InputField>();
                    if (Int32.Parse(quantity.text) == 0)
                        isValid++;
                    else
                        for (int j = 0; j < Inventory.numItemSlots; j++)
                            if (gameController.inventory.types[j] != null && itemTypes[i].Equals(gameController.inventory.types[j]))
                                if (Int32.Parse(quantity.text) <= Int32.Parse(gameController.inventory.quantities[j].text))
                                    isValid++;
                }
                if (isValid == numItemSlots)
                {
                    for (int i = 0; i < numItemSlots; i++)
                    {
                        InputField quantity = gameObjects[Tag.itemQuantity][i].GetComponent<InputField>();
                        if (Int32.Parse(quantity.text) != 0)
                            gameController.inventory.RemoveItem(itemTypes[i], Int32.Parse(quantity.text));
                        quantity.text = "0";
                    }
                    gameController.MoneyTransaction(+total);
                }
                else
                    gameController.DisplayInfo("You do not have enough items.");
            }
        }
        catch (FormatException e)
        {
            gameController.DisplayInfo("Invalid quantity 1.");
        }
    }

    private void Eat()
    {
        int total = totalValue;
        try
        {
            int isValid = 0;
            for (int i = 0; i < numItemSlots; i++)
            {
                InputField quantity = gameObjects[Tag.itemQuantity][i].GetComponent<InputField>();
                if (Int32.Parse(quantity.text) == 0)
                    isValid++;
                else
                    for (int j = 0; j < Inventory.numItemSlots; j++)
                        if (gameController.inventory.types[j] != null && itemTypes[i].Equals(gameController.inventory.types[j]))
                            if (Int32.Parse(quantity.text) <= Int32.Parse(gameController.inventory.quantities[j].text))
                                isValid++;
            }
            if (isValid == numItemSlots)
            {
                for (int i = 0; i < numItemSlots; i++)
                {
                    InputField quantity = gameObjects[Tag.itemQuantity][i].GetComponent<InputField>();
                    if (Int32.Parse(quantity.text) != 0)
                        gameController.inventory.RemoveItem(itemTypes[i], Int32.Parse(quantity.text));
                    quantity.text = "0";
                }
                if (mode == Mode.animalEating)
                {
                    if (animalName != null)
                        gameController.animalFarm.addAnimalFood(animalName, total);
                }
                else
                    gameController.players[gameController.activePlayer].ChangeHunger(total);
            }
            else
                gameController.DisplayInfo("You do not have enough items.");
        }
        catch (FormatException e)
        {
            gameController.DisplayInfo("Invalid quantity.");
            return;
        }
    }

}

