using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    PlayerController player;
    public PlayerActionList actionList;
    private bool doActionPressed = false;
    private Farmland farmland;

    // Start is called before the first frame update
    void Start()
    {
        farmland = new Farmland();
        actionList = new PlayerActionList();
        player = SceneManager.Instance.player;
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

    public static void RemoveGameObject(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    public void StartActionQueue()
    {
        Button playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        playButton.interactable = false;
        doActionPressed = true;
    }

    public void DoAction()
    {
        if (player.IsActionFinished())
        {
            //action finished, check for next
            if (actionList.Count() > 0)
            {
                player.SetDestination(actionList.GetDestination());
            }
            else
            {
                doActionPressed = false;
                Button playButton = GameObject.Find("PlayButton").GetComponent<Button>();
                playButton.interactable = true;
                actionList.quequeCurrentPosition = 7;
            }
            if (player.IsPathFinished())
            {

                if (actionList.Count() > 0)
                {
                    //at the destination, perform actions
                    player.SetAction(actionList.GetActionLength());
                    RemoveGameObject(actionList.Remove(0).gameObject);
                }
            }
        }
    }

    public void OnClickPlayButton()
    {
        StartActionQueue();
    }

    private void AddPointedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                actionList.Add(hit.point, 1000);
            }
        }
    }

}
