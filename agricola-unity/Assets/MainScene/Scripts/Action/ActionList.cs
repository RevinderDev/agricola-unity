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
    public enum ActionType
    {
        /* New action should be added in PerformAction (GameController), OnMouseDown (ActionController
         * and eventually in Farmland or other classes created (custom method)
         * */
        walk = 0,
        plant = 1,
        collectPlant = 2
    }
    /* 
     * There is probably no point in creating "Action" class
     * it would cause some problems e.g. removing by button.
     */

    private int count;
    private List<Button> buttons;
    private List<GameObject> gameObjects;
    private List<int> lengths;
    private List<ActionType> types;
    // Add e.g enum with actionType (to know which action shuould by performed)

    public int quequeCurrentPosition;
    public int queueInterspace;
    public Vector2 queueElementSize;

    GameObject mCanvas;

    public PlayerActionList()
    {
        queueInterspace = 5;
        quequeCurrentPosition = queueInterspace*4;
        queueElementSize = new Vector2(35, 35);
        mCanvas = GameObject.Find("Canvas");
        count = 0;
        gameObjects = new List<GameObject>();
        buttons = new List<Button>();
        lengths = new List<int>();
        types = new List<ActionType>();
    }

    public void Add(GameObject gameObject, ActionType type, int actionLength, string imageDirectory)
    {
        Sprite sprite = Resources.Load<Sprite>(imageDirectory);
        gameObjects.Add(gameObject);
        buttons.Add(CreateButton(sprite));
        lengths.Add(actionLength);
        types.Add(type);
        quequeCurrentPosition += queueInterspace / 2 + (int)queueElementSize.x;
        count++;
    }

    public void Add(GameObject gameObject, ActionType type, int actionLength, Color color)
    {
        gameObjects.Add(gameObject);
        buttons.Add(CreateButton(color));
        lengths.Add(actionLength);
        types.Add(type);
        quequeCurrentPosition += queueInterspace / 2 + (int)queueElementSize.x;
        count++;
    }

    public Button Remove(int i)
    {
        Button buttonToBeDestroyed = buttons[i];
        buttons.RemoveAt(i);
        gameObjects.RemoveAt(i);
        lengths.RemoveAt(i);
        types.RemoveAt(i);
        for (int j = i; j < buttons.Count; j++)
        {
            int move = queueInterspace / 2 + (int)queueElementSize.x;
            RectTransform rectTransform = buttons[j].GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(rectTransform.localPosition.x - move, rectTransform.localPosition.y, 0);
        }
        count--;
        quequeCurrentPosition -= queueInterspace / 2 + (int)queueElementSize.x;
        return buttonToBeDestroyed;
    }

    public Button Remove(Button button)
    {
        int i = buttons.IndexOf(button);
        return Remove(i);
    }

    public Vector3 GetDestination()
    {
        return gameObjects[0].transform.position;
    }

    public GameObject GetGameObject()
    {
        return gameObjects[0];
    }

    public int GetActionLength()
    {
        return lengths[0];
    }

    public ActionType GetActionType()
    {
        return types[0];
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
        rectTransform.localPosition = new Vector3(quequeCurrentPosition + queueInterspace / 2, -queueInterspace*4, 0);
        rectTransform.sizeDelta = queueElementSize;
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);

        button.onClick.AddListener(delegate {
            Remove(button);
            GameController.RemoveGameObject(button.gameObject);
        });
        return button;
    }

    public Button CreateButton(Sprite sprite)
    {
        
        GameObject gameObject = new GameObject();
        gameObject.AddComponent<CanvasRenderer>();
        gameObject.AddComponent<RectTransform>();
        gameObject.layer = 5;
        gameObject.transform.SetParent(mCanvas.transform);

        //Background
        GameObject gameObjectBackground = new GameObject();
        gameObjectBackground.AddComponent<CanvasRenderer>();
        gameObjectBackground.AddComponent<RectTransform>();
        gameObjectBackground.transform.SetParent(gameObject.transform);
        Image imageBackground = gameObjectBackground.AddComponent<Image>();
        imageBackground.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        gameObjectBackground.GetComponent<RectTransform>().sizeDelta = queueElementSize;
        //Icon
        GameObject gameObjectIcon = new GameObject();
        gameObjectIcon.AddComponent<CanvasRenderer>();
        gameObjectIcon.AddComponent<RectTransform>();
        gameObjectIcon.transform.SetParent(gameObject.transform);
        Image image = gameObjectIcon.AddComponent<Image>();
        image.sprite = sprite;
        gameObjectIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(queueElementSize.x - 5, queueElementSize.y - 5);


        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(quequeCurrentPosition + queueInterspace / 2, -queueInterspace*4, 0);
        rectTransform.sizeDelta = queueElementSize;
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);

        //Button
        Button button = gameObject.AddComponent<Button>();
        button.targetGraphic = image;
        button.onClick.AddListener(delegate {
            Remove(button);
            GameController.RemoveGameObject(button.gameObject);
        });
        return button;
    }

    public bool IsActionInQueque(GameObject gameObject, PlayerActionList.ActionType type)
    {
        for(int i = 0; i<gameObjects.Count; i++)
        {
            if (gameObjects[i].Equals(gameObject))
            {
                if (types[i].Equals(type))
                    return true;
            }
        }
        return false;
    }

    public int Count()
    {
        return count;
    }
}