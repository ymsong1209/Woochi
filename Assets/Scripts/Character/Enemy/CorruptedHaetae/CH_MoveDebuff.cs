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
                bool negativeBuffFound = false;
                for (int j = targetBuffIcon.transform.childCount - 1; j >= 0; j--)
                {
                    BaseBuff buff = targetBuffIcon.transform.GetChild(j).GetComponent<BaseBuff>();
                    if (buff.BuffType == BuffType.Negative)
                    {
                        opponent.ApplyBuff(SkillOwner, opponent, buff);
                        SkillOwner.activeBuffs.Remove(buff);
                        negativeBuffFound = true;
                    }
                }
                if(negativeBuffFound) IncreaseStat();
                if(targetBuffIcon.transform.childCount == 0) targetBuffIcon.DeActivate();
            }
        }
    }

    private void IncreaseStat()
    {
        GameObject instantiatedStat = Instantiate(StatBuffGameObject, transform);
        StatBuff statBuff = instantiatedStat.GetComponent<StatBuff>();
        statBuff.BuffName = "저주 흡수";
        statBuff.BuffDurationTurns = -1;
        statBuff.changeStat.accuracy = 1;
        statBuff.changeStat.crit = 1;
        statBuff.changeStat.evasion = 1;
        statBuff.changeStat.minStat = 1;
        statBuff.changeStat.maxStat = 1;
        statBuff.changeStat.speed = 1;
        statBuff.changeStat.defense = 1;
        SkillOwner.ApplyBuff(SkillOwner,SkillOwner,statBuff);
        SkillOwner.CheckForStatChange();
    }
    
    
}
