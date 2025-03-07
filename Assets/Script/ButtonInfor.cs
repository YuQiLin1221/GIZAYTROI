using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonInfor : MonoBehaviour
{
    public int ItemsId;
    public Text Pricetxt;
    public Text Quantitytxt;
    public ShopManager shopManager; // Đã thay đổi kiểu thành ShopManager

    void Start()
    {
        if (shopManager == null)
        {
            shopManager = GameObject.FindObjectOfType<ShopManager>();
        }
    }

    void Update()
    {
        if (shopManager != null)
        {
            // Đảm bảo giá trị không bị lỗi
            if (ItemsId >= 1 && ItemsId <= 9)
            {
                Pricetxt.text = "Price: $" + shopManager.ShopItems[2, ItemsId].ToString();
                Quantitytxt.text = shopManager.ShopItems[3, ItemsId].ToString();
            }
        }
        else
        {
            Debug.LogWarning("ShopManager reference is missing!");
        }
    }
}