using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollDog : BaseEnemy
{
    [SerializeField] private ScrollDogIncreaseEvasionBuff evasionBuff;
    public override void Initialize()
    {
        base.Initialize();
        GameObject instantiatedEvasionBuff = Instantiate(evasionBuff.gameObject, transform);
        ScrollDogIncreaseEvasionBuff roarBuff = instantiatedEvasionBuff.GetComponent<ScrollDogIncreaseEvasionBuff>();
        ApplyBuff(this,this,roarBuff);
    }
    
    public override void TriggerAI()
    {
        System.Random random = new System.Random();
        int randomValue = random.Next(0, 100); // 0에서 99까지의 값을 랜덤으로 생성
        
        BaseCharacter ally = null;
        
        if (randomValue < 60) //60% 확률로 물기
        {
            int randomValue2 = random.Next(0, 100);
            //60% 확률로 3,4열중 단일 대상을 공격으로 선정
            //3,4열에 대상 없을 경우, 랜덤하게 아군을 찾아 스킬 사용
            if (randomValue2 < 60 && BattleUtils.FindAllyFromIndex(2)) 
            {
                ally = BattleUtils.FindRandomAlly(2, 3);
            }
            else //40%의 확률로 랜덤하게 아군을 찾아 스킬 사용
            {
                ally = BattleUtils.FindRandomAlly(0,1,2,3);
            }
            BattleManager.GetInstance.SkillSelected(activeSkills[0]);
        }
        else //40% 확률로 오싹한 짖기
        {
            //전체 아군 대상으로 스킬 시전
            ally = BattleUtils.FindAllyFromIndex(0);
            BattleManager.GetInstance.SkillSelected(activeSkills[1]);
        }

        BattleManager.GetInstance.CharacterSelected(ally);
        BattleManager.GetInstance.ExecuteSelectedSkill(ally);
    }
}
