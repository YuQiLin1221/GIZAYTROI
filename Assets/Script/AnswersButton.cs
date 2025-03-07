using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AnswersButton : MonoBehaviour
{
    public Text answerText;
    public Button BtnComp;
    public void SetAnswerText(string content)
    {
        if (answerText)
        {
            answerText.text = content;
        }
    }



}
