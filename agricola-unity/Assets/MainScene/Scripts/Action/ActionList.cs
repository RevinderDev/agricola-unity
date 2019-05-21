using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/*
* Stores positions (where actions should be performed), lengths of those actions
* (how long player stays there) and buttons (they represent actions and creates queque) 
*/
public class ActionList
{
    GameController gameController;
    private int count;
    private List<Button> buttons;
    private List<GameObject> gameObjects;
    private List<ActionType> actionTypes;
    private List<ItemType> itemsTypesRequired;

    public int quequeCurrentPosition;
    public int queueInterspace;
    public Vector2 queueElementSize;

    GameObject mCanvas;

    public ActionList()
    {
        queueInterspace = 5;
        quequeCurrentPosition = queueInterspace*4;
        queueElementSize = new Vector2(35, 35);
        mCanvas = GameObject.Find("Canvas");
        count = 0;
        gameObjects = new List<GameObject>();
        buttons = new List<Button>();
        actionTypes = new List<ActionType>();
        itemsTypesRequired = new List<ItemType>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    public void Add(GameObject gameObject, ActionType type, ItemType required = null)
    {
        gameObjects.Add(gameObject);
        buttons.Add(CreateButton(type.directory));
        actionTypes.Add(type);
        itemsTypesRequired.Add(required);
        gameController.inventory.RemoveItem(required);
        quequeCurrentPosition += queueInterspace / 2 + (int)queueElementSize.x;
        count++;
        gameController.players[gameController.activePlayer].ActualizeTimeBar();
    }

    public Button Remove(int i, bool notUsed = false)
    {
        Button buttonToBeDestroyed = buttons[i];
        buttons.RemoveAt(i);
        gameObjects.RemoveAt(i);
        actionTypes.RemoveAt(i);
        ItemType type = itemsTypesRequired[i];
        if (type != null && notUsed)
        {
            gameController.inventory.AddItem(type);
        }
        itemsTypesRequired.RemoveAt(i);
        for (int j = i; j < buttons.Count; j++)
        {
            int move = queueInterspace / 2 + (int)queueElementSize.x;
            RectTransform rectTransform = buttons[j].GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(rectTransform.localPosition.x - move, rectTransform.localPosition.y, 0);
        }
        count--;
        quequeCurrentPosition -= queueInterspace / 2 + (int)queueElementSize.x;
        gameController.players[gameController.activePlayer].ActualizeTimeBar();
        return buttonToBeDestroyed;
    }

    public Button Remove(Button button)
    {
        int i = buttons.IndexOf(button);
        return Remove(i, true);
    }

    public Vector3 GetDestination()
    {
        return gameObjects[0].transform.position;
    }

    public GameObject GetGameObject()
    {
        return gameObjects[0];
    }

    public ActionType GetAction()
    {
        return actionTypes[0];
    }

    public ItemType GetItemTypeRequired()
    {
        return itemsTypesRequired[0];
    }

    public Button CreateButton(string spriteDirectory)
    {
        Sprite sprite = Resources.Load<Sprite>(spriteDirectory);

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
        imageBackground.sprite = Resources.GetBuiltinResource(typeof(Sprite), "UI/Skin/UISprite") as Sprite; //TODO fix
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
        if (spriteDirectory == null)
            button.gameObject.SetActive(false);

        return button;
    }

    public bool IsActionInQueque(GameObject gameObject, ActionType type)
    {
        for(int i = 0; i<gameObjects.Count; i++)
        {
            if (gameObjects[i].Equals(gameObject))
            {
                if (actionTypes[i].Equals(type))
                    return true;
            }
        }
        return false;
    }

    public int Count()
    {
        return count;
    }

    public int ActionsLengthsSum()
    {
        return actionTypes.Sum(item => item.length);
    }
}