using System.Collections.Generic;

[System.Serializable]
public class BattleData
{
    public List<int> allies;    // 플레이어(우치)가 소유하고 있는 소환수(우치 자기자신도 포함)
    public int[] formation;     // 전투에 참여할 포메이션(-1은 빈자리)
    public List<int> charms;   // 우치가 소유하고 있는 부적(5개)

    public List<CharacterInfoData> characterInfoList;

    public BattleData()
    {
        allies = new List<int>() { 0, 1, 2 };
        formation = new int[] { 2, 0, 1, -1};   // 호랑이-우치-삼미호 순(시연버전)
        charms = new List<int>(5);

        if (GameManager.GetInstance.UseDebugCharms)
        {
            charms = GameManager.GetInstance.Charms;
        }
        characterInfoList = new List<CharacterInfoData>();
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

    public CharacterInfoData(int _ID, Stat _baseStat, Stat _levelUpStat, Stat _rewardStat, Level _level, Health _health)
    {
        ID = _ID;
        baseStat = _baseStat;
        levelUpStat = _levelUpStat;
        rewardStat = _rewardStat;
        level = _level;
        health = _health;
    }
}
