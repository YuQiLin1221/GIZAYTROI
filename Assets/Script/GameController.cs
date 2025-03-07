using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameController : MonoBehaviour
{
    public float timePerQuestion;
    float m_curTime;

    int m_rightCount;
    int m_diem;

    public GameObject st0, st1, st2, st3;

    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        m_curTime = timePerQuestion;
        m_diem = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        UiManagerLv.instance.SetTimeText("00: " + m_curTime);
        CreateQuestion();
        StartCoroutine(TimecountingDown());
    }


    public void CreateQuestion()
    {
        QuestionData qs = QuestionManager.instance.GetRamdomQuestion();
        if (qs != null)
        {
            UiManagerLv.instance.SetQuestionText(qs.question);

            string[] wrongAnswers = new string[] { qs.answerA, qs.answerB, qs.answerC };
            UiManagerLv.instance.ShuffleAnswers();
            var temp = UiManagerLv.instance.answersButtons;

            if (temp != null && temp.Length > 0)
            {
                int wrongAnswerCount = 0;
                for (int i = 0; i < temp.Length; i++)
                {
                    int answerId = i;

                    if (string.Compare(temp[i].tag, "RightAnsw") == 0)
                    {
                        temp[i].SetAnswerText(qs.rightAnswer);
                    }
                    else
                    {
                        temp[i].SetAnswerText(wrongAnswers[wrongAnswerCount]);
                        wrongAnswerCount++;
                    }

                    temp[answerId].BtnComp.onClick.RemoveAllListeners();
                    temp[answerId].BtnComp.onClick.AddListener(() => CheckRightAnswerEvent(temp[answerId]));
                }
            }
        }

    }

    void CheckRightAnswerEvent(AnswersButton answerButton)
    {
        if (answerButton == null)
        {
            Debug.LogError("AnswerButton is null.");
            return;
        }

        if (answerButton.CompareTag("RightAnsw"))
        {
            if (m_curTime >= 5)
            {
                m_diem += 2; // Cộng 2 điểm nếu thời gian còn >= 5 giây
            }
            else
            {
                m_diem += 1; // Cộng 1 điểm nếu thời gian còn ít hơn 5 giây
            }

            m_curTime = timePerQuestion;
            UiManagerLv.instance.SetTimeText("00 : " + m_curTime);
            m_rightCount++;
            if (m_rightCount == QuestionManager.instance.questions.Length)
            {
                UiManagerLv.instance.DiaLog.SetDialogContent("You Win!!!\ndiem: " + m_diem);
                UiManagerLv.instance.DiaLog.Show(true);
                ShowStarsBasedOnScore();
                Savediem();
                StopAllCoroutines();
            }
            else
            {
                CreateQuestion();
                Debug.Log("Great!");
            }
        }
        else
        {
            UiManagerLv.instance.DiaLog.SetDialogContent("You Lose.\ndiem: " + m_diem);
            UiManagerLv.instance.DiaLog.Show(true);
            Debug.Log("Ban da tra loi sai!");
            ShowStarsBasedOnScore();
            Savediem();
            StopAllCoroutines();
        }
    }

    IEnumerator TimecountingDown()
    {
        yield return new WaitForSeconds(1);
        if (m_curTime > 0)
        {
            m_curTime--;
            StartCoroutine(TimecountingDown());
            UiManagerLv.instance.SetTimeText("00 : " + m_curTime);
        }
        else
        {
            UiManagerLv.instance.DiaLog.SetDialogContent("Time is up! YOU LOSE.\ndiem: " + m_diem);
            UiManagerLv.instance.DiaLog.Show(true);
            Savediem();
            StopAllCoroutines();
        }
    }

    void ShowStarsBasedOnScore()
    {

        // Hiển thị sao dựa trên điểm số
        if (m_diem >= 0 && m_diem < 2)
        {
            st0.SetActive(true);
            Debug.Log("da hien thi");
        }
        else if (m_diem >= 2 && m_diem < 4)
        {
            st1.SetActive(true);
            Debug.Log("da hien thi");
        }
        else if (m_diem >= 4 && m_diem < 7)
        {
            st2.SetActive(true);
            Debug.Log("da hien thi");
        }
        else if (m_diem >= 7)
        {
            st3.SetActive(true);
            Debug.Log("da hien thi");
        }
        else
        {
            Debug.Log("loi");
        }
    }

    void Savediem()
    {
        PlayerPrefs.SetInt("diem", m_diem); // Lưu điểm số vào PlayerPrefs
        PlayerPrefs.Save(); // Đảm bảo điểm số được lưu vào ổ đĩa
    }

    public void Replay()
    {
        m_diem = 0; // Reset điểm số khi replay
        SceneManager.LoadScene("01_Level 01");
        Time.timeScale = 1.0f;
    }
    public void ExitBtn()
    {
        SceneManager.LoadScene("Main");
    }
}
