using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    PlayerController player;
    private Information info;
    private QuestionWindow questionWindow;
    private Farmland farmland;
    private AnimalFarm animalFarm;
    public PlayerActionList actionList;
    public Inventory inventory;

    private bool isPlayButtonPressed;
    Button playButton;

    // Start is called before the first frame update
    void Start()
    {
        farmland = new Farmland();
        animalFarm = new AnimalFarm();
        actionList = new PlayerActionList();
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
                    actionList.Add(null, PlayerActionList.ActionType.walk, 0, new Color(0, 0, 0, 0));
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
        switch(actionList.GetActionType()){
            case PlayerActionList.ActionType.walk:
                break;
            case PlayerActionList.ActionType.plant:
                farmland.AddPlant(actionList.GetGameObject(), farmland.carrot);
                break;
            case PlayerActionList.ActionType.collectPlant:
                farmland.CollectPlant(actionList.GetGameObject());
                break;
        }
        // Delete action from queque
        RemoveGameObject(actionList.Remove(0).gameObject);
    }

    public void StartActionQueue()
    {
        playButton.interactable = false;
        if(actionList.Count() < 4)
        {
            questionWindow.DisplayQuestion("You added less than 4 action. " +
                "I am disappointed with your laziness. Are you sure you want to continue?", "Play");
            isPlayButtonPressed = false;
        }
        else
        {
            isPlayButtonPressed = true;
            // Add final action (walk back home) - transparent
            actionList.Add(null, PlayerActionList.ActionType.walk, 0, new Color(0, 0, 0, 0));
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
                    player.SetAction(actionList.GetActionLength());
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
    }

    /* Action is not allowed: 
     * 1) if there the same action in queque (same type, same object)
     * 2) planting area is already taken
     * 3) plant can not be collected yet (baby or spoiled plant)
     */
    public bool IsAcctionAllowed(GameObject gameObject, PlayerActionList.ActionType type)
    {
        if (actionList.IsActionInQueque(gameObject, type))
            return false;
        switch (type)
        {
            case PlayerActionList.ActionType.plant:
                return !farmland.IsAreaTaken(gameObject);
            case PlayerActionList.ActionType.collectPlant:
                return farmland.CanPlantBeCollected(gameObject);
            default:
                return true;
        }
    }

    // Add action only if animation is NOT in progress
    public void AddAction(GameObject gameObject, PlayerActionList.ActionType type, int actionLength, Color actionColor)
    {
        if (!isPlayButtonPressed)
        {
            if (IsAcctionAllowed(gameObject, type))
                actionList.Add(gameObject, type, actionLength, actionColor);
            else
                info.Display("Not allowed. Action already in queque.");
        }
        else
            info.Display("Not allowed. Animation is in progress.");
    }


    // Add action only if animation is NOT in progress
    public void AddAction(GameObject gameObject, PlayerActionList.ActionType type, int actionLength, string imageDirectory)
    {
        if (!isPlayButtonPressed)
        {
            if (IsAcctionAllowed(gameObject, type))
                actionList.Add(gameObject, type, actionLength, imageDirectory);
            else
                info.Display("Not allowed! Action already in queque.");
        }
        else
            info.Display("Not allowed. Animation is in progress.");
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
