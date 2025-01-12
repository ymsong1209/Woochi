using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Haetae_DisasterImage : BaseSkill
{
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        GameObject statDebuffPrefab = BuffPrefabList[0];
        GameObject statDebuffGameObject = Instantiate(statDebuffPrefab, transform);
        StatDeBuff statDebuff = statDebuffGameObject.GetComponent<StatDeBuff>();
        statDebuff.BuffName = "죄악환영";
        statDebuff.BuffDurationTurns = 3;
        statDebuff.IsAlwaysApplyBuff = true;
        statDebuff.BuffStackType = BuffStackType.ResetDuration;
        statDebuff.changeStat.SetValue(StatType.Accuracy, -2);
        statDebuff.changeStat.SetValue(StatType.Evasion, -2);
        statDebuff.changeStat.SetValue(StatType.MinDamage, -2);
        statDebuff.changeStat.SetValue(StatType.MaxDamage, -2);
        statDebuff.changeStat.SetValue(StatType.Speed, -2);
        instantiatedBuffList.Add(statDebuffGameObject);
        base.ActivateSkill(_Opponent);
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        Stat finalStat = SkillOwner.FinalStat;
        int minStat = (int)Mathf.Round(finalStat.GetValue(StatType.MinDamage) * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(finalStat.GetValue(StatType.MaxDamage) * SkillSO.BaseMultiplier / 100f);
        text.text = "죄악의 형상\n" + 
                    "대상에게 " + minStat + " ~ " + maxStat + "의 피해를 주고\n" + 
                    "3턴동안 명중, 회피, 최소, 최대스탯, 속도 -2만큼 부여";
    }
}