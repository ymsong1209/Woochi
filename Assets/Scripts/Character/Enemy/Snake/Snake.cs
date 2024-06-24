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
            ally = BattleUtils.FindAllyWithLeastHP(0, 1);
        }
        else // 65% 확률로 랜덤하게 선택
        {
            ally = BattleUtils.FindRandomAlly(0, 1);
        }
      
        if (ally != null)
        {
            BattleManager.GetInstance.SkillSelected(activeSkills[0]);
            BattleManager.GetInstance.CharacterSelected(ally);
            BattleManager.GetInstance.ExecuteSelectedSkill(ally);
        }

      
    }
}