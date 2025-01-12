using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_DisasterImage : BaseSkill
{
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        GameObject statDebuffPrefab = BuffPrefabList[0];
        GameObject statDebuffGameObject = Instantiate(statDebuffPrefab, transform);
        StatDeBuff statDebuff = statDebuffGameObject.GetComponent<StatDeBuff>();
        statDebuff.BuffName = "재앙환영";
        statDebuff.BuffDurationTurns = 4;
        statDebuff.IsAlwaysApplyBuff = true;
        statDebuff.BuffStackType = BuffStackType.ResetDuration;
        statDebuff.changeStat.SetValue(StatType.Accuracy, -2);
        statDebuff.changeStat.SetValue(StatType.Evasion, -2);
        statDebuff.changeStat.SetValue(StatType.MinDamage, -2);
        statDebuff.changeStat.SetValue(StatType.MaxDamage, -2);
        statDebuff.changeStat.SetValue(StatType.Speed, -2);
        instantiatedBuffList.Add(statDebuffGameObject);
        base.ActivateSkill(_Opponent);
    }
}
