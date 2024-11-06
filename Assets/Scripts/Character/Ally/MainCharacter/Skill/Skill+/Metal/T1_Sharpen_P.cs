using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T1_Sharpen_P : MainCharacterSkill
{
    public override void ActivateSkill(BaseCharacter _opponent)
    {
        
        GameObject statbuffPrefab = BuffPrefabList[0];
        GameObject statbuffGameObject = Instantiate(statbuffPrefab, transform);
        StatBuff statBuff = statbuffGameObject.GetComponent<StatBuff>();
        statBuff.BuffName = "연마+";
        statBuff.BuffDurationTurns = 1;
        statBuff.IsAlwaysApplyBuff = true;
        statBuff.changeStat.SetValue(StatType.Accuracy, 10);
        statBuff.changeStat.SetValue(StatType.Crit, 10);
        instantiatedBuffList.Add(statbuffGameObject);
        
        base.ActivateSkill(_opponent);
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        Stat finalStat = SkillOwner.FinalStat;
        int minStat = (int)Mathf.Round(finalStat.GetValue(StatType.MinDamage) * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(finalStat.GetValue(StatType.MaxDamage) * SkillSO.BaseMultiplier / 100f);
        text.text = "연마+\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" + 
                    "단일 아군 대상에게 1턴동안 명중,치명 10만큼 부여";
    }
}