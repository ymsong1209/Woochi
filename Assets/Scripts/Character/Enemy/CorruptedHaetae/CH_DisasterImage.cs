using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_DisasterImage : BaseSkill
{
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        GameObject statDebuffPrefab = Bufflist[0];
        GameObject statDebuffGameObject = Instantiate(statDebuffPrefab, transform);
        StatDeBuff statDebuff = statDebuffGameObject.GetComponent<StatDeBuff>();
        statDebuff.StatBuffName = "재앙환영";
        statDebuff.BuffDurationTurns = 4;
        statDebuff.ChanceToApplyBuff = 100;
        statDebuff.ChangeAccuracy = -2;
        statDebuff.ChangeEvasion = -2;
        statDebuff.ChangeMinStat = -2;
        statDebuff.ChangeMaxStat = -2;
        statDebuff.ChangeSpeed = -2;
        instantiatedBuffList.Add(statDebuffGameObject);
        BaseCharacter opponent = BattleUtils.FindRandomAlly(0, 1, 2, 3);
        base.ActivateSkill(opponent);
    }
}
