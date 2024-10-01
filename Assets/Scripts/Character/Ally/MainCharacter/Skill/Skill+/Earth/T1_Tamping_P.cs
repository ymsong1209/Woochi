using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T1_Tamping_P : MainCharacterSkill
{
    public override void ActivateSkill(BaseCharacter _opponent)
    {
        
        GameObject statbuffPrefab = BuffPrefabList[0];
        GameObject statbuffGameObject = Instantiate(statbuffPrefab, transform);
        StatBuff statBuff = statbuffGameObject.GetComponent<StatBuff>();
        statBuff.BuffName = "안정된 자세+";
        statBuff.BuffDurationTurns = 3;
        statBuff.IsAlwaysApplyBuff = true;
        statBuff.changeStat.resist = 10;
        instantiatedBuffList.Add(statbuffGameObject);
        
        base.ActivateSkill(_opponent);
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        int minStat = (int)Mathf.Round(SkillOwner.FinalStat.minStat * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(SkillOwner.FinalStat.maxStat * SkillSO.BaseMultiplier / 100f);
        text.text = "땅 다지기+\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" + 
                    "아군 전체에게 3턴동안 저항 10만큼 부여";
    }
}