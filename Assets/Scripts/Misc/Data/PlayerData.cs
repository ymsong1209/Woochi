using System.Collections.Generic;

public class PlayerData
{
    // bool
    public bool hasSaveData;    // 저장된 데이터가 있는지

    // Battle
    public BattleData battleData;

    // Map Data
    public Map currentMap;

    // Woochi
    public int maxSorceryPoints;    // 최대 도술 포인트
    public int sorceryPoints;       // 현재 도술 포인트 
    public List<Luck> luckList;     // 행운

    // Skill Data
    public int[] currentskillIDs;
    public int[,] totalSkillIDs;
    public int realization;     // 깨달음

    // Default
    public int gold = 0;
    public int scenarioID = 0;      // 현재 진행중인 시나리오 ID
    
    // Game State
    public bool isFirstPlay = true;
    
    public PlayerData()
    {
        ResetData();
    }

    public void ResetData()
    {
        hasSaveData = false;

        battleData = new BattleData(isFirstPlay);
        currentMap = null;

        maxSorceryPoints = 200;
        sorceryPoints = 200;
        luckList = new List<Luck>();

        currentskillIDs = new int[5];
        if(GameManager.GetInstance.UseDebugSkills)
        {
            currentskillIDs = GameManager.GetInstance.Skills;
        }
        else
        {
            currentskillIDs[0] = 1101;
            currentskillIDs[1] = 1201;
            currentskillIDs[2] = 1301;
            currentskillIDs[3] = 1401;
            currentskillIDs[4] = 1501;
        }
        
        //TODO : 도술 두루마리 세팅
        totalSkillIDs = new int[5, 5];
        totalSkillIDs[0, 0] = 1101;
        totalSkillIDs[1, 0] = 1201;
        totalSkillIDs[2, 0] = 1301;
        totalSkillIDs[3, 0] = 1401;
        totalSkillIDs[4, 0] = 1501;

        realization = 4;
        
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
                info.baseStat = newCharacterInfo.baseStat;
                info.levelUpStat = newCharacterInfo.levelUpStat;
                info.rewardStat = newCharacterInfo.rewardStat;
                info.health = newCharacterInfo.health;
                info.level = newCharacterInfo.level;
                return;
            }
        }

        battleData.characterInfoList.Add(newCharacterInfo);
    }

    public int CalculateLuck()
    {
        if(luckList == null || luckList.Count == 0)
        {
            return 0;
        }

        int luck = 0;

        for(int i = 0; i < luckList.Count; i++)
        {
            if (luckList[i].turn > 0)
            {
                luck += luckList[i].value;
            }
        }

        Luck firstLuck = luckList[0];
        firstLuck.turn--;

        if(firstLuck.turn <= 0)
        {
            luckList.RemoveAt(0);
        }
        else
        {
            luckList[0] = firstLuck;
        }

        return luck;
    }
}
