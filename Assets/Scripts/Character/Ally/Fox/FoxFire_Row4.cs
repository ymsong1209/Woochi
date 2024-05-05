using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxFire_Row4 : BaseSkill
{
   [SerializeField] private BaseBuff CureBuff;
   [SerializeField] private BaseBuff BurnBuff;
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

      //아군 타겟으로는 체력회복
      if (opponent.IsAlly)
      {
         //아군에 준 버프는 반드시 명중. 검사 로직 건너뜀
         BaseBuff buff = ApplyBuff(_opponent, CureBuff);
         if (buff is FoxFire_Row4_CureBuff foxbuff)
         {
            foxbuff.SetFox(SkillOwner);
         }
      }
      else
      {
         ApplySkill(opponent);
      }
   }
   
}
