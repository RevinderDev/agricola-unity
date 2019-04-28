using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionScript : EventTrigger
{
    override public void OnPointerEnter(PointerEventData data)
    {
        GetComponent<Image>().color = Color.gray;
    }
    override public void OnPointerExit(PointerEventData data)
    {
        GetComponent<Image>().color = Color.white;
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        GetComponent<Image>().color = Color.white;
        FindObjectOfType<DropdownSelect>().OnOptionSelected(transform.GetChild(0).GetComponent<Text>().text);
    }
}
