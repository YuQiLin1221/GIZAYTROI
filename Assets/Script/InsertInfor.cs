using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class InsertInfor : MonoBehaviour
{
    public TMP_InputField playerNameField;
    public TextMeshProUGUI Scoretext, Tokentext;
    public TextMeshProUGUI resultText;
    private const string playerInfoFilePath = @"C:\Users\PC\Documents\GitHub\S.A\Assets\Resources\PlayerInfo.txt";

    void Start()
    {
        // Load player name and score from file and PlayerPrefs
        LoadPlayerInfo();
    }

    void LoadPlayerInfo()
    {
        int score = PlayerPrefs.GetInt("Score", 0);

        if (File.Exists(playerInfoFilePath))
        {
            string[] lines = File.ReadAllLines(playerInfoFilePath);
            var playerLine = lines.FirstOrDefault(line => line.Contains($"User: {PlayerPrefs.GetString("user")}"));
            if (playerLine != null)
            {
                var parts = playerLine.Split(',').Select(part => part.Trim()).ToArray();
                string playerName = ExtractValue(parts[3]);

                playerNameField.text = playerName;
                PlayerPrefs.SetString("token", ExtractValue(parts[1])); // Assuming the token is in part[1]

                // Update UI elements with the loaded values
                Scoretext.text = "Score: " + score;
                Tokentext.text = "Token: " + PlayerPrefs.GetString("token");
            }
        }
    }

    public void SendDataButton()
    {
        StartCoroutine(SendHighscoreData());
    }

    IEnumerator SendHighscoreData()
    {
        string token = PlayerPrefs.GetString("token");
        string playerName = PlayerPrefs.GetString("playerName");
        string score = PlayerPrefs.GetString("score");

        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(playerName) || string.IsNullOrEmpty(score))
        {
            resultText.text = "Thông tin không đầy đủ!";
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("token", token);
        form.AddField("playerName", playerName);
        form.AddField("score", score);

        UnityWebRequest request = UnityWebRequest.Post("https://fpl.expvn.com/InsertHighscore.php", form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            resultText.text = "Lỗi...: Không kết nối được đến server.";
        }
        else
        {
            string response = request.downloadHandler.text;
            if (response.Contains("Bạn cần đăng nhập để thực hiện thao tác này"))
            {
                resultText.text = "Bạn cần đăng nhập lại.";
                // Call login again and resend the data
                // You might need to implement login logic here to automatically resend
                // For demonstration, I will call a login function, assuming it handles this
                // StartCoroutine(LoginAndResendData());
            }
            else if (response.Contains("Done"))
            {
                resultText.text = "Dữ liệu đã được lưu thành công.";
            }
            else
            {
                resultText.text = "Lỗi không xác định từ server.";
            }
        }
    }

    string ExtractValue(string part)
    {
        int colonIndex = part.IndexOf(':') + 1;
        return part.Substring(colonIndex).Trim();
    }
}