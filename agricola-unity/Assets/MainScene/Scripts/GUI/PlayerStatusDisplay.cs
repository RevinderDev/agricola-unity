using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerStatusDisplay : EventTrigger
{
    GameController gameController;
    public int id = 0;

    void Start()
    {
        id = System.Int32.Parse(gameObject.name[gameObject.name.Length - 1] + "");
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
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
        gameController.players[id].ActualizeHealthBar();
        gameController.players[id].ActualizeHungerBar();
        gameController.players[id].ActualizeIcon();
        gameController.players[id].ActualizeAgeBar();
    }

}
