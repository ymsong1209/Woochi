using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollDog : BaseCharacter
{
    [SerializeField] private ScrollDogIncreaseEvasionBuff evasionBuff;
    public override void Initialize()
    {
        base.Initialize();
        ApplyBuff(this,evasionBuff);
    }
    
    public override void TriggerAI()
    {
        System.Random random = new System.Random();
        int randomValue = random.Next(0, 100); // 0에서 99까지의 값을 랜덤으로 생성
        //아군 1,2,3,4열 중 랜덤하게 아군 선택
        BaseCharacter ally; 
        
        int skillIndex = randomValue < 60 ? 0 : 1; // 60% 확률로 물기, 40% 확률로 오싹한 짖기
        BattleManager.GetInstance.SkillSelected(activeSkills[skillIndex]);
        if (skillIndex == 0)
        {
            randomValue = random.Next(0, 100);// 0에서 99까지의 값을 랜덤으로 다시 생성
            //60%의 확률로 3,4열 중 단일 대상으로 공격 대상으로 선정
            if (randomValue < 60)
            {
                ally = BattleUtils.FindRandomAlly(2,3);
            }
            //40%의 확률로 1,2,3,4열중 랜덤하게 대상 선정
            else
            {
                ally = BattleUtils.FindRandomAlly(0,1,2,3);
            }
            //60%의 확률을 가진 물기 스킬을 사용할 때, 아군이 없을 경우 1,2열중 랜덤하게 대상 선정
            if (ally == null)
            {
                ally = BattleUtils.FindRandomAlly(0,1);
            }
        }
        //일단은 위에 로직이랑 동일하게. 나중에 추가 로직 생성할것.
        else
        {
            randomValue = random.Next(0, 100);// 0에서 99까지의 값을 랜덤으로 다시 생성
            //60%의 확률로 3,4열 중 단일 대상으로 공격 대상으로 선정
            if (randomValue < 60)
            {
                ally = BattleUtils.FindRandomAlly(2,3);
            }
            //40%의 확률로 1,2,3,4열중 랜덤하게 대상 선정
            else
            {
                ally = BattleUtils.FindRandomAlly(0,1,2,3);
            }
            //60%의 확률을 가진 물기 스킬을 사용할 때, 아군이 없을 경우 1,2열중 랜덤하게 대상 선정
            if (ally == null)
            {
                ally = BattleUtils.FindRandomAlly(0,1);
            }
        }

        BattleManager.GetInstance.CharacterSelected(ally);
        BattleManager.GetInstance.ExecuteSelectedSkill(ally);
    }
}
