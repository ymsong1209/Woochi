using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 3열일때는 적 전체를 대상으로 타격
/// 100% 확률로 화상 디버프
/// </summary>
public class FoxFire_Row3 : BaseSkill
{
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        GameObject burnDebuffPrefab = BuffPrefabList[0];
        GameObject burnDebuffGameObject = Instantiate(burnDebuffPrefab, transform);
        BurnDebuff burnDebuff = burnDebuffGameObject.GetComponent<BurnDebuff>();
        burnDebuff.BuffDurationTurns = 3;
        burnDebuff.ChanceToApplyBuff = 100;
        instantiatedBuffList.Add(burnDebuffGameObject);
        
        base.ActivateSkill(_Opponent);
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        text.text = "여우불(전체)\n" + "대상 전체에게 " + SkillSO.BaseMultiplier +"%의 피해를 주고 \n 화상 부여";
    }
}
