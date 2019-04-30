using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalFoodWindow : MonoBehaviour
{
    // Start is called before the first frame update


    GameObject windowObject;
    Button finishButton;
    Button convertFoodButton;

    void Start()
    {
        windowObject = GameObject.Find("AnimalFood");
        finishButton = GameObject.Find("FinishFoodButton").GetComponent<Button>();
        convertFoodButton = GameObject.Find("ConvertFoodButton").GetComponent<Button>();


        initButtonListeners();
    }



    public void initButtonListeners()
    {
        finishButton.onClick.AddListener(Hide);
        convertFoodButton.onClick.AddListener(convertFood);
    }
    

    public void convertFood()
    {

    }

    public void Hide()
    {
        windowObject.SetActive(false);
        ActionController.isActive = true;
    }


    public void Display()
    {
        windowObject.SetActive(true);
        ActionController.isActive = false;
    }
}
