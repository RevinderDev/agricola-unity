using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System;
using System.Diagnostics;

public class PlayerController : MonoBehaviour
{
    public NavMeshAgent agent;
    public int id;
    private int health;
    private int maxHealth;
    private int hunger;
    private int maxHunger;
    public Vector3 home;

    private Stopwatch actionStopwatch;
    private int currentActionLengh;
    private GameController gameController;

    void Start() { 
        agent.speed = 6f; // test
        health = 100;
        maxHealth = 100;
        hunger = 50;
        maxHunger = 50;
        ActualizeHealthBar();
        ActualizeHungerBar();
        currentActionLengh = 0;
        actionStopwatch = Stopwatch.StartNew();
        actionStopwatch.Stop();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    public void setId()
    {
        id = gameController.players.Count - 1;
    }

    public void setHomeLocalization(Vector3 homePosition)
    {
        home = homePosition;
    }

    public bool IsAlive()
    {
        return health > 0;
    }

    void ActualizeHealthBar()
    {
        Image bar = GameObject.Find("HealthBar").GetComponent<Image>();
        bar.rectTransform.localScale = new Vector2((float)health / maxHealth, 1f);
        Text value = GameObject.Find("HealthValue").GetComponent<Text>();
        value.text = health.ToString() + "/" + maxHealth.ToString();
        //if lower than... do...
    }

    public void ActualizeTimeBar()
    {
        float timeUsed = (float)gameController.actionList.ActionsLengthsSum() / 1000;
        float timeLeft = (float)gameController.dayLength / 1000 - timeUsed;
        float totalTime = (float)gameController.dayLength / 1000;

        Image bar = GameObject.Find("TimeBar" + id).GetComponent<Image>();
        bar.rectTransform.localScale = new Vector2(timeLeft / totalTime, 1f);
        Text value = GameObject.Find("TimeValue" + id).GetComponent<Text>();
        // Label off/on
        //value.text = "";
        value.text = timeLeft.ToString() + "/" + totalTime.ToString() + "h";
    }

    public bool IsHungry()
    {
        return hunger != maxHunger;
    }

    void ActualizeHungerBar()
    {
        Image bar = GameObject.Find("HungerBar").GetComponent<Image>();
        bar.rectTransform.localScale = new Vector2((float)hunger / maxHunger, 1f);
        Text value = GameObject.Find("HungerValue").GetComponent<Text>();
        value.text = hunger.ToString() + "/" + maxHunger.ToString();
        if (hunger == 0)
        {
            ChangeHalth(-10);
        }
    }

    public void ChangeHunger(int value)
    {
        if (hunger == 0 && value < 0 || hunger == maxHunger && value > 0)
            ;
        else if (hunger + value > maxHunger)
            hunger = maxHunger;
        else if (hunger + value < 0)
            hunger = 0;
        else
            hunger += value;
        ActualizeHungerBar();
    }

    public void ChangeHalth(int value)
    {
        if (health == 0 && value < 0 || health == maxHealth && value > 0)
            ;
        else if (health + value > maxHealth)
            health = maxHealth;
        else if (health + value < 0)
            health = 0;
        else
            health += value;
        ActualizeHealthBar();
    }

    public void SetDestination(Vector3 target)
    {
        agent.SetDestination(target);
    }

    public bool IsPathFinished()
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

    public void SetAction(ActionType actionType)
    {
        currentActionLengh = actionType.length;
        if (actionType == ActionType.market)
            currentActionLengh = 0;
        actionStopwatch.Restart();
    }

    // Checks if player spend enough time performing action
    public bool IsActionFinished()
    {
        if (ItemSelection.isVisible == true)
            return false;
        if (!actionStopwatch.IsRunning)
            return true;
        if (actionStopwatch.ElapsedMilliseconds >= currentActionLengh)
        {
            actionStopwatch.Stop();
            gameController.PerformAction(agent.transform.position);
            return true;
        }
        return false;
    }
}



