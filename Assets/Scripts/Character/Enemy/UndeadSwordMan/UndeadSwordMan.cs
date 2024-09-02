using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class UndeadSwordMan : BaseEnemy
{
    public AnimatorController controller1;
    public AnimatorController controller2;

    private void Start()
    {
        Animator animator = GetComponent<Animator>();
        int randomValue = UnityEngine.Random.Range(0, 2); // 0 또는 1을 랜덤으로 생성
        if(randomValue == 0)
        {
            animator.runtimeAnimatorController = controller1;
        }
        else
        {
            animator.runtimeAnimatorController = controller2;
        }
    }

    public override void TriggerAI()
    {
        System.Random random = new System.Random();
        int randomValue = random.Next(0, 100); // 0에서 99까지의 값을 랜덤으로 생성
        
        BaseCharacter ally = null;
        
        if (randomValue < 60) // 60% 확률로 가장 낮은 체력의 아군 선택
        {
            ally = BattleUtils.FindAllyWithLeastHP(0, 1,2,3);
        }
        else // 40% 확률로 랜덤하게 선택
        {
            ally = BattleUtils.FindRandomAlly(0, 1,2,3);
        }
      
        if (ally != null)
        {
            BattleManager.GetInstance.SkillSelected(activeSkills[0]);
            BattleManager.GetInstance.CharacterSelected(ally);
            BattleManager.GetInstance.ExecuteSelectedSkill(ally);
        }

      
    }
}
