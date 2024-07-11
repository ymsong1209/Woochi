using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadArcher : BaseEnemy
{
    public override void TriggerAI()
    {
        int formationIdx = BattleManager.GetInstance.Enemies.FindCharacterIndex(this);
        Debug.Log(formationIdx);
        if (formationIdx == 0)
        {
            ActivateRetreatShot();
        }
        else
        {
            ActivateUnholyArrow();
        }
    }

    private void ActivateUnholyArrow()
    {
        System.Random random = new System.Random();
        int randomValue = random.Next(0, 100); // 0에서 99까지의 값을 랜덤으로 생성
        
        BaseCharacter ally = null;
        
        if (randomValue < 35) // 60% 확률로 2,3,4열 중 가장 낮은 체력의 아군 선택
        {
            ally = BattleUtils.FindAllyWithLeastHP(1, 2, 3);
        }
        else // 40% 확률로 랜덤하게 선택
        {
            ally = BattleUtils.FindRandomAlly(1, 2, 3);
        }
      
        if (ally != null)
        {
            BattleManager.GetInstance.SkillSelected(activeSkills[0]);
            BattleManager.GetInstance.CharacterSelected(ally);
            BattleManager.GetInstance.ExecuteSelectedSkill(ally);
        }
    }

    private void ActivateRetreatShot()
    {
        BaseCharacter ally = null;
        ally = BattleUtils.FindRandomAlly(0,1);
        if (ally != null)
        {
            BattleManager.GetInstance.SkillSelected(activeSkills[1]);
            BattleManager.GetInstance.CharacterSelected(ally);
            BattleManager.GetInstance.ExecuteSelectedSkill(ally);
        }
    }
}