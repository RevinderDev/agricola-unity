using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class QuestionWindow
{
    private GameObject windowObject;
    private Button buttonYes;
    private Button buttonNo;
    private Text windowText;
    private bool answer;
    private bool answered;
    private bool wasAnswerChecked;
    private string questionTag;
    private bool wasQuestionAsked;

    public QuestionWindow(GameObject windowObject, Text windowText, Button buttonYes, Button buttonNo)
    {
        this.windowObject = windowObject;
        this.windowText = windowText;
        this.buttonYes = buttonYes;
        this.buttonNo = buttonNo;
        answered = false;
        wasAnswerChecked = false;
        wasQuestionAsked = true;
        buttonYes.onClick.AddListener(delegate { answer = true; answered = true; Hide(); });
        buttonNo.onClick.AddListener(delegate { answer = false; answered = true; Hide(); });
    }

    public void DisplayQuestion(string questionText, string questionTag)
    {
        windowObject.SetActive(true);
        answered = false;
        wasAnswerChecked = false;
        wasQuestionAsked = true;
        ActionController.isActive = false;
        this.questionTag = questionTag;
        windowText.text = questionText;
    }

    public void Hide()
    {
        windowObject.SetActive(false);
        ActionController.isActive = true;
    }

    public bool GetAnswer()
    {
        wasAnswerChecked = true;
        wasQuestionAsked = false;
        return answer;
    }
    
    public bool WasQuestionAsked()
    {
        return wasQuestionAsked;
    }
    
    public bool WasQuestionAnswered()
    {
        return answered;
    }

    public string GetQuestionTag()
    {
        return questionTag;
    }

}