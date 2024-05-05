using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : BaseCharacter
{
   public override void TriggerAI()
   {
      System.Random random = new System.Random();
      int randomValue = random.Next(0, 100); // 0에서 99까지의 값을 랜덤으로 생성
        
      BaseCharacter ally = null;
        
      if (randomValue < 35) // 35% 확률로 가장 낮은 체력의 아군 선택
      {
         ally = FindAllyWithLeastHP();
      }
      else // 65% 확률로 랜덤하게 선택
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

      //아군 1,2열 중 하나를 고름.
      //아군 1,2열은 formation[0],formation[1]임.
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

      // 아군 1, 2열을 리스트에 추가
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

      return null; // 1, 2열에 아군이 없을 경우
   }
}
