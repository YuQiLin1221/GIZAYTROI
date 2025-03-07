using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text starsText;
    public Button backButton;

    private void Start()
    {
        // Đăng ký sự kiện click cho nút "Back"
        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackButtonClick);
        }
    }

    private void Update()
    {
        UpdateStarsUI();//TODO Not put inside the Update method later
    }

    public void UpdateStarsUI()
    {
        int sum = 0;

        for(int i = 1; i < 14; i++)
        {
            sum += PlayerPrefs.GetInt("Lv" + i.ToString());//Add the level 1 stars number, level 2 stars number.....
        }

        starsText.text = sum + "/" + 39;
    }

    private void OnBackButtonClick()
    {
        // Chuyển đến scene "canva"
        SceneManager.LoadScene("Main");

    }
}
