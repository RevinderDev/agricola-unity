using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
    {


    public Camera cam;
    public NavMeshAgent agent;



    private Queue<Vector3> actionQueue; // poczatkowo tylko wektor pozycji
    private bool doActionPressed = false;



    // before the first frame update
    void Start()
    {
        actionQueue = new Queue<Vector3>();
        agent.speed = 6f; // test
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



