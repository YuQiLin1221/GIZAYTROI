using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManagerLv : MonoBehaviour
{
    public static UiManagerLv instance;
    public Text timeText;
    public Text questionText;
    public DiaLog DiaLog;

    public AnswersButton[] answersButtons;

    private void Awake()
    {
        MakeSingleton();
    }


    public void SetTimeText(string content)
    {
        if (timeText)
        {
            timeText.text = content;
        }
    }

    public void SetQuestionText(string content)
    {
        if (questionText)
        {
            questionText.text = content;
        }
    }

    public void ShuffleAnswers()
    {
        if (answersButtons != null && answersButtons.Length > 0)
        {
            for (int i = 0; i < answersButtons.Length; i++)
            {
                if (answersButtons[i])
                {
                    answersButtons[i].tag = "Untagged";
                }
            }

            int ranIdx = Random.Range(0, answersButtons.Length);

            if (answersButtons[ranIdx])
            {
                answersButtons[ranIdx].tag = "RightAnsw";
            }
        }
    }
    public void MakeSingleton()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
