using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxFire_Row4 : BaseSkill
{
   [SerializeField] private GameObject cureBuffPrefab;
   public override void ActivateSkill(BaseCharacter _opponent)
   {
     //아군 보호 스킬등으로 보호 할 수 있음
      //최종적으로 공격해야하는 적 판정
      BaseCharacter opponent = _opponent;
      if (!_opponent.IsAlly)
      {
         opponent = CheckOpponentValid(_opponent);
      }
      
      if(opponent == null)
      {
         Debug.LogError("opponent is null");
         return;
      }
      
      GameObject burnDebuffPrefab = BuffPrefabList[0];
      GameObject burnDebuffGameObject = Instantiate(burnDebuffPrefab, transform);
      BurnDebuff burnDebuff = burnDebuffGameObject.GetComponent<BurnDebuff>();
      burnDebuff.BuffDurationTurns = 3;
      burnDebuff.ChanceToApplyBuff = 100;
      instantiatedBuffList.Add(burnDebuffGameObject);

      //아군 타겟으로는 체력회복
      if (opponent.IsAlly)
      {
         //아군에 준 버프는 반드시 명중. 검사 로직 건너뜀
         GameObject curebuffInstantiated = Instantiate(cureBuffPrefab, transform);
         FoxFire_Row4_CureBuff cureBuff = curebuffInstantiated.GetComponent<FoxFire_Row4_CureBuff>();
         BaseBuff buff = SkillOwner.ApplyBuff(_opponent, cureBuff);
         cureBuff.SetFox(SkillOwner);
      }
      else
      {
         ApplySkill(opponent);
      }
   }
   
}
