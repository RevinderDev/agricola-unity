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
            gameController.AddAction(areaObject, ActionList.plant);
            ActionController.isActive = true;
            Hide();
        }
    }

    public void Hide()
    {
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
        dropdown.transform.position = new Vector3(x, y, 0);
        dropdown.gameObject.SetActive(true);
    }
}