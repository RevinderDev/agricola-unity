using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerActionList
{
    public int quequeVisualPosition;
    public int queueVisualInterspace;
    public Vector2 queueVisualElementSize;

    private int count;
    private List<Button> visualElementsList;
    private List<Vector3> actionList;

    GameObject mCanvas;

    public PlayerActionList()
    {
        quequeVisualPosition = 7;
        queueVisualInterspace = 14;
        queueVisualElementSize = new Vector2(20, 20);
        mCanvas = GameObject.Find("Canvas");
        count = 0;
        actionList = new List<Vector3>();
        visualElementsList = new List<Button>();
    }

    public void Add(Vector3 action)
    {
        actionList.Add(action);
        visualElementsList.Add(CreateButton());
        count++;
    }

    public Button Remove(int i)
    {
        Button buttonToBeDestroyed = visualElementsList[i];
        visualElementsList.RemoveAt(i);
        actionList.RemoveAt(i);
        for (int j = i; j < visualElementsList.Count; j++)
        {
            int move = queueVisualInterspace / 2 + (int)queueVisualElementSize.x;
            RectTransform rectTransform = visualElementsList[j].GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(rectTransform.localPosition.x - move, rectTransform.localPosition.y, 0);
        }
        count--;
        return buttonToBeDestroyed;
    }

    public Button Remove(Button button)
    {
        int i = visualElementsList.IndexOf(button);
        return Remove(i);
    }

    public Vector3 GetDestination()
    {
        return actionList[0];
    }

    public Button CreateButton()
    {
        GameObject gameObject = new GameObject();
        gameObject.AddComponent<CanvasRenderer>();
        gameObject.AddComponent<RectTransform>();
        gameObject.layer = 5;

        Button button = gameObject.AddComponent<Button>();
        Image image = gameObject.AddComponent<Image>();
        button.targetGraphic = image;
        gameObject.transform.SetParent(mCanvas.transform);
        //Random color test only
        Color newColor = new Color(Random.value, Random.value, Random.value, 1.0f);
        button.GetComponent<Image>().color = newColor;

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(quequeVisualPosition + queueVisualInterspace / 2, -queueVisualInterspace, 0);
        rectTransform.sizeDelta = queueVisualElementSize;
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);

        quequeVisualPosition += queueVisualInterspace / 2 + (int)queueVisualElementSize.x;

        button.onClick.AddListener(delegate {
            Remove(button);
            PlayerController.RemoveGameObject(button.gameObject);
        });
        return button;
    }

    public int Count()
    {
        return count;
    }
}


public class PlayerController : MonoBehaviour
{

    public Camera cam;
    public NavMeshAgent agent;
    public int health;
    public int maxHealth;
    public int hunger;
    public int maxHunger;
    public PlayerActionList actionList;
    private bool doActionPressed = false;

    // before the first frame update
    void Start()
    {
        agent.speed = 6f; // test
        health = 100;
        maxHealth = 100;
        hunger = 50;
        maxHunger = 50;
        ActualizeHealthBar();
        ActualizeHungerBar();
        actionList = new PlayerActionList();
    }

    void ActualizeHealthBar()
    {
        Image bar = GameObject.Find("HealthBar").GetComponent<Image>();
        bar.rectTransform.localScale = new Vector2(health / maxHealth, 1f);
        Text value = GameObject.Find("HealthValue").GetComponent<Text>();
        value.text = health.ToString() + "/" + maxHealth.ToString();
        //if lower than... do...
    }

    void ActualizeHungerBar()
    {
        Image bar = GameObject.Find("HungerBar").GetComponent<Image>();
        bar.rectTransform.localScale = new Vector2(hunger / maxHunger, 1f);
        Text value = GameObject.Find("HungerValue").GetComponent<Text>();
        value.text = hunger.ToString() + "/" + maxHunger.ToString();
        //if lower than... do...
    }

    // once per frame
    void Update()
    {
        if (doActionPressed)
        {
            DoAction();
        }
        else
        {
            AddPointedAction();
        }
    }


    private void AddPointedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                actionList.Add(hit.point);
            }
        }
    }

    public static void RemoveGameObject(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    private void DoAction()
    {
        if (IsPathFinished())
        {
            if (actionList.Count() > 0)
            {
                agent.SetDestination(actionList.GetDestination());
                RemoveGameObject(actionList.Remove(0).gameObject);
            }
            else
            {
                doActionPressed = false;
                Button playButton = GameObject.Find("PlayButton").GetComponent<Button>();
                playButton.interactable = true;
                actionList.quequeVisualPosition = 7;
            }
        }
    }

    public void StartActionQueue()
    {
        Button playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        playButton.interactable = false;
        doActionPressed = true;
    }

    private bool IsPathFinished()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }

        return false;
    }
}



