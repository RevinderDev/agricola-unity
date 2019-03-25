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

    private Queue<Vector3> actionQueue; // poczatkowo tylko wektor pozycji
    private bool doActionPressed = false;



    // before the first frame update
    void Start()
    {
        actionQueue = new Queue<Vector3>();
        agent.speed = 6f; // test
        health = 100;
        maxHealth = 100;
        actualizeHealthBar();
    }

    void actualizeHealthBar()
    {
        Image bar = GameObject.Find("Bar").GetComponent<Image>();
        bar.rectTransform.localScale = new Vector2(health / maxHealth, 1f);
    }

    // once per frame
    void Update()
    {
        addPointedAction();


        if (doActionPressed)
        {
            doAction();
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
            }
        }
    }

    private void doAction()
    {
        if (isPathFinished())
        {
            if(actionQueue.Count > 0)
                agent.SetDestination(actionQueue.Dequeue());
        }

        if (actionQueue.Count == 0)
        {
            doActionPressed = false;
        }
    }


    public void StartActionQueue()
    {

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

}



