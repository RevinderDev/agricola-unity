using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{

    private static Tooltip instance;
    private Text tooltipText;
    private RectTransform backgroundRectTransform;

    public void Awake()
    {
        backgroundRectTransform = transform.Find("tooltipBackground").GetComponent<RectTransform>();
        tooltipText = transform.Find("tooltipText").GetComponent<Text>();
        instance = this;

        HideTooltip();
    }


    public static void ChangePosition(Vector2 newPosition)
    {
        instance.transform.position = newPosition;
    }

    private void Showtooltip(string tooltipString)
    {
        gameObject.SetActive(true);


        tooltipText.text = tooltipString;
        float textPadding = 4f;
        Vector2 backgroundSize = new Vector2(tooltipText.preferredWidth + textPadding * 2f, 
            tooltipText.preferredHeight + textPadding * 2f);

        backgroundRectTransform.sizeDelta = backgroundSize;
    }

    private void HideTooltip()
    {
        gameObject.SetActive(false);
    }


    public static void ShowTooltip_Static(string tooltipString)
    {
        instance.Showtooltip(tooltipString);
    }

    public static void Hidetooltip_Static()
    {
        instance.HideTooltip();
    }
}
