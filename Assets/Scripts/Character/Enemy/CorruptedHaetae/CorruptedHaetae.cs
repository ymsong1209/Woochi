using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptedHaetae : BaseCharacter
{
    public override void TriggerAI()
    {
        // 디버프가 두개 이상일때 부정한 율법 시전
        if (CheckDebuffOverTwo())
        {
            BattleManager.GetInstance.SkillSelected(activeSkills[0]);
            BattleManager.GetInstance.ExecuteSelectedSkill(BattleUtils.FindRandomAlly(0, 1,2,3));
            return;
        }

        System.Random random = new System.Random();
        int randomValue = random.Next(0, 100); // 0에서 99까지의 값을 랜덤으로 생성
        
        BaseCharacter ally = null;
        
        if (randomValue < 50) // 50% 확률로 부정한 찌르기
        {
           
            ally = BattleUtils.FindAllyWithLeastHP(0, 1);
            BattleManager.GetInstance.SkillSelected(activeSkills[1]);
            BattleManager.GetInstance.ExecuteSelectedSkill(ally);
        }
        else if (randomValue < 75) // 25% 확률로 역치
        {
            ally = BattleUtils.FindRandomAlly(0, 1, 2, 3);
            BattleManager.GetInstance.SkillSelected(activeSkills[2]);
            BattleManager.GetInstance.ExecuteSelectedSkill(ally);
        }
        else // 25% 확률로 재앙의 형상
        {
            ally = BattleUtils.FindRandomAlly(0, 1,2,3);
            BattleManager.GetInstance.SkillSelected(activeSkills[3]);
            BattleManager.GetInstance.ExecuteSelectedSkill(ally);
        }
        
     
    }

    private bool CheckDebuffOverTwo()
    {
        return true;
    }
}
