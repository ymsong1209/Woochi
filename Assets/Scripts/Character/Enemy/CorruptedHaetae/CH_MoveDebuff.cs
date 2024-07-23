using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_MoveDebuff : BaseSkill
{
    [SerializeField] private GameObject StatBuffGameObject;
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        base.ActivateSkill(_Opponent);
        if (SkillResult.isHit)
        {
            TransferBuff(SkillResult.Opponent);
        }
    }

    private void TransferBuff(BaseCharacter opponent)
    {
        for(int i = SkillOwner.BuffList.BuffIcons.Length - 1; i >= 0; i--)
        {
            BuffIcon targetBuffIcon = SkillOwner.BuffList.BuffIcons[i];
            if (targetBuffIcon && targetBuffIcon.gameObject.activeSelf)
            {
                for (int j = targetBuffIcon.transform.childCount - 1; j >= 0; j--)
                {
                    BaseBuff buff = targetBuffIcon.transform.GetChild(j).GetComponent<BaseBuff>();
                    if (buff.BuffType == BuffType.Negative)
                    {
                        opponent.ApplyBuff(buff.Caster, opponent, buff);
                        SkillOwner.activeBuffs.Remove(buff);
                        IncreaseStat();
                    }
                }
                if(targetBuffIcon.transform.childCount == 0) targetBuffIcon.DeActivate();
            }
        }
    }

    private void IncreaseStat()
    {
        GameObject instantiatedStat = Instantiate(StatBuffGameObject, transform);
        StatBuff statBuff = instantiatedStat.GetComponent<StatBuff>();
        statBuff.BuffName = "부정한 율법";
        statBuff.BuffDurationTurns = -1;
        statBuff.ChangeStat.accuracy = 1;
        statBuff.ChangeStat.crit = 1;
        statBuff.ChangeStat.evasion = 1;
        statBuff.ChangeStat.minStat = 1;
        statBuff.ChangeStat.maxStat = 1;
        statBuff.ChangeStat.speed = 1;
        statBuff.ChangeStat.defense = 1;
        SkillOwner.ApplyBuff(SkillOwner,SkillOwner,statBuff);
        SkillOwner.CheckForStatChange();
    }
    
    
}
