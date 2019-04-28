using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropdownSelect : EventTrigger
{
    public GameObject dropdown;
    private string selected;
    GameController gameController;
    GameObject areaObject;

    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        dropdown = GameObject.Find("Dropdown");
    }

    public void OnOptionSelected(string selected)
    {
        this.selected = selected;
        gameController.AddAction(areaObject, ActionType.plant);
        ActionController.isActive = true;
        Hide();
    }

    public string GetSelected()
    {
        return selected;
    }

    public void Hide()
    {
        dropdown.SetActive(false);
        ActionController.isActive = true;
    }

    public void Display(GameObject areaObject)
    {
        this.areaObject = areaObject;
        var x = Input.mousePosition.x;
        var y = Input.mousePosition.y;
        dropdown.transform.position = new Vector3(x, y, 0);
        dropdown.SetActive(true);
    }

    override public void OnPointerExit(PointerEventData data)
    {
        Hide();
    }
}