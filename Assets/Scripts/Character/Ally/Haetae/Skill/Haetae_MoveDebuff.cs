using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 1,2,3,4열중 단일 대상에게 디버프 이동하면서 능력치 상승
/// </summary>
public class Haetae_MoveDebuff : BaseSkill
{
    [SerializeField] private GameObject StatBuffGameObject;
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        base.ActivateSkill(_Opponent);
        if (SkillResult.IsAnyHit())
        {
            TransferBuff(SkillResult.Opponent[0]);
        }
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        Stat finalStat = SkillOwner.FinalStat;
        int minStat = (int)Mathf.Round(finalStat.GetValue(StatType.MinDamage) * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(finalStat.GetValue(StatType.MaxDamage) * SkillSO.BaseMultiplier / 100f);
        text.text = "수신의 율법\n" + 
                    "해태에게 부여된 모든 디버프를 이전하고\n" + 
                    "이전된 디버프의 수 만큼 모든 능력치를 1씩 상승";
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
                    if (buff.BuffType == BuffType.Negative && buff.IsRemovableDuringBattle)
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
        statBuff.changeStat.SetValue(StatType.Speed, 1);
        statBuff.changeStat.SetValue(StatType.Defense, 1);
        statBuff.changeStat.SetValue(StatType.Crit, 1);
        statBuff.changeStat.SetValue(StatType.Accuracy, 1);
        statBuff.changeStat.SetValue(StatType.Evasion, 1);
        statBuff.changeStat.SetValue(StatType.Resist, 1);
        statBuff.changeStat.SetValue(StatType.MinDamage, 1);
        statBuff.changeStat.SetValue(StatType.MaxDamage, 1);
        SkillOwner.ApplyBuff(SkillOwner,SkillOwner,statBuff);
        SkillOwner.CheckForStatChange();
    }
}
