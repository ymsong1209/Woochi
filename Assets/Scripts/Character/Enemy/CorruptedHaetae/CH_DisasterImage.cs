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
        statDebuff.ChangeStat.accuracy = -2;
        statDebuff.ChangeStat.evasion = -2;
        statDebuff.ChangeStat.minStat = -2;
        statDebuff.ChangeStat.maxStat = -2;
        statDebuff.ChangeStat.speed = -2;
        instantiatedBuffList.Add(statDebuffGameObject);
        BaseCharacter opponent = BattleUtils.FindRandomAlly(0, 1, 2, 3);
        base.ActivateSkill(opponent);
    }
}
