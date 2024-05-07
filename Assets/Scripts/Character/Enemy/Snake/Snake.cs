using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : BaseCharacter
{
   public override void TriggerAI()
   {
      System.Random random = new System.Random();
      int randomValue = random.Next(0, 100); // 0���� 99������ ���� �������� ����
        
      BaseCharacter ally = null;
        
      if (randomValue < 35) // 35% Ȯ���� ���� ���� ü���� �Ʊ� ����
      {
         ally = FindAllyWithLeastHP();
      }
      else // 65% Ȯ���� �����ϰ� ����
      {
         ally = FindRandomAllyInFrontRows();
      }
      
      if (ally != null)
      {
         BattleManager.GetInstance.SkillSelected(activeSkills[0]);
         BattleManager.GetInstance.ExecuteSelectedSkill(ally);
      }

      
   }

   public BaseCharacter FindAllyWithLeastHP()
   {
      Formation allies = BattleManager.GetInstance.Allies;
      BaseCharacter characterWithLeastHP = null;
      int lowestHP = int.MaxValue;

      //�Ʊ� 1,2�� �� �ϳ��� ��.
      //�Ʊ� 1,2���� formation[0],formation[1]��.
      for (int i = 0; i < 2; ++i)
      {
         BaseCharacter ally = allies.formation[i];
         if (ally != null)
         {
            int currentHP = ally.Health.CurHealth;
            if (currentHP < lowestHP)
            {
               lowestHP = currentHP;
               characterWithLeastHP = ally;
            }
         }
      }

      return characterWithLeastHP;
   }

   public BaseCharacter FindRandomAllyInFrontRows()
   {
      Formation allies = BattleManager.GetInstance.Allies;
      List<BaseCharacter> frontRowAllies = new List<BaseCharacter>();

      // �Ʊ� 1, 2���� ����Ʈ�� �߰�
      for (int i = 0; i < 2; ++i)
      {
         BaseCharacter ally = allies.formation[i];
         if (ally != null)
         {
            frontRowAllies.Add(ally);
         }
      }

      if (frontRowAllies.Count > 0)
      {
         System.Random random = new System.Random();
         int randomIndex = random.Next(0, frontRowAllies.Count);
         return frontRowAllies[randomIndex];
      }

      return null; // 1, 2���� �Ʊ��� ���� ���
   }
}
