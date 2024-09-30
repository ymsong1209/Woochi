public class PlayerData
{
    // bool
    public bool hasSaveData;    // 저장된 데이터가 있는지

    // Battle
    public BattleData battleData;

    // Map Data
    public Map currentMap;
    
    // Skill Data
    public int[] currentskillIDs;
    public int[,] totalSkillIDs;

    // Default
    public int gold = 0;

    public PlayerData()
    {
        ResetData();
    }

    public void ResetData()
    {
        hasSaveData = false;

        battleData = new BattleData();
        currentMap = null;
        currentskillIDs = new int[5];
        currentskillIDs[0] = 1001;
        currentskillIDs[1] = 2002;
        currentskillIDs[2] = 3001;
        currentskillIDs[3] = 4001;
        currentskillIDs[4] = 5001;
        
        totalSkillIDs = new int[5, 5];
        totalSkillIDs[0, 0] = 1001;
        totalSkillIDs[1, 0] = 2002;
        totalSkillIDs[2, 0] = 3001;
        totalSkillIDs[3, 0] = 4001;
        totalSkillIDs[4, 0] = 5001;

        gold = 0;
    }

    public CharacterInfoData LoadInfo(int ID)
    {
        foreach(var info in battleData.characterInfoList)
        {
            if (info.ID == ID)
            {
                return info;
            }
        }

        return null;
    }

    public void SaveInfo(CharacterInfoData newCharacterInfo)
    {
        foreach (var info in battleData.characterInfoList)
        {
            if (info.ID == newCharacterInfo.ID)
            {
                info.baseStat = new Stat(newCharacterInfo.baseStat);
                info.health = newCharacterInfo.health;
                info.level = newCharacterInfo.level;
                return;
            }
        }

        battleData.characterInfoList.Add(newCharacterInfo);
    }
}
