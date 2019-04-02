using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
* Stores positions (where actions should be performed), lengths of those actions
* (how long player stays there) and buttons (they represent actions and creates queque) 
*/
public class PlayerActionList
{
    /* 
     * There is probably no point in creating "Action" class
     * it would cause some problems e.g. removing by button.
     */

    private int count;
    private List<Button> buttons;
    private List<Vector3> positions;
    private List<int> lengths;
    // Add e.g enum with actionType (to know which action shuould by performed)

    public int quequeCurrentPosition;
    public int queueInterspace;
    public Vector2 queueElementSize;

    GameObject mCanvas;

    public PlayerActionList()
    {
        quequeCurrentPosition = 7;
        queueInterspace = 14;
        queueElementSize = new Vector2(20, 20);
        mCanvas = GameObject.Find("Canvas");
        count = 0;
        positions = new List<Vector3>();
        buttons = new List<Button>();
        lengths = new List<int>();
    }

    public void Add(Vector3 actionPosition, int actionLength, Color color)
    {
        positions.Add(actionPosition);
        buttons.Add(CreateButton(color));
        lengths.Add(actionLength);
        count++;
    }

    public Button Remove(int i)
    {
        Button buttonToBeDestroyed = buttons[i];
        buttons.RemoveAt(i);
        positions.RemoveAt(i);
        lengths.RemoveAt(i);
        for (int j = i; j < buttons.Count; j++)
        {
            int move = queueInterspace / 2 + (int)queueElementSize.x;
            RectTransform rectTransform = buttons[j].GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(rectTransform.localPosition.x - move, rectTransform.localPosition.y, 0);
        }
        count--;
        return buttonToBeDestroyed;
    }

    public Button Remove(Button button)
    {
        int i = buttons.IndexOf(button);
        return Remove(i);
    }

    public Vector3 GetDestination()
    {
        return positions[0];
    }

    public int GetActionLength()
    {
        return lengths[0];
    }

    public Button CreateButton(Color color)
    {
        GameObject gameObject = new GameObject();
        gameObject.AddComponent<CanvasRenderer>();
        gameObject.AddComponent<RectTransform>();
        gameObject.layer = 5;

        Button button = gameObject.AddComponent<Button>();
        Image image = gameObject.AddComponent<Image>();
        button.targetGraphic = image;
        gameObject.transform.SetParent(mCanvas.transform);
        //Random color test only
        //Color newColor = new Color(Random.value, Random.value, Random.value, 1.0f);
        button.GetComponent<Image>().color = color;

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(quequeCurrentPosition + queueInterspace / 2, -queueInterspace, 0);
        rectTransform.sizeDelta = queueElementSize;
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);

        quequeCurrentPosition += queueInterspace / 2 + (int)queueElementSize.x;

        button.onClick.AddListener(delegate {
            Remove(button);
            GameController.RemoveGameObject(button.gameObject);
        });
        return button;
    }

    public int Count()
    {
        return count;
    }
}