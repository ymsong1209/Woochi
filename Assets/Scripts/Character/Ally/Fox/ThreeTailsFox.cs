using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeTailsFox : BaseCharacter
{
    protected override void InitializeSkill()
    {
        DestroyActiveSkills();
        AddFoxSkill();
    }
    public override void CheckSkillsOnTurnStart()
    {
        if(IsDead)
        {
            return;
        }

        DestroyActiveSkills();
        AddFoxSkill();
    }

    private void AddFoxSkill()
    {
        AddTailFire(level.rank);
        AddFoxFire(level.rank);
        AddFascinate(level.rank);
        AddBlueFire(level.rank);
    }
    
    private void AddTailFire(int rank)
    {
        int characterIndex = BattleManager.GetInstance.GetCharacterIndex(this);
        if (characterIndex == 0 || characterIndex == 1)
        {
            InstantiateSkill(rank > 1 ? characterStat.ReinforcedSkills[1] : characterStat.Skills[1]);
        }
        else if (characterIndex == 2 || characterIndex == 3)
        {
            InstantiateSkill(rank > 1 ? characterStat.ReinforcedSkills[0] : characterStat.Skills[0]);
        }
    }
    
    private void AddFoxFire(int rank)
    {
        int characterIndex = BattleManager.GetInstance.GetCharacterIndex(this);
        if (characterIndex == 2)
        {
            InstantiateSkill(rank > 2 ? characterStat.ReinforcedSkills[2] : characterStat.Skills[2]);
        }
        else if (characterIndex == 3)
        {
            InstantiateSkill(rank > 2 ? characterStat.ReinforcedSkills[3] : characterStat.Skills[3]);
        }
        // 3열과 4열 중 어디에도 없을 경우 여우불 없음
    }

    private void AddFascinate(int rank)
    {
        int characterIndex = BattleManager.GetInstance.GetCharacterIndex(this);
        if (characterIndex == 2)
        {
            InstantiateSkill(rank > 3 ? characterStat.ReinforcedSkills[5] : characterStat.Skills[5]);
        }
        else if (characterIndex == 3)
        {
            InstantiateSkill(rank > 3 ? characterStat.ReinforcedSkills[4] : characterStat.Skills[4]);
        }
        // 3열과 4열 중 어디에도 없을 경우 홀리기 없음
    }

    private void AddBlueFire(int rank)
    {
        int characterIndex = BattleManager.GetInstance.GetCharacterIndex(this);
        if (characterIndex == 3)
        {
            InstantiateSkill(rank > 4 ? characterStat.ReinforcedSkills[6] : characterStat.Skills[6]);
        }
        // 4열이 아니면 푸른불 없음
    }
}