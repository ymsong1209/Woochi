using Newtonsoft.Json;
using UnityEngine;

/// <summary>
/// ������ ��Ÿ���� ����ü
/// </summary>
[System.Serializable]
public struct Level
{
    [JsonIgnore]    public BaseCharacter owner;
    [ReadOnly]      public int  rank;    // �� �ܰ�����
    [ReadOnly]      public int  exp;     // ���� ����ġ

    private int maxRank;

    public Level(Level level)
    {
        owner = null;
        rank = level.rank;
        exp = level.exp;
        maxRank = level.maxRank;
    }

    public void Initialize()
    {
        owner = null;
        rank = 1;
        exp = 0;
        maxRank = 5;
    }

    public void AddExp(int value)
    {
        if (owner.IsDead || IsMaxRank()) return;

        int newExp = exp + value;
        int requireExp = GetRequireExp();

        if(newExp >= requireExp)
        {
            exp = Mathf.Clamp(newExp - requireExp, 0, requireExp);
            rank++;
            owner.onLevelUp();
        }
        else
        {
            exp = newExp;
        }

        Debug.Log($"AddExp: {value}, {exp}/{GetRequireExp()}");
    }

    public bool IsMaxRank()
    {
        return rank >= maxRank;
    }

    private readonly int GetRequireExp()
    {
        return 200 * (int)Mathf.Pow(2.5f, rank) + 0;
    }
}
