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
    public Vector3 homePosition;
    public Vector3 deadPosition;
    public bool isActive = false;
    public int lifeLength;

    private Stopwatch actionStopwatch;
    private int currentActionLengh;
    public ActionController actionController;
    private static GameController gameController;
    private GameObject timeBarObject;

    void Start() {
        lifeLength = 20;
        agent.speed = 6f; // test
        health = 100;
        maxHealth = 100;
        hunger = 50;
        maxHunger = 50;
        currentActionLengh = 0;
        actionStopwatch = Stopwatch.StartNew();
        actionStopwatch.Stop();
        if(gameController == null)
            gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    public void Setup()
    {
        id = gameController.players.Count - 1;
        actionController = GameObject.Find("Player" + id).GetComponent<ActionController>();
        timeBarObject = GameObject.Find("TimeBarObject" + id);
        timeBarObject.SetActive(false);
    }

    public void SetHomeLocalization(Vector3 homePosition)
    {
        this.homePosition = homePosition;
    }

    public void SetDeadLocalization(Vector3 deadPosition)
    {
        this.deadPosition = deadPosition;
    }

    public bool IsAlive()
    {
        return health > 0;
    }

    public void SetActive()
    {
        isActive = true;
        health = maxHealth;
        hunger = maxHunger;
        timeBarObject.SetActive(true);
        ActualizeHealthBar();
        ActualizeHungerBar();
        ActualizeTimeBar();
        ActualizeAgeBar();
        ActualizeIcon();
        GameObject.Find("BarValue" + (id)).GetComponent<Image>().color
                    = GameObject.Find("Player" + (id)).GetComponent<MeshRenderer>().material.color;
        SetDestination(homePosition);
    }

    public void SetInactive()
    {
        isActive = false;
        timeBarObject.SetActive(false);
        GameObject.Find("Player" + id).GetComponent<Transform>().position = deadPosition;
        SetDestination(deadPosition);
    }

    public void ActualizeHealthBar()
    {
        Image bar = GameObject.Find("HealthBar").GetComponent<Image>();
        bar.rectTransform.localScale = new Vector2((float)health / maxHealth, 1f);
        Text value = GameObject.Find("HealthValue").GetComponent<Text>();
        value.text = health.ToString() + "/" + maxHealth.ToString();
        //if lower than... do...
    }

    public void ActualizeAgeBar()
    {

        Image bar = GameObject.Find("AgeBar").GetComponent<Image>();
        bar.rectTransform.localScale = new Vector2((float)actionController.age / lifeLength, 1f);
        Text value = GameObject.Find("AgeValue").GetComponent<Text>();
        value.text = actionController.age.ToString() + "/" + lifeLength.ToString();
        //if lower than... do...
    }

    public void ActualizeTimeBar()
    {
        float timeUsed = (float)gameController.actionList.ActionsLengthsSum() / 1000;
        float timeLeft = (float)gameController.dayLength / 1000 - timeUsed;
        float totalTime = (float)gameController.dayLength / 1000;

        try
        {
            Image bar = GameObject.Find("TimeBar" + id).GetComponent<Image>();
            bar.rectTransform.localScale = new Vector2(timeLeft / totalTime, 1f);
            // Label off/on
            //value.text = "";
            Text value = GameObject.Find("TimeValue" + id).GetComponent<Text>();
            value.text = timeLeft.ToString() + "/" + totalTime.ToString() + "h";
            GameObject.Find("BarValue" + (id)).GetComponent<Image>().color
            = GameObject.Find("Player" + (id)).GetComponent<MeshRenderer>().material.color;
        }
        catch(Exception e) { }

    }

    public bool IsHungry()
    {
        return hunger != maxHunger;
    }

    public bool IsStarving()
    {
        return hunger == 0;
    }

    public void ActualizeHungerBar()
    {
        Image bar = GameObject.Find("HungerBar").GetComponent<Image>();
        bar.rectTransform.localScale = new Vector2((float)hunger / maxHunger, 1f);
        Text value = GameObject.Find("HungerValue").GetComponent<Text>();
        value.text = hunger.ToString() + "/" + maxHunger.ToString();
    }

    public void ActualizeIcon()
    {
        GameObject.Find("PlayerImage").GetComponent<Image>().color
            = GameObject.Find("Player" + (id)).GetComponent<MeshRenderer>().material.color;
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
        //TODO: DEBUG FIX
        currentActionLengh = 1;
        if (actionStopwatch.ElapsedMilliseconds >= currentActionLengh)
        {
            actionStopwatch.Stop();
            gameController.PerformAction(agent.transform.position);
            return true;
        }
        return false;
    }
}



