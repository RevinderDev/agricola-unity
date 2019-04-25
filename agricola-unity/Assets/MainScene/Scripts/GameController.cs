using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    PlayerController player;
    private Information info;
    private QuestionWindow questionWindow;
    private Market market;
    private Farmland farmland;
    private AnimalFarm animalFarm;
    public ActionList actionList;
    public Inventory inventory;
    public int money;
    private readonly int dayLength = 12000;
    private int currentDay = 0;

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
        info = new Information(GameObject.Find("InformationObject"),
            GameObject.Find("InformationText").GetComponent<Text>());
        info.Hide();
        questionWindow = new QuestionWindow(GameObject.Find("WindowObject"), 
            GameObject.Find("WindowQuestion").GetComponent<Text>(),
            GameObject.Find("ButtonYes").GetComponent<Button>(), 
            GameObject.Find("ButtonNo").GetComponent<Button>());
        questionWindow.Hide();
        Item.ItemType.Initialize();
        market = FindObjectOfType<Market>();
        market.SetMarket();
        market.Hide();
        money = 10;
        MoneyTransaction(0);
        ActualizeTimeBar();
        Text dayLabel = GameObject.Find("DayLabel").GetComponent<Text>();
        dayLabel.text = "Day " + (currentDay++).ToString();
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
                    actionList.Add(null, ActionList.walk);
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
        if (actionList.GetAction() == ActionList.walk)
            ;
        else if (actionList.GetAction() == ActionList.plant)
            farmland.AddPlant(actionList.GetGameObject(), farmland.carrot);
        else if (actionList.GetAction() == ActionList.collectPlant)
            farmland.CollectPlant(actionList.GetGameObject());
        else if (actionList.GetAction() == ActionList.buyCow)
            animalFarm.addCow(actionList.GetGameObject());
        else if (actionList.GetAction() == ActionList.market)
            market.Display();
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
            // Add final action (walk back home) - transparent
            actionList.Add(null, ActionList.walk);
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
                if (actionList.GetGameObject() == null) // Last action, just walk home
                    player.SetDestination(new Vector3(0, 1.5f, 0));
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
    }

    /* Action is not allowed: 
     * 1) if there the same action in queque (same type, same object)
     * 2) planting area is already taken
     * 3) plant can not be collected yet (baby or spoiled plant)
     */
    public string IsAcctionAllowed(GameObject gameObject, ActionList.ActionType type)
    {
        if (isPlayButtonPressed)
            return "Animation is in progress.";
        if (actionList.IsActionInQueque(gameObject, type)) // TODO: blad tutaj jest z wyswietlaniem komunikatu kiedy action przekroczy sie limit czasu uzywajac tylko krów.
            return "Action already in queque.";
        if (actionList.ActionsLengthsSum() + type.length > dayLength)
            return "Action too long. " + ((double)(dayLength - actionList.ActionsLengthsSum()) / 1000) + "h left.";
        if (type == ActionList.plant)
            if(farmland.IsAreaTaken(gameObject))
                return "This area is already taken.";
        if (type == ActionList.collectPlant)
            if (!farmland.CanPlantBeCollected(gameObject))
                return "Plant can not be collected.";
        if (type == ActionList.buyCow)
            if (!animalFarm.isSlotAvailable(gameObject))
                return "Slots already taken by another cow.";
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
        // Label off
        value.text = "";
        //value.text = timeLeft.ToString() + "/" + totalTime.ToString() + "h";
    }

    // Add action only if animation is NOT in progress
    public void AddAction(GameObject gameObject, ActionList.ActionType type)
    {
        string message = IsAcctionAllowed(gameObject, type);
        if (message == null)
            actionList.Add(gameObject, type);
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
