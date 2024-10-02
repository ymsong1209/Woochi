using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Troll_Hammer : BaseElite
{
    //스킬 사용 횟수 카운트
    private int skillUsageCount = 0;
  
    public override bool CheckTurnEndFromSkillResult(SkillResult result)
    {
        //스킬을 이미 한번 사용했거나 스킬이 성공적으로 적중했을 경우
        if (skillUsageCount == 1 || result.IsAnyHit())
        {
            skillUsageCount = 0;
            return true;
        }
        
        if (result.IsAnyHit() == false)
        {
            ++skillUsageCount;
            return false;
        }
        return true;
    }
    public override void TriggerAI()
    {
        System.Random random = new System.Random();
        int randomValue = random.Next(0, 100); // 0에서 99까지의 값을 랜덤으로 생성
        
        BaseCharacter ally = null;
        
        if (randomValue < 40) //40% 확률로 망치 내려찍기
        {
            //아군1,2열 중 단일 캐릭터를 찾아 스킬 사용
            ally = BattleUtils.FindRandomAlly(0,1);
            BattleManager.GetInstance.SkillSelected(activeSkills[0]);
        }
        else if (randomValue < 70) //30% 확률로 몸통박치기
        {
            //아군 1열을 대상으로 스킬 사용
            ally = BattleUtils.FindAllyFromIndex(0);
            BattleManager.GetInstance.SkillSelected(activeSkills[1]);
        }
        else //30% 확률로 기묘한 환상
        {
            //랜덤한 단일 아군 대상으로 스킬 시전
            ally = BattleUtils.FindRandomAlly(0, 1, 2, 3);
            BattleManager.GetInstance.SkillSelected(activeSkills[2]);
        }

        BattleManager.GetInstance.CharacterSelected(ally);
        BattleManager.GetInstance.ExecuteSelectedSkill(ally);
    }

    public E_Troll_Hammer()
    {
        skillUsageCount = 0;
    }
}
