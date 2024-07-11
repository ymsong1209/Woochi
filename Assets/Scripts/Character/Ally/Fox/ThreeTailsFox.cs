using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeTailsFox : BaseCharacter
{
    public override void CheckSkillsOnTurnStart()
    {
        if(IsDead)
        {
            return;
        }

        DestroyActiveSkills();
        int characterIndex = BattleManager.GetInstance.GetCharacterIndex(this);
    
        // 꼬리불 처리
        if (characterIndex == 0 || characterIndex == 1)
        {
            InstantiateSkill(characterStat.Skills[1]);
        }
        else if(characterIndex == 2 || characterIndex == 3)
        {
            InstantiateSkill(characterStat.Skills[0]);
        }

        // 여우불
        if (characterIndex == 2)
        {
            InstantiateSkill(characterStat.Skills[2]);
        }
        else if(characterIndex == 3)
        {
            InstantiateSkill(characterStat.Skills[3]);
        }
        // 3열과 4열중 어디에도 없을 경우 여우불 없음

        // 홀리기
        if (characterIndex == 2)
        {
            InstantiateSkill(characterStat.Skills[5]);
        }
        else if(characterIndex == 3)
        {
            InstantiateSkill(characterStat.Skills[4]);
        }
        // 3열과 4열중 어디에도 없을 경우 홀리기 없음
    }
}