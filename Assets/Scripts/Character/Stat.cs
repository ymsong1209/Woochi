using System.Collections.Generic;
using DataTable;
using Newtonsoft.Json;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField, JsonProperty] private List<StatValue> list;

    public Stat()
    {
        list = new List<StatValue>()
        {
            new StatValue() { type = StatType.Health, value = 0 },
            new StatValue() { type = StatType.Speed, value = 0 },
            new StatValue() { type = StatType.Defense, value = 0 },
            new StatValue() { type = StatType.Crit, value = 0 },
            new StatValue() { type = StatType.Accuracy, value = 0 },
            new StatValue() { type = StatType.Evasion, value = 0 },
            new StatValue() { type = StatType.Resist, value = 0 },
            new StatValue() { type = StatType.MinDamage, value = 0 },
            new StatValue() { type = StatType.MaxDamage, value = 0 },
        };
    }

    public Stat(Stat stat)
    {
        list = new List<StatValue>(stat.list);
    }

    public Stat(CharacterData data, bool isLevelUp) : this()
    {
        #region 데이터 테이블로부터 스탯 초기화
        if (isLevelUp)
        {
            list[(int)StatType.Health].value = data.add_health;
            list[(int)StatType.Speed].value = data.add_speed;
            list[(int)StatType.Defense].value = data.add_defense;
            list[(int)StatType.Crit].value = data.add_crit;
            list[(int)StatType.Accuracy].value = data.add_accuracy;
            list[(int)StatType.Evasion].value = data.add_evasion;
            list[(int)StatType.Resist].value = data.add_resist;
            list[(int)StatType.MinDamage].value = data.add_minStat;
            list[(int)StatType.MaxDamage].value = data.add_maxStat;
        }
        else
        {
            list[(int)StatType.Health].value = data.health;
            list[(int)StatType.Speed].value = data.speed;
            list[(int)StatType.Defense].value = data.defense;
            list[(int)StatType.Crit].value = data.crit;
            list[(int)StatType.Accuracy].value = data.accuracy;
            list[(int)StatType.Evasion].value = data.evasion;
            list[(int)StatType.Resist].value = data.resist;
            list[(int)StatType.MinDamage].value = data.minStat;
            list[(int)StatType.MaxDamage].value = data.maxStat;
        }
        #endregion
    }
    
    public float GetValue(StatType type)
    {
        if (type == StatType.MinDamage)
        {
            return list[(int)type].value;
        }
        else
        {
            return list[(int)type].value;
        }
    }

    public void SetValue(StatType type, float value) => list[(int)type].value = value;
    public void AddValue(StatType type, float value) => list[(int)type].value += value;
    
    public void AddStat(Stat stat)
    {
        for (int i = 0; i < (int)StatType.END; i++)
        {
            list[i].value += stat.list[i].value;
        }
    }
    
    public void Clamp()
    {
        float value = list[(int)StatType.Health].value;
        list[(int)StatType.Health].value = Mathf.Clamp(value, 1, 999);
        
        for(int i = 1; i < (int)StatType.END; i++)
        {
            value = list[i].value;
            list[i].value = Mathf.Clamp(value, 0, 999);
        }
    }
    
    public static Stat operator +(Stat a, Stat b)
    {
        Stat result = new Stat();
        for (int i = 0; i < (int)StatType.END; i++)
        {
            result.list[i].value = a.list[i].value + b.list[i].value;
        }
        return result;
    }
    
    public List<StatValue> StatList => list;
}

[System.Serializable]
public class StatValue
{
    public StatType type;
    public float value;
}

