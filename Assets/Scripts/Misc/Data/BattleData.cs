using System.Collections.Generic;
using UnityEngine.Serialization;

[System.Serializable]
public class BattleData
{
    public List<int> allies;    // 플레이어(우치)가 소유하고 있는 소환수(우치 자기자신도 포함)
    public int[] formation;     // 전투에 참여할 포메이션(-1은 빈자리)
    public List<int> charms;   // 우치가 소유하고 있는 부적(5개)

    public List<CharacterInfoData> characterInfoList;
    
    public BattleData(bool isTutorial)
    {
        if (isTutorial)
        {
            allies = new List<int>() { 0 };
            formation = new int[] { 0, -1, -1, -1 };   
        }
        else
        {
            allies = new List<int>() { 0, 1, 2 };
            formation = new int[] { 2, 0, 1, -1};   // 호랑이-우치-삼미호 순(시연버전)
        }
        charms = new List<int>(5);
        //원기회복부(힐부적) 3장 들고 시작
        charms.Insert(0, 9);
        charms.Insert(1, 9);
        charms.Insert(2, 9);
        if (GameManager.GetInstance.UseDebugCharms)
        {
            charms = GameManager.GetInstance.Charms;
        }
        characterInfoList = new List<CharacterInfoData>();
    }

    public void SetFormation(int[] newFormation)
    {
        for(int i = 0; i < formation.Length; i++)
        {
            formation[i] = newFormation[i];
        }
    }
    
    public void AddAlly(int ID)
    {
        allies.Add(ID);
    }
}

[System.Serializable]
public class CharacterInfoData
{
    public int ID;
    public Stat baseStat;
    public Stat levelUpStat;
    public Stat rewardStat;
    public Level level;
    public Health health;

    public CharacterInfoData()
    {
        ID = -1;
        baseStat = new Stat();
        levelUpStat = new Stat();
        rewardStat = new Stat();
        level = new Level();
        health = null;
    }

    public CharacterInfoData(int ID, Stat baseStat, Stat levelUpStat, Stat rewardStat, Level level, Health health)
    {
        this.ID = ID;
        this.baseStat = new Stat(baseStat);
        this.levelUpStat = new Stat(levelUpStat);
        this.rewardStat = new Stat(rewardStat);
        this.level = new Level(level);
        this.health = new Health(health);
    }
}
