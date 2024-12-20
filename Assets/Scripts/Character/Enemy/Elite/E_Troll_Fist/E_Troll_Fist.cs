using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Troll_Fist : BaseElite
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
        
        if (randomValue < 50) //50% 확률로 몸통박치기
        {
            //아군1열의 캐릭터를 찾아 스킬 사용
            ally = BattleUtils.FindAllyFromIndex(0);
            BattleManager.GetInstance.SkillSelected(activeSkills[0]);
        }
        else if (randomValue < 80) //30% 확률로 기묘한 환상
        {
            //랜덤한 단일 아군 대상으로 스킬 시전
            ally = BattleUtils.FindRandomAlly(0, 1, 2, 3);
            BattleManager.GetInstance.SkillSelected(activeSkills[1]);
        }
        else //20% 확률로 괴력
        {
            //아군1열의 캐릭터를 찾아 스킬 사용
            ally = BattleUtils.FindAllyFromIndex(0);
            BattleManager.GetInstance.SkillSelected(activeSkills[2]);
        }
        
        BattleManager.GetInstance.CharacterSelected(ally);
        BattleManager.GetInstance.ExecuteSelectedSkill(ally);
    }

    public E_Troll_Fist()
    {
        skillUsageCount = 0;
    }
}
