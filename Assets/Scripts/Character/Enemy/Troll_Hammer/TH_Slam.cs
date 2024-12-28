using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TH_Slam : BaseSkill
{
    [SerializeField] protected int buffProbability = 40;
    
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        //40%의 확률로 적을 기절시키는 디버프 부여
        GameObject stunDebuffPrefab = BuffPrefabList[0];
        GameObject stunDebuffGameObject = Instantiate(stunDebuffPrefab, transform);
        StunDeBuff stunDebuff = stunDebuffGameObject.GetComponent<StunDeBuff>();
        stunDebuff.BuffDurationTurns = 1;
        stunDebuff.ChanceToApplyBuff = buffProbability;
        instantiatedBuffList.Add(stunDebuffGameObject);
        
        base.ActivateSkill(_Opponent);
    }
}
