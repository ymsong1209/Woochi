using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
         BaseBuff buff = SkillOwner.ApplyBuff(_opponent, cureBuff);
         cureBuff.SetFox(SkillOwner);
      }
   }
   
}
