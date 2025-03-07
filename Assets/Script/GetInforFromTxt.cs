using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GetInforFromTxt : MonoBehaviour
{
    public Text Name;
    public Text Gold;
    public Text Diamond;
    public Text bd;
    public Text keyItem;

    private const string playerInfoFilePath = @"C:\Users\PC\Documents\GitHub\S.A\Assets\Resources\PlayerInfo.txt";

    // Start is called before the first frame update
    void Start()
    {
        DisplaySceneMain();
    }

    void DisplaySceneMain()
    {
        if (File.Exists(playerInfoFilePath))
        {
            string[] lines = File.ReadAllLines(playerInfoFilePath);

            foreach (string line in lines)
            {
                if (line.Contains($"User: {PlayerPrefs.GetString("user")}"))
                {
                    // Extract PlayerName, Score, and Token from the line
                    string playerName = ExtractValue(line, "Name");
                    string gold = ExtractValue(line, "Gold");
                    string diamd = ExtractValue(line, "Diamond");
                    string bogd = ExtractValue(line, "Kinhlup");
                    string key = ExtractValue(line, "Keyitems");

                    // Update UI text fields
                    Name.text = $"{playerName}";
                    Gold.text = $"{gold}";
                    Diamond.text = $"{diamd}";
                    bd.text = $"{bogd}";
                    keyItem.text = $"{key}";
                    return;
                }
            }

            // If user not found in the file
            Name.text = "Null";
            Gold.text = "00000";
            Diamond.text = "0";
            bd.text = "0";
            keyItem.text = "0" ;
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