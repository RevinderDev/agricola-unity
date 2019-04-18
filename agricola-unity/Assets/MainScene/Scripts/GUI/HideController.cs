using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HideController : EventTrigger
{
    public bool canBeHiden = true;

    override public void OnPointerEnter(PointerEventData data)
    {
        canBeHiden = false;
    }
    override public void OnPointerExit(PointerEventData data)
    {
        canBeHiden = true;
    }
}

