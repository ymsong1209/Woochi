using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 4열일때 적 1,2,3,4열 중 하나를 대상으로 타격
/// 100% 확률로 화상 디버프
/// </summary>
public class FoxFire_Row4 : BaseSkill
{
   [SerializeField] private GameObject cureBuffPrefab;
   public override void ActivateSkill(BaseCharacter _opponent)
   {
      GameObject burnDebuffPrefab = BuffPrefabList[0];
      GameObject burnDebuffGameObject = Instantiate(burnDebuffPrefab, transform);
      BurnDebuff burnDebuff = burnDebuffGameObject.GetComponent<BurnDebuff>();
      burnDebuff.BuffDurationTurns = 3;
      burnDebuff.ChanceToApplyBuff = 100;
      instantiatedBuffList.Add(burnDebuffGameObject);
      
      base.ActivateSkill(_opponent);

      if (SkillResult.Opponent && SkillResult.Opponent.IsAlly)
      {
         //아군에 준 버프는 반드시 명중. 검사 로직 건너뜀
         GameObject curebuffInstantiated = Instantiate(cureBuffPrefab, transform);
         FoxFire_Row4_CureBuff cureBuff = curebuffInstantiated.GetComponent<FoxFire_Row4_CureBuff>();
         BaseBuff buff = SkillOwner.ApplyBuff(SkillOwner,_opponent, cureBuff);
         cureBuff.SetFox(SkillOwner);
      }
   }
   
}
