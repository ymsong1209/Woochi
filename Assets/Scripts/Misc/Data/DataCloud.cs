using Newtonsoft.Json;
using UnityEngine;

public static class DataCloud
{
    public static PlayerData playerData;            // 플레이어 데이터
    public static GameSettingData gameSettingData;  // 게임 설정 데이터
    
    public static string[] allyRankNames = new string[] { "기", "귀", "혼", "령", "신"};
    public static string[] woochiRankNames = new string[] { "일류", "절정", "삼화취정", "오기조원", "조화경"};
    
    public static int countForRessurection = 4;      // 부활하려면 방문해야 하는 지점 수

    public static bool IsFocusing = false;
    public static bool IsScenarioMode = false;
    
    private static int[] normalExps = new int[] { 300, 360, 432, 518, 622, 746, 895, 1074, 1289, 1547 };
    private static int[] eliteExps = new int[] { 700, 760, 832, 918, 1022, 1246, 1395, 1574, 1789, 1947 };

    public static void SavePlayerData()
    {
        string json = JsonConvert.SerializeObject(playerData, Formatting.Indented,
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        PlayerPrefs.SetString("PlayerData", json);
        PlayerPrefs.Save();
    }
    
    public static void SaveGameSetting()
    {
        string json = JsonConvert.SerializeObject(gameSettingData, Formatting.Indented,
            new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        PlayerPrefs.SetString("GameSettingData", json);
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
    
    public static void LoadGameSetting()
    {
        if (PlayerPrefs.HasKey("GameSettingData"))
        {
            string gameSettingDataJson = PlayerPrefs.GetString("GameSettingData");
            gameSettingData = JsonConvert.DeserializeObject<GameSettingData>(gameSettingDataJson);
        }
        else
        {
            gameSettingData = new GameSettingData();
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
    
    public static int GetExp(int grade, bool isElite)
    {
        return isElite ? eliteExps[grade] : normalExps[grade];
    }
}
