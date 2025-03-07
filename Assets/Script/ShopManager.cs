using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public int[,] ShopItems = new int[10,10];
    public float gold;
    public Text CoinTxt;

    void Start()
    {
        gold = ReadGoldFromFile();
        CoinTxt.text = "Gold: " + gold.ToString();

        //Id's
        ShopItems[1, 1] = 1;
        ShopItems[1, 2] = 2;
        ShopItems[1, 3] = 3;
        ShopItems[1, 4] = 4;
        ShopItems[1, 5] = 5;
        ShopItems[1, 6] = 6;
        ShopItems[1, 7] = 7;
        ShopItems[1, 8] = 8;
        ShopItems[1, 9] = 9;

        //price
        ShopItems[2, 1] = 10;
        ShopItems[2, 2] = 20;
        ShopItems[2, 3] = 30;
        ShopItems[2, 4] = 40;
        ShopItems[2, 5] = 50;
        ShopItems[2, 6] = 60;
        ShopItems[2, 7] = 70;
        ShopItems[2, 8] = 80;
        ShopItems[2, 9] = 90;

        //quanlity
        ShopItems[3, 1] = 0;
        ShopItems[3, 2] = 0;
        ShopItems[3, 3] = 0;
        ShopItems[3, 4] = 0;
        ShopItems[3, 5] = 0;
        ShopItems[3, 6] = 0;
        ShopItems[3, 7] = 0;
        ShopItems[3, 8] = 0;
        ShopItems[3, 9] = 0;
    }

    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        ButtonInfor buttonInfo = ButtonRef.GetComponent<ButtonInfor>();

        if (buttonInfo != null)
        {
            int itemId = buttonInfo.ItemsId;
            float itemPrice = ShopItems[2, itemId];

            if (gold >= itemPrice)
            {
                gold -= itemPrice;
                ShopItems[3, itemId]++;
                CoinTxt.text = "gold: " + gold.ToString();
                buttonInfo.Quantitytxt.text = ShopItems[3, itemId].ToString();
            }
            else
            {
                Debug.LogWarning("Not enough gold to buy this item.");
            }
        }
        else
        {
            Debug.LogWarning("ButtonInfor component not found on the selected button.");
        }
    }

    float ReadGoldFromFile()
    {
        string filePath = @"C:\Users\PC\Documents\GitHub\S.A\Assets\Resources\PlayerInfo.txt"; // Đường dẫn tới file của bạn
        float gold = 0f;

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                // Tìm giá trị gold trong file
                Match match = Regex.Match(line, @"Gold:\s*(\d+)");
                if (match.Success)
                {
                    gold = float.Parse(match.Groups[1].Value);
                    break;
                }
            }
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
        }

        return gold;
    }

    void UpdateGoldInFile(float newGoldAmount)
    {
        string filePath = @"C:\Users\PC\Documents\GitHub\S.A\Assets\Resources\PlayerInfo.txt"; // Đường dẫn tới file của bạn

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            List<string> updatedLines = new List<string>();

            foreach (string line in lines)
            {
                // Cập nhật giá trị gold trong file
                if (line.StartsWith("Gold:"))
                {
                    updatedLines.Add($"Gold: {newGoldAmount}");
                }
                else
                {
                    updatedLines.Add(line);
                }
            }

            // Nếu không tìm thấy dòng gold, thêm mới
            if (!lines.Any(line => line.StartsWith("Gold:")))
            {
                updatedLines.Add($"Gold: {newGoldAmount}");
            }

            File.WriteAllLines(filePath, updatedLines);
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
        }
    }
}