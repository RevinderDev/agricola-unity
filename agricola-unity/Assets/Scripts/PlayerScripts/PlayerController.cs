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
    public int health;
    public int maxHealth;
    public int hunger;
    public int maxHunger;

    public Stopwatch actionStopwatch;
    public int currentActionLengh;

    // before the first frame update
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

    public void SetAction(int actionLength)
    {
        currentActionLengh = actionLength;
        actionStopwatch.Start();
    }

    public bool IsActionFinished()
    {
        UnityEngine.Debug.Log(actionStopwatch.ElapsedMilliseconds);
        if (!actionStopwatch.IsRunning || actionStopwatch.ElapsedMilliseconds >= currentActionLengh)
        {
            actionStopwatch.Stop();
            actionStopwatch.Reset();
            return true;
        }
        return false;
    }
}



