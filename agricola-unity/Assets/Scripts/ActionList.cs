using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionList
{
    public int quequeCurrentPosition;
    public int queueInterspace;
    public Vector2 queueElementSize;

    private int count;
    private List<Button> elementsList;
    private List<Vector3> actionPositionsList;
    private List<int> actionLenghtsList;

    GameObject mCanvas;

    public PlayerActionList()
    {
        quequeCurrentPosition = 7;
        queueInterspace = 14;
        queueElementSize = new Vector2(20, 20);
        mCanvas = GameObject.Find("Canvas");
        count = 0;
        actionPositionsList = new List<Vector3>();
        elementsList = new List<Button>();
        actionLenghtsList = new List<int>();
    }

    public void Add(Vector3 actionPosition, int actionLength)
    {
        actionPositionsList.Add(actionPosition);
        elementsList.Add(CreateButton());
        actionLenghtsList.Add(actionLength);
        count++;
    }

    public Button Remove(int i)
    {
        Button buttonToBeDestroyed = elementsList[i];
        elementsList.RemoveAt(i);
        actionPositionsList.RemoveAt(i);
        actionLenghtsList.RemoveAt(i);
        for (int j = i; j < elementsList.Count; j++)
        {
            int move = queueInterspace / 2 + (int)queueElementSize.x;
            RectTransform rectTransform = elementsList[j].GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(rectTransform.localPosition.x - move, rectTransform.localPosition.y, 0);
        }
        count--;
        return buttonToBeDestroyed;
    }

    public Button Remove(Button button)
    {
        int i = elementsList.IndexOf(button);
        return Remove(i);
    }

    public Vector3 GetDestination()
    {
        return actionPositionsList[0];
    }

    public int GetActionLength()
    {
        return actionLenghtsList[0];
    }

    public Button CreateButton()
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
        Color newColor = new Color(Random.value, Random.value, Random.value, 1.0f);
        button.GetComponent<Image>().color = newColor;

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