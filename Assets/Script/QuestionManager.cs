using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager instance;

    public QuestionData[] questions;

    List<QuestionData> m_question;

    QuestionData m_CurQuestion;

    public QuestionData CurQuestion { get => m_CurQuestion; set => m_CurQuestion = value; }

    private void Awake()
    {
        m_question = questions.ToList();
        MakeSingleton();
    }

    public QuestionData GetRamdomQuestion()
    {
        if (m_question != null && m_question.Count > 0)
        {
            int ranIdx = Random.Range(0, m_question.Count);

            m_CurQuestion = m_question[ranIdx];
            m_question.RemoveAt(ranIdx);
        }

        return m_CurQuestion;
    }

    public void MakeSingleton()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

}
