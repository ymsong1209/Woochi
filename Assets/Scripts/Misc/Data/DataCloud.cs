using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public static class DataCloud
{
    public static PlayerData playerData;        // 플레이어 데이터
    
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
            Debug.Log(playerDataJson);

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
