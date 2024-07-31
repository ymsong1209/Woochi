using DataTable;
using UnityEngine;

[System.Serializable]
public struct Stat
{
    [ReadOnly(true)] public int maxHealth;
    [ReadOnly(true)] public int speed;
    [ReadOnly(true)] public float defense;
    [ReadOnly(true)] public float crit;
    [ReadOnly(true)] public float accuracy;
    [ReadOnly(true)] public float evasion;
    [ReadOnly(true)] public float resist;
    [ReadOnly(true)] public float minStat;
    [ReadOnly(true)] public float maxStat;

    public Stat(CharacterData data)
    {
        maxHealth = Mathf.Clamp(data.health, 0, 999);
        speed = Mathf.Clamp(data.speed, 0, 999);
        defense = Mathf.Clamp(data.defense, 0, 999);
        crit = Mathf.Clamp(data.crit, 0, 999);
        accuracy = Mathf.Clamp(data.accuracy, 0, 999);
        evasion = Mathf.Clamp(data.evasion, 0, 999);
        resist = Mathf.Clamp(data.resist, 0, 999);
        minStat = Mathf.Clamp(data.minStat, 0, 999);
        maxStat = Mathf.Clamp(data.maxStat, 0, 999);
    }

    public Stat(Stat stat)
    {
        maxHealth = stat.maxHealth;
        speed = stat.speed;
        defense = stat.defense;
        crit = stat.crit;
        accuracy = stat.accuracy;
        evasion = stat.evasion;
        resist = stat.resist;
        minStat = stat.minStat;
        maxStat = stat.maxStat;
    }

    public void Clamp()
    {
        maxHealth = Mathf.Clamp(maxHealth, 1, 999);
        speed = Mathf.Clamp(speed, 0, 999);
        defense = Mathf.Clamp(defense, 0, 999);
        crit = Mathf.Clamp(crit, 0, 999);
        accuracy = Mathf.Clamp(accuracy, 0, 999);
        evasion = Mathf.Clamp(evasion, 0, 999);
        resist = Mathf.Clamp(resist, 0, 999);
        minStat = Mathf.Clamp(minStat, 0, 999);
        maxStat = Mathf.Clamp(maxStat, 0, 999);
    }

    public static Stat operator+(Stat a, Stat b)
    {
        Stat result = new Stat();
        result.maxHealth = a.maxHealth + b.maxHealth;
        result.speed = a.speed + b.speed;
        result.defense = a.defense + b.defense;
        result.crit = a.crit + b.crit;
        result.accuracy = a.accuracy + b.accuracy;
        result.evasion = a.evasion + b.evasion;
        result.resist = a.resist + b.resist;
        result.minStat = a.minStat + b.minStat;
        result.maxStat = a.maxStat + b.maxStat;
        return result;
    }
}
