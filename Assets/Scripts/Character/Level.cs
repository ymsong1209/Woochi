using Newtonsoft.Json;
using UnityEngine;

[System.Serializable]
public class Level
{
    [JsonIgnore]    public BaseCharacter owner;
    [ReadOnly]      public int  rank;    // 몇 단계인지
    [ReadOnly]      public int  exp;     // 현재 경험치
    [ReadOnly]      public int  plusExp; // 추가 경험치

    private int maxRank;

    public Level(Level level)
    {
        owner = null;
        rank = level.rank;
        exp = level.exp;
        maxRank = level.maxRank;
    }

    public Level()
    {
        owner = null;
        rank = 1;
        exp = 0;
        maxRank = 5;
        plusExp = 0;
    }

    /// <summary>
    /// 경험치 추가
    /// </summary>
    public bool AddExp()
    {
        if (owner.IsDead || IsMaxRank()) return false;

        int newExp = exp + plusExp;
        plusExp = 0;
        int requireExp = GetRequireExp();

        bool isLevelUp = false;

        if(newExp >= requireExp)
        {
            exp = Mathf.Clamp(newExp - requireExp, 0, requireExp);
            rank++;
            owner.onLevelUp();
            isLevelUp = true;
        }
        else
        {
            exp = newExp;
            isLevelUp = false;
        }

        owner.SaveStat();
        return isLevelUp;
    }

    public bool IsMaxRank()
    {
        return rank >= maxRank;
    }

    public int GetRequireExp()
    {
        return 200 * (int)Mathf.Pow(2.5f, rank) + 0;
    }
}
