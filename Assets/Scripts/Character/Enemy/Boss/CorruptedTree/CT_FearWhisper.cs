using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CT_FearWhisper : BaseSkill
{
    //누구에게 사용할지는 AI에서 결정
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
       
        GameObject fearDebuffPrefab = BuffPrefabList[0];
        GameObject fearDebuffGameObject = Instantiate(fearDebuffPrefab, transform);
        FearBuff fearBuff = fearDebuffGameObject.GetComponent<FearBuff>();
        fearBuff.ChanceToApplyBuff = 100;
        fearBuff.IsAlwaysApplyBuff = true;
        instantiatedBuffList.Add(fearDebuffGameObject);
        
        base.ActivateSkill(_Opponent);
    }
}
