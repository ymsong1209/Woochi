using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 4열일때 적 1,2,3,4열 중 하나를 대상으로 타격
/// 100% 확률로 화상 디버프
/// </summary>
public class FoxFire_Row4_P : BaseSkill
{
   public override void ActivateSkill(BaseCharacter _opponent)
   {
      GameObject burnDebuffPrefab = BuffPrefabList[0];
      GameObject burnDebuffGameObject = Instantiate(burnDebuffPrefab, transform);
      BurnDebuff burnDebuff = burnDebuffGameObject.GetComponent<BurnDebuff>();
      burnDebuff.BuffDurationTurns = 3;
      burnDebuff.ChanceToApplyBuff = 100;
      instantiatedBuffList.Add(burnDebuffGameObject);
      
      base.ActivateSkill(_opponent);
   }
   public override void SetSkillDescription(TextMeshProUGUI text)
   {
      text.text = "여우불(단일)+\n" + "대상에게 " + SkillSO.BaseMultiplier +"%의 피해를 주고 \n 화상 부여";
   }
}
