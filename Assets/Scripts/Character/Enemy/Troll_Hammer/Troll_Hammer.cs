using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troll_Hammer : BaseEnemy
{
    //스킬 사용 횟수 카운트
    private int skillUsageCount = 0;
  
    public override bool CheckTurnEndFromSkillResult(SkillResult result)
    {
        //스킬을 이미 한번 사용했거나 스킬이 성공적으로 적중했을 경우
        if (skillUsageCount > 0 || result.IsAnyHit())
        {
            skillUsageCount = 0;
            return true;
        }
        
        ++skillUsageCount;
        return false;
    }
    public override void TriggerAI()
    {
        System.Random random = new System.Random();
        int randomValue = random.Next(0, 100); // 0에서 99까지의 값을 랜덤으로 생성
        
        BaseCharacter ally = null;
        
        if (randomValue < 70) //70% 확률로 망치 내려찍기
        {
            //아군1,2열 중 단일 캐릭터를 찾아 스킬 사용
            ally = BattleUtils.FindRandomAlly(0,1);
            BattleManager.GetInstance.SkillSelected(activeSkills[0]);
        }
        else //30% 확률로 기묘한 환상
        {
            //랜덤한 단일 아군 대상으로 스킬 시전
            ally = BattleUtils.FindRandomAlly(0, 1, 2, 3);
            BattleManager.GetInstance.SkillSelected(activeSkills[1]);
        }

        BattleManager.GetInstance.CharacterSelected(ally);
        BattleManager.GetInstance.ExecuteSelectedSkill(ally);
    }

    public Troll_Hammer()
    {
        skillUsageCount = 0;
    }
}
