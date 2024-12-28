using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CT_ResurrectAndBuff : BaseSkill
{
    
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        GameObject statBuffPrefab = BuffPrefabList[0];
        GameObject statBuffGameObject = Instantiate(statBuffPrefab, transform);
        StatBuff statBuff = statBuffGameObject.GetComponent<StatBuff>();
        statBuff.BuffName = "혼령 공명";
        statBuff.BuffDurationTurns = 1;
        statBuff.ChanceToApplyBuff = 100;
        statBuff.changeStat.SetValue(StatType.Accuracy, 5);
        statBuff.changeStat.SetValue(StatType.Evasion, 5);
        statBuff.changeStat.SetValue(StatType.Defense, 5);
        statBuff.changeStat.SetValue(StatType.MinDamage, 5);
        statBuff.changeStat.SetValue(StatType.MaxDamage, 5);
        instantiatedBuffList.Add(statBuffGameObject);
        base.ActivateSkill(_Opponent);
    }
}
