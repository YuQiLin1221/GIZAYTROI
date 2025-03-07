using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DiaLog : MonoBehaviour
{
    public TextMeshProUGUI dialogContentText;
    public void Show(bool show)
    {
        gameObject.SetActive(show);
    }

    public void SetDialogContent(string content)
    {
        if (dialogContentText)
        {
            dialogContentText.text = content;
        }
    }

}
