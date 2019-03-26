using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
    {


    public Camera cam;
    public NavMeshAgent agent;
    public int health;
    public int maxHealth;
    public int hunger;
    public int maxHunger;
    public int quequeVisualPosition;
    public int queueVisualInterspace;
    public Vector2 queueVisualElementSize;
    public List<Button> listVisualElements;

    private Queue<Vector3> actionQueue; // poczatkowo tylko wektor pozycji
    private bool doActionPressed = false;



    // before the first frame update
    void Start()
    {
        actionQueue = new Queue<Vector3>();
        agent.speed = 6f; // test
        health = 100;
        maxHealth = 100;
        hunger = 50;
        maxHunger = 50;
        quequeVisualPosition = 7;
        queueVisualInterspace = 14;
        queueVisualElementSize = new Vector2(20, 20);
        actualizeHealthBar();
        actualizeHungerBar();
    }

    void actualizeHealthBar()
    {
        Image bar = GameObject.Find("HealthBar").GetComponent<Image>();
        bar.rectTransform.localScale = new Vector2(health / maxHealth, 1f);
        Text value = GameObject.Find("HealthValue").GetComponent<Text>();
        value.text =  health.ToString() + "/" + maxHealth.ToString();
    }

    void actualizeHungerBar()
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
            doAction();
        }
        else
        {
            addPointedAction();
        }
    }


    private void addPointedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                actionQueue.Enqueue(hit.point);
                addQuequeElement();
            }
        }
    }

    private void doAction()
    {
        if (isPathFinished())
        {
            if (actionQueue.Count > 0) { 
                agent.SetDestination(actionQueue.Dequeue());
                removeQuequeElement(0);
            }
            else
            {
                doActionPressed = false;
                Button playButton = GameObject.Find("PlayButton").GetComponent<Button>();
                playButton.interactable = true;
                quequeVisualPosition = 7;
            }
        }
    }


    public void StartActionQueue()
    {
        Button playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        playButton.interactable = false;
        doActionPressed = true;

    }

    private bool isPathFinished()
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

    public void addQuequeElement()
    {
        GameObject mCanvas = GameObject.Find("Canvas");
        GameObject button = new GameObject();

        button.AddComponent<CanvasRenderer>();
        button.AddComponent<RectTransform>();
        button.layer = 5;
        Button mButton = button.AddComponent<Button>();
        Image mImage = button.AddComponent<Image>();
        mButton.targetGraphic = mImage;
        button.transform.SetParent(mCanvas.transform);


        RectTransform rectTransform = button.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(quequeVisualPosition + queueVisualInterspace/2, -queueVisualInterspace, 0);
        rectTransform.sizeDelta = queueVisualElementSize;
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);

        quequeVisualPosition += queueVisualInterspace/2 + (int)queueVisualElementSize.x;

        listVisualElements.Add(mButton);
    }

    public void removeQuequeElement(int i)
    {
        Destroy(listVisualElements[i].gameObject);
        listVisualElements.RemoveAt(i);
        for(int j = i; j< listVisualElements.Count; j++)
        {
            int move = queueVisualInterspace / 2 + (int)queueVisualElementSize.x;
            RectTransform rectTransform = listVisualElements[j].GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(rectTransform.localPosition.x - move,rectTransform.localPosition.y, 0);
        }
        
    }
}



