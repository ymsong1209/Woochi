using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T1_Sharpen : MainCharacterSkill
{
    public override void ActivateSkill(BaseCharacter _opponent)
    {
        
        GameObject statbuffPrefab = BuffPrefabList[0];
        GameObject statbuffGameObject = Instantiate(statbuffPrefab, transform);
        StatBuff statBuff = statbuffGameObject.GetComponent<StatBuff>();
        statBuff.BuffName = "연마";
        statBuff.BuffDurationTurns = 1;
        statBuff.ChanceToApplyBuff = 100;
        statBuff.changeStat.accuracy = 5;
        statBuff.changeStat.crit = 5;
        instantiatedBuffList.Add(statbuffGameObject);
        
        base.ActivateSkill(_opponent);
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        int minStat = (int)Mathf.Round(SkillOwner.FinalStat.minStat * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(SkillOwner.FinalStat.maxStat * SkillSO.BaseMultiplier / 100f);
        text.text = "연마\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" + 
                    "단일 아군 대상에게 연마 버프 부여";
    }
}
