using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Haetae_MoveDebuff_P : BaseSkill
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
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        int minStat = (int)Mathf.Round(SkillOwner.FinalStat.minStat * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(SkillOwner.FinalStat.maxStat * SkillSO.BaseMultiplier / 100f);
        text.text = "수신의 율법+\n" + "해태에게 부여된 모든 디버프를 이전하고\n" + "이전된 디버프의 수 만큼 모든 스탯을 0.5씩 상승";
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
                        opponent.ApplyBuff(buff.Caster, opponent, buff);
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
        statBuff.changeStat.defense = 0.5f;
        statBuff.changeStat.crit = 0.5f;
        statBuff.changeStat.accuracy = 0.5f;
        statBuff.changeStat.evasion = 0.5f;
        statBuff.changeStat.resist = 0.5f;
        statBuff.changeStat.minStat = 0.5f;
        statBuff.changeStat.maxStat = 0.5f;
        SkillOwner.ApplyBuff(SkillOwner,SkillOwner,statBuff);
        SkillOwner.CheckForStatChange();
    }
}