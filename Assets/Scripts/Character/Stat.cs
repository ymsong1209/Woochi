using DataTable;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [ReadOnly(true)] public int health;
    [ReadOnly(true)] public int speed;
    [ReadOnly(true)] public float defense;
    [ReadOnly(true)] public float crit;
    [ReadOnly(true)] public float accuracy;
    [ReadOnly(true)] public float evasion;
    [ReadOnly(true)] public float resist;
    [ReadOnly(true)] public float minStat;
    [ReadOnly(true)] public float maxStat;

    public Stat()
    {
        health = 0; speed = 0; defense = 0; crit = 0; accuracy = 0; evasion = 0; resist = 0; minStat = 0; maxStat = 0;
    }

    public Stat(CharacterData data)
    {
        health = Mathf.Clamp(data.health, 0, 999);
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
        health = stat.health;
        speed = stat.speed;
        defense = stat.defense;
        crit = stat.crit;
        accuracy = stat.accuracy;
        evasion = stat.evasion;
        resist = stat.resist;
        minStat = stat.minStat;
        maxStat = stat.maxStat;
    }

    public static Stat operator+(Stat a, Stat b)
    {
        Stat result = new Stat();
        result.health = a.health + b.health;
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
