using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour
{
    public List<PlayerController> players;
    public int activePlayer = 0;
    public List<GameObject> timeBarObjects;
    public int maxNumberOfPlayers = 4;
    private Information info;
    private QuestionWindow questionWindow;
    private ItemSelection itemSelection;
    private AnimalFoodWindow animalFoodWindow;
    private Farmland farmland;
    public AnimalFarm animalFarm { get; set; }
    public ActionList actionList;
    public Inventory inventory;
    public DropdownSelect dropdown;
    public int money;
    private readonly int lifeLength = 10;
    public readonly int dayLength = 12000;
    private int currentDay = 0;
    //private Vector3 homePosition = new Vector3(-5.3f, 1, 17);
    private Vector3 marketPosition = new Vector3(27f, 1, -48);
    private List<ActionController> controlledObjects = new List<ActionController>();
    private System.Random r = new System.Random();
    private string eventsCommunicate = ""; 

    private bool isPlayButtonPressed;
    Button playButton;

    // Start is called before the first frame update
    void Start()
    {
        farmland = new Farmland();
        animalFarm = new AnimalFarm();
        actionList = new ActionList();

        players = new List<PlayerController>();
        timeBarObjects = new List<GameObject>();
        for (int i = 0; i < maxNumberOfPlayers; i++)
        {
            timeBarObjects.Add(GameObject.Find("TimeBarObject" + i));
            timeBarObjects[i].SetActive(false);
        }
        AddNewPlayer();
        AddNewPlayer();

        isPlayButtonPressed = false;
        playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        inventory = FindObjectOfType<Inventory>();

        inventory.AddItem(ItemType.tomatoSeeds, 5);
        inventory.AddItem(ItemType.carrotSeeds, 5);
        inventory.AddItem(ItemType.pumpkinSeeds, 1);
        inventory.AddItem(ItemType.chicken, 5);

        info = new Information(GameObject.Find("InformationObject"),
            GameObject.Find("InformationText").GetComponent<Text>());
        info.Hide();

        questionWindow = new QuestionWindow(GameObject.Find("WindowObject"), 
            GameObject.Find("WindowQuestion").GetComponent<Text>(),
            GameObject.Find("ButtonYes").GetComponent<Button>(), 
            GameObject.Find("ButtonNo").GetComponent<Button>());
        questionWindow.Hide();

        ItemType.Initialize();
        itemSelection = FindObjectOfType<ItemSelection>();
        itemSelection.SetMarket();
        itemSelection.Hide();

        money = 1000;
        MoneyTransaction(0);
       
        Text dayLabel = GameObject.Find("DayLabel").GetComponent<Text>();
        dayLabel.text = "Day " + (currentDay++).ToString();
        dropdown = FindObjectOfType<DropdownSelect>();
        dropdown.Hide();
    }

    public void AddNewPlayer()
    {
        timeBarObjects[players.Count].SetActive(true);
        players.Add(GameObject.Find("Player" + players.Count).GetComponent<PlayerController>());
        players[players.Count-1].setId();
        players[players.Count - 1].setHomeLocalization(new Vector3(-4 + players.Count - 1, 1, 17));
        players[players.Count-1].ActualizeTimeBar();
        GameObject.Find("BarValue" + (players.Count - 1)).GetComponent<Image>().color 
            = GameObject.Find("Player" + (players.Count - 1)).GetComponent<MeshRenderer>().material.color;
    }


    public void AddControlledObject(ActionController actionController)
    {
        controlledObjects.Add(actionController);
    }

    public void RemoveControlledObject(ActionController actionController)
    {
        controlledObjects.Remove(actionController);
    }

    public int GetMoney()
    {
        return money;
    }

    public void MoneyTransaction(int transactionAmount)
    {
        money += transactionAmount;
        GameObject.Find("MoneyValue").GetComponent<Text>().text = money.ToString();
    }

    public int GetMilkSpoilage()
    {
        return animalFarm.getMilkSpoilage();
    }

    public int GetMilkCount()
    {
        return animalFarm.getMilkCount();
    }

    public int GetEggSpoilage()
    {
        return animalFarm.getEggSpoilage();
    }

    public int GetEggCount()
    {
        return animalFarm.getEggCount();
    }

    // once per frame
    void Update()
    {
        // TODO: null tutaj?
        if (questionWindow.WasQuestionAsked() && questionWindow.GetQuestionTag() == "Play")
        {
            if (questionWindow.WasQuestionAnswered())
            {
                if (questionWindow.GetAnswer() == true)
                {
                     isPlayButtonPressed = true;
                    // Add final action (walk back home) - transparent
                    actionList.Add(null, ActionType.walk);
                }
                else
                {
                    playButton.interactable = true;
                }
            }
        }

        if (isPlayButtonPressed)
        {
            DoAction();
        }
     
        if(info.hide == true)
        {
            info.Hide();
        }
    }

    // Specifies actions related to the execution of a given action 
    public void PerformAction(Vector3 position) 
    {
        // Custom reaction
        if (actionList.GetAction() == ActionType.walk)
            ;
        else if (actionList.GetAction() == ActionType.plant)
            switch (actionList.GetItemTypeRequired().name)
            {
                case "carrot seeds":
                    farmland.AddPlant(actionList.GetGameObject(), PlantType.carrot);
                    break;
                case "tomato seeds":
                    farmland.AddPlant(actionList.GetGameObject(), PlantType.tomato);
                    break;
                case "pumpkin seeds":
                    farmland.AddPlant(actionList.GetGameObject(), PlantType.pumpkin);
                    break;
            }
        else if (actionList.GetAction() == ActionType.collectPlant)
            farmland.CollectPlant(actionList.GetGameObject());
        else if (actionList.GetAction() == ActionType.placeCow)
            animalFarm.addCow(actionList.GetGameObject());
        else if (actionList.GetAction() == ActionType.gatherMilk)
            animalFarm.gatherMilk();
        else if (actionList.GetAction() == ActionType.market)
        {
            itemSelection.SetMode(ItemSelection.Mode.market);
            itemSelection.Initialize();
            itemSelection.Display();
        }
        else if (actionList.GetAction() == ActionType.eat)
        {
            itemSelection.SetMode(ItemSelection.Mode.eating);
            itemSelection.Initialize();
            itemSelection.Display();
        }
        else if (actionList.GetAction() == ActionType.feedCow)
        {
            //animalFoodWindow.Display();
            itemSelection.SetMode(ItemSelection.Mode.animalEating);
            itemSelection.Initialize();
            itemSelection.animalName = "cows";
            itemSelection.Display();
        }
        else if (actionList.GetAction() == ActionType.placeChicken)
        {
            animalFarm.addChicken(actionList.GetGameObject());
        }
        else if (actionList.GetAction() == ActionType.feedChicken)
        {
            itemSelection.SetMode(ItemSelection.Mode.animalEating);
            itemSelection.Initialize();
            itemSelection.animalName = "chickens";
            itemSelection.Display();
        }
        else if (actionList.GetAction() == ActionType.gatherEgg)
        {
            animalFarm.gatherEgg();
        }
        //Random events
        RandomEvents(actionList.GetAction().associatedEvents);
        // Delete action from queque
        RemoveGameObject(actionList.Remove(0).gameObject);
    }

    public void RandomEvents(IReadOnlyCollection<ActionEvent> eventsCollection)
    {
        foreach (ActionEvent actionEvent in eventsCollection)
        {
            int rInt = r.Next(0, 100);
            // Perform event
            if (100 - actionEvent.probability * 100 <= rInt)
            {
                eventsCommunicate += actionEvent.description + "\n\n";
                PerformEventAction(actionEvent);
                break;
            }
        }
    }

    private void PerformEventAction(ActionEvent actionEvent)
    {
        players[activePlayer].ChangeHalth(actionEvent.healthChange);
        players[activePlayer].ChangeHunger(actionEvent.hungerChange);
        foreach (ItemType itemType in actionEvent.itemsChange.Keys)
        {
            int value = actionEvent.itemsChange[itemType];
            if (value > 0)
                inventory.AddItem(itemType, value);
            else if (value < 0)
                inventory.RemoveItem(itemType, -value);
        }
        
    }

    public void StartActionQueue()
    {
        playButton.interactable = false;
        if(actionList.ActionsLengthsSum() < dayLength)
        {
            double timeUsed = (double)actionList.ActionsLengthsSum() / 1000;
            double timeLeft = ((double)dayLength / 1000 - timeUsed);
            questionWindow.DisplayQuestion("You used " + timeUsed + "h of your day time, " + 
                "but you still have " + timeLeft +  "h. " +
                "Are you sure you want to continue?", "Play");
            isPlayButtonPressed = false;
        }
        else
        {
            isPlayButtonPressed = true;
            if(actionList.GetAction() == ActionType.market)
                actionList.Add(null, ActionType.walk);
            // Add final action (walk back home) - transparent
            actionList.Add(null, ActionType.walk);
        }
    }

    // Manages the execution of the action queue. 
    public void DoAction()
    {
        if (players[activePlayer].IsActionFinished())
        {
            //action finished, check for next
            if (actionList.Count() > 0)
            {
                if (actionList.GetGameObject() == null)
                    if (actionList.Count() == 1)        // Last action, just walk home
                        players[activePlayer].SetDestination(players[activePlayer].home);
                    else                                // Market action, walk somewhere
                        players[activePlayer].SetDestination(marketPosition);

                else
                    players[activePlayer].SetDestination(actionList.GetDestination());
            }
            else
            {
                NextDay();
            }
            if (players[activePlayer].IsPathFinished())
            {
                if (actionList.Count() > 0)
                {
                    //at the destination, perform actions
                    players[activePlayer].SetAction(actionList.GetAction());
                }
            }
        }
    }

    public void OnClickPlayButton()
    {
        StartActionQueue();
    }

    private void KillPlayers()
    {
        for (int i = 0; i < players.Count; i++) //foreach player
            if (players[activePlayer].GetComponent<ActionController>().age > lifeLength || !players[i].IsAlive())
            {
                //Remove player (add grave? xD)
                //player.gameObject.SetActive(false);
                if (true) //we do not have more players
                {
                    //todo save Score to file?
                    int inventoryValue = inventory.GetInventoryValue() + money;
                    questionWindow.DisplayQuestion("All yours subordinates died. The game is over. Do you want to play again? \nScore: " + inventoryValue + 
                        "\nDay: " + currentDay, "Game over");
                }
            }

    }

    // Applies some changes to the game view
    public void NextDay()
    {
        isPlayButtonPressed = false;
        playButton.interactable = true;
        if (eventsCommunicate != "")
        {
            questionWindow.DisplayQuestion(eventsCommunicate, "Action event", true);
            eventsCommunicate = "";
        }
        if (activePlayer < players.Count - 1)
        {
            activePlayer++;
            players[activePlayer].ActualizeHealthBar();
            players[activePlayer].ActualizeHungerBar();
            players[activePlayer].ActualizeIcon();
            return;
        }
        else
            activePlayer = 0;
       
        farmland.GrowPlants();
        for(int i = 0; i<players.Count; i++)
            players[i].ActualizeTimeBar();
        Text dayLabel = GameObject.Find("DayLabel").GetComponent<Text>();
        dayLabel.text = "Day " + (currentDay++).ToString();
        animalFarm.spoilFood();
        animalFarm.generateFoodProducts();
        animalFarm.ageAnimals();
        animalFarm.animalsEat();    
        foreach (ActionController actionController in controlledObjects)
            actionController.age += 1;

        for (int i = 0; i < players.Count; i++)
        {
            if (!players[i].IsHungry())
                players[i].ChangeHalth(+5);
            players[i].ChangeHunger(-50); // TODO FIX
        }

        KillPlayers();
    }

    /* Action is not allowed: 
     * 1) if there the same action in queque (same type, same object)
     * 2) planting area is already taken
     * 3) plant can not be collected yet (baby or spoiled plant)
     */
    public string IsAcctionAllowed(GameObject gameObject, ActionType type)
    {
        if (isPlayButtonPressed)
            return "Animation is in progress.";
        if (actionList.IsActionInQueque(gameObject, type)) // TODO: blad tutaj jest z wyswietlaniem komunikatu kiedy action przekroczy sie limit czasu uzywajac tylko krów.
            return "Action already in queue.";
        if (actionList.ActionsLengthsSum() + type.length > dayLength)
            return "Action too long. " + ((double)(dayLength - actionList.ActionsLengthsSum()) / 1000) + "h left.";
        if (type == ActionType.plant)
        {
            if (farmland.IsAreaTaken(gameObject))
                return "This area is already taken.";
            if (!inventory.DoesContain(dropdown.GetSelected()))
                return "You do not have relevant seeds.";
        }
        if (type == ActionType.collectPlant)
            if (!farmland.CanPlantBeCollected(gameObject))
                return "Plant can not be collected.";
        if (type == ActionType.placeCow)
            if (!animalFarm.isSlotAvailable(gameObject))
                return "Slots taken by another cow.";
            else if (!inventory.DoesContain(ItemType.cow))
                return "You don't have any cows.";
        if (type == ActionType.gatherMilk)
            if (animalFarm.getMilkCount() <= 0)
                return "No milks to gather.";
        if (type == ActionType.checkCowStatus || type == ActionType.checkChickenStatus)
            return "No action to be done.";
        if (type == ActionType.placeChicken)
            if (!animalFarm.isSlotAvailable(gameObject))
                return "Slots taken by another chicken.";
            else if (!inventory.DoesContain(ItemType.chicken))
                return "You don't have any chickens.";
        if (type == ActionType.gatherEgg)
            if (animalFarm.getEggCount() <= 0)
                return "No eggs to gather.";
        return null;
    }

    // Add action only if animation is NOT in progress
    public void AddAction(GameObject gameObject, ActionType type)
    {
        string message = IsAcctionAllowed(gameObject, type);
        if (message == null)
        {
            if (type == ActionType.plant)
            {
                switch (dropdown.GetSelected())
                {
                    case "carrot seeds":
                        actionList.Add(gameObject, type, ItemType.carrotSeeds);
                        break;
                    case "tomato seeds":
                        actionList.Add(gameObject, type, ItemType.tomatoSeeds);
                        break;
                    case "pumpkin seeds":
                        actionList.Add(gameObject, type, ItemType.pumpkinSeeds);
                        break;
                }
            }
            else if (type == ActionType.placeCow)
                actionList.Add(gameObject, type, ItemType.cow);
            else if (type == ActionType.placeChicken)
                actionList.Add(gameObject, type, ItemType.chicken);
            else
                actionList.Add(gameObject, type);
        }

            
        else
            info.Display("Not allowed. " + message);
 
    }

    public void DisplayInfo(string message)
    {
        info.Display(message);
    }

    // Providing Instantiate method for other classes (context problem)
    public GameObject InstantiatePrefab(UnityEngine.Object prefab, Vector3 vector, Quaternion identity)
    {
        return Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
    }
    // Providing Destroy method for other classes (context problem)
    public static void RemoveGameObject(GameObject gameObject)
    {
        Destroy(gameObject);
    }
}
