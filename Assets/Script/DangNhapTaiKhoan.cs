using System;
using System.Collections;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class DangNhapTaiKhoan : MonoBehaviour
{
    public TMP_InputField user;
    public TMP_InputField passwd;
    public TextMeshProUGUI thongbao;
    public GameObject dangnhap;
    public GameObject InforMain;
    public GameObject sceneMain; // Thêm biến cho Canvas sceneMain

    public TMP_InputField playerName;
    public TMP_InputField mail;
    public TextMeshProUGUI saveStatus; // Hiển thị thông báo lưu thông tin

    private const string playerInfoFilePath = @"C:\Users\PC\Documents\GitHub\S.A\Assets\Resources\PlayerInfo.txt";
    private const string playerNameKey = "playerName";

    private string token;
    private string savedUser;
    private string savedPasswd;

    void Start()
    {
        // Đảm bảo canvas thông tin và scene chính không hiển thị ngay từ đầu
        InforMain.SetActive(false);
        sceneMain.SetActive(false);
    }


    public void DangNhapButton()
    {
        StartCoroutine(DangNhap());
    }

    IEnumerator DangNhap()
    {
        WWWForm dataForm = new WWWForm();
        dataForm.AddField("user", user.text);
        dataForm.AddField("passwd", passwd.text);

        UnityWebRequest www = UnityWebRequest.Post("https://fpl.expvn.com/dangnhap.php", dataForm);
        yield return www.SendWebRequest();

        if (!www.isDone)
        {
            thongbao.text = "Ket noi khong thanh cong...";
        }
        else
        {
            string get = www.downloadHandler.text;

            if (get == "emty")
            {
                thongbao.text = "Vui lòng nhập đầy đủ thông tin đăng nhập";
            }
            else if (get == "" || get == null)
            {
                thongbao.text = "Tài khoản hoặc mật khẩu không chính xác";
            }
            else if (get.Contains("Lỗi"))
            {
                thongbao.text = "Không kết nối được tới server";
            }
            else
            {
                thongbao.text = "Đăng nhập thành công";

                token = get;
                savedUser = user.text;
                savedPasswd = passwd.text;

                PlayerPrefs.SetString("token", token);
                PlayerPrefs.SetString("user", savedUser);
                PlayerPrefs.SetString("passwd", savedPasswd);
                PlayerPrefs.SetString("loginTime", System.DateTime.Now.ToString());

                // Kiểm tra xem tên người chơi đã được lưu trong file chưa
                if (IsPlayerNameSaved())
                {
                    // Chuyển đến sceneMain nếu tên người chơi đã được lưu
                    SceneManager.LoadScene("Main");
                }
                else
                {
                    // Hiển thị canvas thông tin người chơi nếu chưa lưu
                    InforMain.SetActive(true);
                }
            }
        }
    }

    bool IsPlayerNameSaved()
    {
        if (File.Exists(playerInfoFilePath))
        {
            string[] lines = File.ReadAllLines(playerInfoFilePath);
            return lines.Any(line => line.Contains($"User: {savedUser}"));
        }
        return false;
    }

    public void SaveButton()
    {
        StartCoroutine(SavePlayerInfo());
    }

    IEnumerator SavePlayerInfo()
    {
        string name = playerName.text;
        string email = mail.text;
        string score = ""; // Cột score để trống
        string date = System.DateTime.Now.ToString("yyyy-MM-dd"); // Ngày hiện tại
        string gold = "2000";
        string diamond = "10";
        string kinhlup = "3";
        string keyitems = "1";

        PlayerPrefs.SetInt("gold", 2000);
        PlayerPrefs.SetInt("diamond", 10);
        PlayerPrefs.SetInt("kinhlup", 3);
        PlayerPrefs.SetInt("keyitems", 1);

        if (IsPlayerNameSaved())
        {
            thongbao.text = "Tên người chơi đã được nhập trước đó.";
            yield break;
        }

        using (StreamWriter sw = new StreamWriter(playerInfoFilePath, true))
        {
            sw.WriteLine($"User: {savedUser}, Token: {token}, Password: {savedPasswd}, Name: {name}, Date: {date}, Mail: {email}, Gold: {gold}, Diamond: {diamond}, Kinhlup: {kinhlup}, Keyitems: {keyitems}, Score: {score}");
        }

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("Main");
    }

    public void UpdatePlayerInfo(string user, int goldDelta, int diamondDelta, int kinhlupDelta, int keyitemsDelta, int scoreDelta)
    {
        if (File.Exists(playerInfoFilePath))
        {
            string[] lines = File.ReadAllLines(playerInfoFilePath);
            using (StreamWriter sw = new StreamWriter(playerInfoFilePath, false))
            {
                foreach (string line in lines)
                {
                    if (line.Contains($"User: {user}"))
                    {
                        string updatedLine = UpdateLine(line, goldDelta, diamondDelta, kinhlupDelta, keyitemsDelta, scoreDelta);
                        sw.WriteLine(updatedLine);
                    }
                    else
                    {
                        sw.WriteLine(line);
                    }
                }
            }
        }
    }

    string UpdateLine(string line, int goldDelta, int diamondDelta, int kinhlupDelta, int keyitemsDelta, int scoreDelta)
    {
        string[] parts = line.Split(',').Select(part => part.Trim()).ToArray();
        int gold = int.Parse(ExtractValue(parts[6]));
        int diamond = int.Parse(ExtractValue(parts[7]));
        int kinhlup = int.Parse(ExtractValue(parts[8]));
        int keyitems = int.Parse(ExtractValue(parts[9]));
        int score = int.Parse(ExtractValue(parts[10]));

        gold += goldDelta;
        diamond += diamondDelta;
        kinhlup += kinhlupDelta;
        keyitems += keyitemsDelta;
        score += scoreDelta;

        return $"{parts[0]}, Token: {ExtractValue(parts[1])}, Password: {ExtractValue(parts[2])}, Name: {ExtractValue(parts[3])}, Date: {ExtractValue(parts[4])}, Mail: {ExtractValue(parts[5])}, Gold: {gold}, Diamond: {diamond}, Kinhlup: {kinhlup}, Keyitems: {keyitems}, Score: {score}";
    }

    string ExtractValue(string part)
    {
        int colonIndex = part.IndexOf(':') + 1;
        return part.Substring(colonIndex).Trim();
    }

    bool IsTokenValid()
    {
        if (PlayerPrefs.HasKey("loginTime"))
        {
            DateTime loginTime = DateTime.Parse(PlayerPrefs.GetString("loginTime"));
            TimeSpan elapsed = DateTime.Now - loginTime;
            return elapsed.TotalHours < 24; // Token còn hiệu lực trong 24 giờ
        }
        return false;
    }
}