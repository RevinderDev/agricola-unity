using System.Diagnostics;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

/*
 * Manages informationn window. Information can be displayed after Display function call 
 * and will be automatically hiden after some amount of time (2000 ms), but only when
 * mouse is NOT over informationObject.
 */
public class Information
{
    private GameObject informationObject;
    private Text text;
    private int displayingTime = 2000;
    private Stopwatch actionStopwatch = Stopwatch.StartNew();
    public bool hide = false;
    
    
    public Information(GameObject informationObject, Text text)
    {
        this.informationObject = informationObject;
        this.text = text;
        informationObject.AddComponent<HideController>();
    }

    public void Display(string newInfoText)
    {
        informationObject.SetActive(true);
        text.text = newInfoText;

        (new Thread(() => {
            actionStopwatch.Reset();
            actionStopwatch.Start();
            while (displayingTime > actionStopwatch.ElapsedMilliseconds)
                ;
            actionStopwatch.Stop();
            hide = true;
        })).Start();
    }

    public void Hide()
    {
        if (informationObject.GetComponent<HideController>().canBeHiden)
        {
            informationObject.SetActive(false);
            hide = false;
        }
    }
}