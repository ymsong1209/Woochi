using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 꼬리불 3,4열 일때는 적 2,3,4열을 대상으로 타격
/// 타격된 적은 20%의 확률로 화상 디버프
/// 원거리 스킬
/// </summary>
public class TailFire_Ranged : BaseSkill
{
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        GameObject burnDebuffPrefab = BuffPrefabList[0];
        GameObject burnDebuffGameObject = Instantiate(burnDebuffPrefab, transform);
        BurnDebuff burnDebuff = burnDebuffGameObject.GetComponent<BurnDebuff>();
        burnDebuff.BuffDurationTurns = 3;
        burnDebuff.ChanceToApplyBuff = 20;
        
        instantiatedBuffList.Add(burnDebuffGameObject);
        
        base.ActivateSkill(_Opponent);
    }
}
