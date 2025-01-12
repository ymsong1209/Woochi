using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CT_ResurrectAndBuff : BaseSkill
{
    
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        GameObject statBuffPrefab = BuffPrefabList[0];
        GameObject statBuffGameObject = Instantiate(statBuffPrefab, transform);
        StatBuff statBuff = statBuffGameObject.GetComponent<StatBuff>();
        statBuff.BuffName = "혼령 공명";
        statBuff.BuffDurationTurns = 1;
        statBuff.IsAlwaysApplyBuff = true;
        statBuff.BuffStackType = BuffStackType.ResetDuration;
        statBuff.changeStat.SetValue(StatType.Accuracy, 5);
        statBuff.changeStat.SetValue(StatType.Evasion, 5);
        statBuff.changeStat.SetValue(StatType.Defense, 5);
        statBuff.changeStat.SetValue(StatType.MinDamage, 5);
        statBuff.changeStat.SetValue(StatType.MaxDamage, 5);
        instantiatedBuffList.Add(statBuffGameObject);
        base.ActivateSkill(_Opponent);
    }
    
    protected override void CustomSkillLogic(BaseCharacter _Opponent)
    {
        CT_LeftSoul leftSoul = _Opponent as CT_LeftSoul;
        if (leftSoul)
        {
            if (leftSoul.SoulDead)
            {
                leftSoul.Body.gameObject.SetActive(true);
                leftSoul.anim.Animator.Play("Skill4");
                leftSoul.Body.sprite = null;
                leftSoul.onPlayAnimation?.Invoke(AnimationType.Skill1);
            }
            else
            {
                leftSoul.onPlayAnimation?.Invoke(AnimationType.Heal);
            }
        }
        
        CT_RightSoul rightSoul = _Opponent as CT_RightSoul;
        if (rightSoul)
        {
            if (rightSoul.SoulDead)
            {
                rightSoul.Body.gameObject.SetActive(true);
                rightSoul.anim.Animator.Play("Skill4");
                rightSoul.Body.sprite = null;
                rightSoul.onPlayAnimation?.Invoke(AnimationType.Skill1);
            }
            else
            {
                rightSoul.onPlayAnimation?.Invoke(AnimationType.Heal);
            }
        }
    }
}
