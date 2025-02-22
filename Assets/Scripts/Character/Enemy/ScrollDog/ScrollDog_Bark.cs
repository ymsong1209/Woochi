using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollDog_Bark : BaseSkill
{
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        GameObject statDebuffPrefab = BuffPrefabList[0];
        GameObject statDebuffGameObject = Instantiate(statDebuffPrefab, transform);
        StatDeBuff statDebuff = statDebuffGameObject.GetComponent<StatDeBuff>();
        statDebuff.BuffName = "오싹함";
        statDebuff.BuffDurationTurns = 1;
        statDebuff.IsAlwaysApplyBuff = true;
        statDebuff.BuffStackType = BuffStackType.ResetDuration;
        statDebuff.BuffStackType = BuffStackType.ResetDuration;
        statDebuff.changeStat.SetValue(StatType.Accuracy, -5);
        instantiatedBuffList.Add(statDebuffGameObject);
        
        base.ActivateSkill(_Opponent);
    }
}
