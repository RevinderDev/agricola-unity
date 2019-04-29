using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public PlayerController player;
    private Information info;
    private QuestionWindow questionWindow;
    private ItemSelection itemSelection;
    private Farmland farmland;
    public AnimalFarm animalFarm { get; set; }
    public ActionList actionList;
    public Inventory inventory;
    public DropdownSelect dropdown;
    public int money;
    private readonly int dayLength = 12000;
    private int currentDay = 0;
    private Vector3 homePosition = new Vector3(-5.3f, 1, 17);
    private Vector3 marketPosition = new Vector3(27f, 1, -48);
    private List<ActionController> controlledObjects = new List<ActionController>();

    private bool isPlayButtonPressed;
    Button playButton;

    // Start is called before the first frame update
    void Start()
    {
        farmland = new Farmland();
        animalFarm = new AnimalFarm();
        actionList = new ActionList();
        player = SceneManager.Instance.player;
        isPlayButtonPressed = false;
        playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        inventory = FindObjectOfType<Inventory>();
        inventory.AddItem(ItemType.tomatoSeeds, 5);
        inventory.AddItem(ItemType.carrotSeeds, 5);
        inventory.AddItem(ItemType.pumpkinSeeds, 1);
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
        money = 10;
        MoneyTransaction(0);
        ActualizeTimeBar();
        Text dayLabel = GameObject.Find("DayLabel").GetComponent<Text>();
        dayLabel.text = "Day " + (currentDay++).ToString();
        dropdown = FindObjectOfType<DropdownSelect>();
        dropdown.Hide();
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

    // once per frame
    void Update()
    {
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
        else if (actionList.GetAction() == ActionType.buyCow)
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
            animalFarm.feedCow(actionList.GetGameObject());
        }

        // Delete action from queque
        RemoveGameObject(actionList.Remove(0).gameObject);
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
        if (player.IsActionFinished())
        {
            //action finished, check for next
            if (actionList.Count() > 0)
            {
                if (actionList.GetGameObject() == null)
                    if (actionList.Count() == 1)        // Last action, just walk home
                        player.SetDestination(homePosition);
                    else                                // Market action, walk somewhere
                        player.SetDestination(marketPosition);

                else
                    player.SetDestination(actionList.GetDestination());
            }
            else
            {
                NextDay();
            }
            if (player.IsPathFinished())
            {
                if (actionList.Count() > 0)
                {
                    //at the destination, perform actions
                    player.SetAction(actionList.GetAction());
                }
            }
        }
    }

    public void OnClickPlayButton()
    {
        StartActionQueue();
    }
    // Applies some changes to the game view
    public void NextDay()
    {
        isPlayButtonPressed = false;
        playButton.interactable = true;
        farmland.GrowPlants();
        ActualizeTimeBar();
        Text dayLabel = GameObject.Find("DayLabel").GetComponent<Text>();
        dayLabel.text = "Day " + (currentDay++).ToString();
        player.ChangeHunger(-10);
        animalFarm.spoilFood();
        animalFarm.generateFoodProducts();
        animalFarm.ageAnimals();
        foreach (ActionController actionController in controlledObjects)
            actionController.age += 1;
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
            return "Action already in queque.";
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
        if (type == ActionType.buyCow)
            if (!animalFarm.isSlotAvailable(gameObject))
                return "Slots taken by another cow.";
        if (type == ActionType.gatherMilk)
            if (animalFarm.getMilkCount() <= 0)
                return "No milks to gather.";
        if (type == ActionType.checkCowStatus)
            return "No action to be done.";
        return null;
    }

    public void ActualizeTimeBar()
    {
        float timeUsed = (float)actionList.ActionsLengthsSum()/1000;
        float timeLeft = (float)dayLength/1000 - timeUsed;
        float totalTime = (float)dayLength / 1000;

        Image bar = GameObject.Find("TimeBar").GetComponent<Image>();
        bar.rectTransform.localScale = new Vector2(timeLeft / totalTime, 1f);
        Text value = GameObject.Find("TimeValue").GetComponent<Text>();
        // Label off/on
        //value.text = "";
        value.text = timeLeft.ToString() + "/" + totalTime.ToString() + "h";
    }

    // Add action only if animation is NOT in progress
    public void AddAction(GameObject gameObject, ActionType type)
    {
        string message = IsAcctionAllowed(gameObject, type);
        if (message == null)
        {
            if(type == ActionType.plant)
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
    public GameObject InstantiatePrefab(Object prefab, Vector3 vector, Quaternion identity)
    {
        return Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
    }
    // Providing Destroy method for other classes (context problem)
    public static void RemoveGameObject(GameObject gameObject)
    {
        Destroy(gameObject);
    }
}
