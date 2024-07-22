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

        playerData.hasSaveData = true;
        PlayerPrefs.SetString("PlayerData", json);
        PlayerPrefs.Save();
    }

    public static void LoadPlayerData()
    {
        if (PlayerPrefs.HasKey("PlayerData"))
        {
            string playerDataJson = PlayerPrefs.GetString("PlayerData");
            playerData = JsonConvert.DeserializeObject<PlayerData>(playerDataJson, 
                new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace});
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

    public static void DeletePlayerData()
    {
        PlayerPrefs.DeleteKey("PlayerData");
        playerData = new PlayerData();
    }
}
