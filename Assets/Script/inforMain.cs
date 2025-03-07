using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class inforMain : MonoBehaviour
{
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI scoreText;
    public Text tokenText;
    public TextMeshProUGUI email;

    private const string playerInfoFilePath = @"C:\Users\PC\Documents\GitHub\S.A\Assets\Resources\PlayerInfo.txt";

    // Start is called before the first frame update
    void Start()
    {
        DisplayPlayerInfo();
    }

    void DisplayPlayerInfo()
    {
        string userName = PlayerPrefs.GetString("user", "UnknownUser");
        int score = PlayerPrefs.GetInt("Score", 0);


        if (File.Exists(playerInfoFilePath))
        {
            string[] lines = File.ReadAllLines(playerInfoFilePath);

            foreach (string line in lines)
            {
                if (line.Contains($"User: {userName}"))
                {
                    // Extract PlayerName, Token, and Email from the line
                    string playerName = ExtractValue(line, "Name");
                    string token = ExtractValue(line, "Token");
                    string mail = ExtractValue(line, "Mail");

                    // Update UI text fields
                    playerNameText.text = $"Player Name: {playerName}";
                    scoreText.text = $"Score: {score}";
                    tokenText.text = $"Token: {token}";
                    email.text = $"Mail: {mail}";
                    return;
                }
            }

            // If user not found in the file
            playerNameText.text = "Player not found.";
            scoreText.text = "NULL";
            tokenText.text = "NULL";
            email.text = "NULL";
        }
        else
        {
            Debug.LogError("File not found: " + playerInfoFilePath);
        }
    }

    string ExtractValue(string line, string key)
    {
        // Find the key in the line and extract the value
        int keyIndex = line.IndexOf(key + ": ");
        if (keyIndex >= 0)
        {
            int valueStartIndex = keyIndex + (key.Length + 2);
            int valueEndIndex = line.IndexOf(", ", valueStartIndex);
            if (valueEndIndex == -1)
            {
                valueEndIndex = line.Length;
            }
            return line.Substring(valueStartIndex, valueEndIndex - valueStartIndex);
        }
        return "N/A";
    }
}