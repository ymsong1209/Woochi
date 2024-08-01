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
        statDebuff.ChanceToApplyBuff = 100;
        statDebuff.changeStat.accuracy = -2;
        statDebuff.changeStat.evasion = -2;
        statDebuff.changeStat.minStat = -2;
        statDebuff.changeStat.maxStat = -2;
        statDebuff.changeStat.speed = -2;
        instantiatedBuffList.Add(statDebuffGameObject);
        base.ActivateSkill(_Opponent);
    }
}
