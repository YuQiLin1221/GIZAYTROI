using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SCENE : MonoBehaviour
{
    public GameObject dangnhap;
    // Các Canvas
    public Canvas sceneMainCanvas;       // Canvas chính chứa các Button
    public Canvas[] canvases;            // Mảng các Canvas phụ

    // Các Button trên Canvas chính
    public Button[] mainButtons;         // Mảng các Button trên Canvas chính

    // Các Button "Back" trên các Canvas phụ
    public Button[] backButtons;         // Mảng các Button "Back" trên các Canvas phụ

    // Button "Map"
    public Button mapButton;             // Button "Map" để chuyển scene

    // Button "Start" để chuyển đến scene đang chơi dở
    public Button startButton;           // Button "Start"
    public TextMeshProUGUI Thongbaotxt;

    private void Start()
    {
        dangnhap.SetActive(false);

        // Thiết lập trạng thái ban đầu
        SetCanvasActive(sceneMainCanvas);

        // Đăng ký các sự kiện click cho các Button trên Canvas chính
        for (int i = 0; i < mainButtons.Length; i++)
        {
            int index = i; // Để tránh vấn đề với biến vòng lặp
            mainButtons[i].onClick.AddListener(() => SwitchCanvas(canvases[index]));
        }

        // Đăng ký các sự kiện click cho các Button "Back"
        foreach (var backButton in backButtons)
        {
            backButton.onClick.AddListener(ReturnToSceneMain);
        }

        // Đăng ký sự kiện click cho Button "Map"
        if (mapButton != null)
        {
            mapButton.onClick.AddListener(OnMapButtonClick);
        }

        // Đăng ký sự kiện click cho Button "Start"
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClick);
        }
    }

    private void SwitchCanvas(Canvas newCanvas)
    {
        // Ẩn canvas chính và tất cả các canvas khác
        SetCanvasActive(null);

        // Hiển thị canvas mới
        SetCanvasActive(newCanvas);
    }

    private void ReturnToSceneMain()
    {
        // Hiển thị canvas chính và ẩn tất cả các canvas khác
        SetCanvasActive(sceneMainCanvas);
    }

    private void SetCanvasActive(Canvas canvasToActivate)
    {
        // Ẩn tất cả các canvas
        foreach (var canvas in canvases)
        {
            canvas.gameObject.SetActive(false);
        }

        // Hiển thị canvas cần kích hoạt
        if (canvasToActivate != null)
        {
            canvasToActivate.gameObject.SetActive(true);
        }
    }

    private void OnMapButtonClick()
    {
        // Chuyển đến scene "Map"
        SceneManager.LoadScene("00_Level Selection");
    }
    private void OnStartButtonClick()
    {
        // Đọc tên scene đang chơi dở từ PlayerPrefs và chuyển đến scene đó
        string currentScene = PlayerPrefs.GetString("currentScene", "DefaultSceneName");

        // Kiểm tra nếu sceneName không phải là DefaultSceneName (bạn có thể đặt một giá trị mặc định khác hoặc kiểm tra kỹ hơn)
        if (currentScene != "DefaultSceneName")
        {
            SceneManager.LoadScene(currentScene);
        }
        else
        {
            // Nếu không có scene đang chơi dở, bạn có thể chọn một hành động mặc định hoặc thông báo người dùng
            Thongbaotxt.text = ("No saved scene found, loading default scene.");
            
        }
    }

    // Ghi nhớ scene đang chơi dở
    public void SaveCurrentScene(string sceneName)
    {
        PlayerPrefs.SetString("currentScene", sceneName);
        PlayerPrefs.Save(); // Đảm bảo thông tin được lưu vào ổ đĩa
    }
}