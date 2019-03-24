using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
    {
    public Camera cam;
    public NavMeshAgent agent;

    private List<Vector3> actionList; // poczatkowo tylko wektor pozycji

    // Start is called before the first frame update
    void Start()
    {
        actionList = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                //agent.SetDestination(hit.point);
                actionList.Add(hit.point);
            }
        }
    }


    public void StartActionQueue()
    {
        //TODO: Tutaj zaczac zabawe z kolejkowaniem :) Generalnie obecnie to nie dziala dobrze w ogole wiec feel free wywalic calosc
        for(int i = 0 ; i < actionList.Count;  )
        {
     
            if ( agent.speed == 0 )
                agent.SetDestination(actionList[i]);

            float dist = agent.remainingDistance;
            if (agent.pathStatus == NavMeshPathStatus.PathComplete)
            {
                print(actionList[i]);
                // doszedl ziomek na miejsce
                i++;
                //actionList.Dequeue();
            }
        }

        actionList.Clear();

    }
}
