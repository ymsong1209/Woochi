using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class DataCloud
{
    public static bool dontSave = false;        // 저장하지 않을 때  

    public static PlayerData playerData;        // 플레이어 데이터
    
    public static string[] allyRankNames = new string[] { "기", "귀", "혼", "령", "신"};
    public static string[] woochiRankNames = new string[] { "일류", "절정", "삼화취정", "오기조원", "조화경"};
    
    public static int countForRessurection = 4;      // 부활하려면 방문해야 하는 지점 수

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
        SavePlayerData();
    }

    public static void DeletePlayerData()
    {
        if(PlayerPrefs.HasKey("PlayerData"))
            PlayerPrefs.DeleteKey("PlayerData");

        playerData = null;
    }
}
