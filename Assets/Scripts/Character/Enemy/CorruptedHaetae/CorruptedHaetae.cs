using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptedHaetae : BaseEnemy
{
    public override void TriggerAI()
    {
        System.Random random = new System.Random();
        int randomValue = random.Next(0, 100); // 0에서 99까지의 값을 랜덤으로 생성
        BaseCharacter ally = null;
        // 디버프가 두개 이상
        if (CheckDebuffOverTwo())
        {
            if (randomValue < 35) // 35% 확률로 부정한 찌르기
            {
            
                ally = BattleUtils.FindAllyWithLeastHP(0, 1);
                BattleManager.GetInstance.SkillSelected(activeSkills[1]);
            }
            else if (randomValue < 60) // 25% 확률로 역치
            {
                ally = BattleUtils.FindRandomAlly(0, 1, 2, 3);
                BattleManager.GetInstance.SkillSelected(activeSkills[2]);
            }
            else if (randomValue < 85) // 25% 확률로 재앙의 형상
            {
                ally = BattleUtils.FindRandomAlly(0, 1,2,3);
                BattleManager.GetInstance.SkillSelected(activeSkills[3]);
            }
            else //15% 확률로 부정한 율법
            {
                ally = BattleUtils.FindRandomAlly(0, 1,2,3);
                BattleManager.GetInstance.SkillSelected(activeSkills[0]);
            }
        }
        else
        {
            if (randomValue < 40) // 40% 확률로 부정한 찌르기
            {
           
                ally = BattleUtils.FindAllyWithLeastHP(0, 1);
                BattleManager.GetInstance.SkillSelected(activeSkills[1]);
            }
            else if (randomValue < 70) // 30% 확률로 역치
            {
                ally = BattleUtils.FindRandomAlly(0, 1, 2, 3);
                BattleManager.GetInstance.SkillSelected(activeSkills[2]);
            }
            else // 30% 확률로 재앙의 형상
            {
                ally = BattleUtils.FindRandomAlly(0, 1,2,3);
                BattleManager.GetInstance.SkillSelected(activeSkills[3]);
            }
        }
        BattleManager.GetInstance.CharacterSelected(ally);
        BattleManager.GetInstance.ExecuteSelectedSkill(ally);
    }

    private bool CheckDebuffOverTwo()
    {
        int debuffnum = 0;
        foreach (BuffIcon icon in BuffList.BuffIcons)
        {
            if (icon && icon.gameObject.activeSelf && icon.BuffType == BuffType.Negative)
            {
                ++debuffnum;
            }
        }

        if (debuffnum >= 2) return true;
        return false;
    }
}
