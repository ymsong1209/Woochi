using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TF_Vision : BaseSkill
{
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        //100%의 확률로 기묘한 환상 디버프 부여
        //중첩시 지속시간과 감소 수치 중첩
        GameObject statDebuffPrefab = BuffPrefabList[0];
        GameObject statDebuffGameObject = Instantiate(statDebuffPrefab, transform);
        StatDeBuff statDebuff = statDebuffGameObject.GetComponent<StatDeBuff>();
        statDebuff.BuffName = "기묘한 환상";
        statDebuff.BuffDurationTurns = 3;
        statDebuff.ChanceToApplyBuff = 100;
        statDebuff.ChangeStat.accuracy = -4;
        instantiatedBuffList.Add(statDebuffGameObject);
        
        base.ActivateSkill(_Opponent);
    }
}
