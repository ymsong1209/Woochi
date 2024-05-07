using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxFire_Row4 : BaseSkill
{
   [SerializeField] private BaseBuff CureBuff;
   [SerializeField] private BaseBuff BurnBuff;
   public override void ActivateSkill(BaseCharacter _opponent)
   {
      //�Ʊ� ��ȣ ��ų������ ��ȣ �� �� ����
      //���������� �����ؾ��ϴ� �� ����
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

      //�Ʊ� Ÿ�����δ� ü��ȸ��
      if (opponent.IsAlly)
      {
         //�Ʊ��� �� ������ �ݵ�� ����. �˻� ���� �ǳʶ�
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
