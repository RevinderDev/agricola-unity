using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerStatusDisplay : EventTrigger
{
    GameController gameController;
    public PlayerController player;

    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        player = GameObject.Find("Player" + System.Int32.Parse(gameObject.name[gameObject.name.Length - 1] + "")).GetComponent<PlayerController>();
    }

    override public void OnPointerExit(PointerEventData data)
    {
        gameController.players[gameController.activePlayer].ActualizeHealthBar();
        gameController.players[gameController.activePlayer].ActualizeHungerBar();
        gameController.players[gameController.activePlayer].ActualizeIcon();
        gameController.players[gameController.activePlayer].ActualizeAgeBar();
    }

    override public void OnPointerEnter(PointerEventData data)
    {
        player.ActualizeHealthBar();
        player.ActualizeHungerBar();
        player.ActualizeIcon();
        player.ActualizeAgeBar();
    }

}
