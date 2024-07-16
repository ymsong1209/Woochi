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
        result.health   = Mathf.Clamp(a.health + b.health, 0, 999);
        result.speed    = Mathf.Clamp(a.speed + b.speed, 0, 999);
        result.defense  = Mathf.Clamp(a.defense + b.defense, 0, 999);
        result.crit     = Mathf.Clamp(a.crit + b.crit, 0, 999);
        result.accuracy = Mathf.Clamp(a.accuracy + b.accuracy, 0, 999);
        result.evasion  = Mathf.Clamp(a.evasion + b.evasion, 0, 999);
        result.resist   = Mathf.Clamp(a.resist + b.resist, 0, 999);
        result.minStat  = Mathf.Clamp(a.minStat + b.minStat, 0, 999);
        result.maxStat  = Mathf.Clamp(a.maxStat + b.maxStat, 0, 999);
        return result;
    }
}
