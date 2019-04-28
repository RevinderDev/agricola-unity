using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownSelect : MonoBehaviour
{
    public static List<string> dropOptions = new List<string> { "select seeds", "carrot seeds", "tomato seeds" };
    private Dropdown dropdown;
    private string selected;
    GameController gameController;
    GameObject areaObject;
    Rect dropdownRectClosed;
    Rect dropdownRectOpened;


    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        dropdown = GetComponent<Dropdown>();
        dropdown.ClearOptions();
        dropdown.AddOptions(dropOptions);
        dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropdown);
        });
    }

    public string getSelected()
    {
        return selected;
    }

    void DropdownValueChanged(Dropdown change)
    {
        if (dropdown.value != 0)
        {
            selected = dropOptions[dropdown.value];
            gameController.AddAction(areaObject, ActionType.plant);
            Hide();
        }
    }

    public void Hide()
    {
        ActionController.isActive = true;
        dropdown.gameObject.SetActive(false);
        if(dropdown.transform.Find("Dropdown List") != null)
            Destroy(dropdown.transform.Find("Dropdown List").gameObject);
    }

    public void Display(GameObject areaObject)
    {
        this.areaObject = areaObject;
        dropdown.ClearOptions();
        dropdown.AddOptions(dropOptions);
        dropdown.value = 0;
        var x = Input.mousePosition.x;
        var y = Input.mousePosition.y;
        dropdown.transform.position = new Vector3(x+50, y, 0);
        dropdown.gameObject.SetActive(true);

        dropdownRectClosed = new Rect(x, y, 160, 30);
        dropdownRectClosed = new Rect(x, y, 160, 90);
    }

   
}