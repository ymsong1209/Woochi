using Newtonsoft.Json;
using UnityEngine;

public static class DataCloud
{
    public static bool dontSave = false;        // �������� ���� ��  

    public static PlayerData playerData;        // �÷��̾� ������
    
    public static void SavePlayerData()
    {
        string json = JsonConvert.SerializeObject(playerData, Formatting.Indented,
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

        PlayerPrefs.SetString("PlayerData", json);
        PlayerPrefs.Save();
    }

    public static void LoadPlayerData()
    {
        if (PlayerPrefs.HasKey("PlayerData"))
        {
            string playerDataJson = PlayerPrefs.GetString("PlayerData");

            playerData = JsonConvert.DeserializeObject<PlayerData>(playerDataJson);
        }
        else
        {
            playerData = new PlayerData();
        }
    }

    public static void ResetPlayerData()
    {
        playerData.ResetData();
    }

}