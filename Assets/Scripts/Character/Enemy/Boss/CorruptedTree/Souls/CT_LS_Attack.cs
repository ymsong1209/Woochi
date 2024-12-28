using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CT_LS_Attack : BaseSkill
{
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        //25%의 확률로 적을 기절시키는 디버프 부여
        GameObject stunDebuffPrefab = BuffPrefabList[0];
        GameObject stunDebuffGameObject = Instantiate(stunDebuffPrefab, transform);
        StunDeBuff stunDebuff = stunDebuffGameObject.GetComponent<StunDeBuff>();
        stunDebuff.BuffDurationTurns = 1;
        stunDebuff.ChanceToApplyBuff = 25;
        
        instantiatedBuffList.Add(stunDebuffGameObject);
        
        base.ActivateSkill(_Opponent);
    }
}
