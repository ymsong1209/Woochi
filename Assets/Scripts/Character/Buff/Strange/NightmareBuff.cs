using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightmareBuff : StatDeBuff
{
    public override void AddBuff(BaseCharacter caster, BaseCharacter _buffOwner)
    {
        // Health 제외
        StatType randomStat = (StatType)Random.Range(1, (int)StatType.MaxDamage);
        changeStat.SetValue(randomStat, -5);
        
        base.AddBuff(caster, _buffOwner);
    }
}
